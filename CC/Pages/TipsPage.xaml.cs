using System;
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
    public sealed partial class TipsPage : Page
    {
        public TipsPage()
        {
            this.InitializeComponent();
        }

        #region Navigation
        
        private void CardsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CardsPage), this.splitView.IsPaneOpen);
        }

        private void SettingsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage), this.splitView.IsPaneOpen);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e != null && e.Parameter is bool)
            {
                this.splitView.IsPaneOpen = Window.Current.Bounds.Width < App.MIN_WIDTH ? false : (bool)e.Parameter;
            }
        }

        #endregion

        private void SMZDMButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://www.smzdm.com"));
        }

        private void SMZDMBYButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://post.smzdm.com/p/125780"));
        }

        private void SMZDMFQQXButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://post.smzdm.com/p/74738"));
        }
    }
}
