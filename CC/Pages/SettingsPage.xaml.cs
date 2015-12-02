using CC.Common;
using CC.Models;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
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
        }

        private void TipsPageTapped(object sender, TappedRoutedEventArgs e)
        {

            Frame.Navigate(typeof(TipsPage), this.splitView.IsPaneOpen);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e != null && e.Parameter is bool)
            {
                this.splitView.IsPaneOpen = Window.Current.Bounds.Width < App.MIN_WIDTH ? false : (bool)e.Parameter;
            }
            this.tbCurrent.Text = CreditCardManager.GetInstance().ToString();
            Frame.BackStack.Clear();
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
            
            //this.BackButtonHandler();
        }

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
            md.ShowAsync();
        }

        private void ToastSwitchChanged(object sender, RoutedEventArgs e)
        {
            UserSettings.IsToastOn = this.toastSwitch.IsOn;
        }

        private void ToastTimeChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (!this.isInited) return;
            UserSettings.ToastTime = this.toastSlider.Value;
        }

        private void TileSwitchChanged(object sender, RoutedEventArgs e)
        {
            UserSettings.IsTileOn = this.tileSwitch.IsOn;
            //BackgroundTask.UpdatePrimaryTile();
        }

        private void TileRefreshDayChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (!this.isInited) return;
            UserSettings.TileDay = this.tileSlider.Value;
            //BackgroundTask.UpdatePrimaryTile();
        }

        private void BackButtonHandler()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                // 如果设备有后退按钮，那么同样处理下。
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonsBackPressed;
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsBackPressed;
            }
        }

        private void HardwareButtonsBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }
    }
}
