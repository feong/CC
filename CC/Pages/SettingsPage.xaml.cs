using CC.Common;
using CC.Common.Models;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace CC.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private bool isInited;

        public SettingsPage()
        {
            this.InitializeComponent();
            this.Init();
            this.isInited = true;
        }

        #region Navigation

        private void CardsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CardsPage), this.splitView.IsPaneOpen);
            this.splitView.IsPaneOpen = false;
        }

        private void TipsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TipsPage), this.splitView.IsPaneOpen);
            this.splitView.IsPaneOpen = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter is bool)
            {
                this.splitView.IsPaneOpen = this.splitView.DisplayMode == SplitViewDisplayMode.Overlay ? false : (bool)e.Parameter;
            }
            this.tbCurrent.Text = CreditCardManager.GetInstance().ToString();
        }

        #endregion
        
        private void Init()
        {
            var version = Package.Current.Id.Version;
            this.tbVersion.Text = version.ToString();
            this.tbVersion.Text = String.Format("{0}.{1}", version.Major, version.Minor);

            this.toastSwitch.IsOn = UserSettings.IsToastOn;
            this.tileSwitch.IsOn = UserSettings.IsTileOn;
            this.toastSlider.Value = UserSettings.ToastTime;
            this.tileSlider.Value = UserSettings.TileDay;
        }

        #region Toast & Tile

        private void ToastSwitchChanged(object sender, RoutedEventArgs e)
        {
            if (!this.isInited) return;

            UserSettings.IsToastOn = this.toastSwitch.IsOn;
            if (this.toastSwitch.IsOn)
            {
                BackgroundTaskRegister.RegisterBackgroundTask();
                new Task(() =>
                {
                    ToastTileManager.MakeAToast();
                }).Start();
            }
        }

        private void ToastTimeChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (!this.isInited) return;

            UserSettings.ToastTime = this.toastSlider.Value;
            new Task(() =>
            {
                ToastTileManager.MakeAToast();
            }).Start();
        }

        private void TileSwitchChanged(object sender, RoutedEventArgs e)
        {
            if (!this.isInited) return;

            UserSettings.IsTileOn = this.tileSwitch.IsOn;
            if (this.tileSwitch.IsOn)
            {
                BackgroundTaskRegister.RegisterBackgroundTask();
                new Task(() =>
                {
                    ToastTileManager.UpdatePrimaryTile();
                }).Start();
            }
        }

        private void TileRefreshDayChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (!this.isInited) return;

            UserSettings.TileDay = this.tileSlider.Value;
            new Task(() =>
            {
                ToastTileManager.UpdatePrimaryTile();
            }).Start();
        }

        #endregion

        #region Copy & Restore

        private void CopyButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(this.tbCurrent.Text);
            Clipboard.SetContent(dataPackage);
        }

        private void RestoreButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.tbInput.Text.Equals("")) return;

            MessageDialog md = new MessageDialog("确定要还原片信息吗？该操作不可逆，建议先保存好当前的用卡设置！", "询问");
            md.Commands.Add(new UICommand("确定", cmd =>
            {
                CreditCardManager.GetInstance().LoadCards(this.tbInput.Text);
                this.tbCurrent.Text = CreditCardManager.GetInstance().ToString();
            }, 0));
            md.Commands.Add(new UICommand("放弃", cmd =>
            {
            }, 1));
            md.DefaultCommandIndex = 0;
            md.CancelCommandIndex = 1;
            var x = md.ShowAsync();
        }

        #endregion
        
    }
}
