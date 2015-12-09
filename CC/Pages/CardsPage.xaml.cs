using CC.Common.Models;
using CC.Views;
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
    public sealed partial class CardsPage : Page
    {

        private CreditCardEditor cce = new CreditCardEditor();
        private CreditCardManager ccm = CreditCardManager.GetInstance();
        private DateTime openDate = DateTime.Now;

        public CardsPage()
        {
            this.InitializeComponent();
            this.Init();
            //this.TestCode();
        }

        #region Navigation

        private void TipsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TipsPage), this.splitView.IsPaneOpen);
            this.splitView.IsPaneOpen = false;
        }

        private void SettingsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage), this.splitView.IsPaneOpen);
            this.splitView.IsPaneOpen = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter is bool)
            {
                this.splitView.IsPaneOpen = this.splitView.DisplayMode == SplitViewDisplayMode.Overlay ? false : (bool)e.Parameter;
            }
            Frame.BackStack.Clear();
            this.ReloadGridViewIfNeed();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.ReloadGridViewIfNeed();
        }

        #endregion


        private void Init()
        {
            this.gvCards.ItemsSource = CreditCardManager.GetInstance().GetAllCards();
            this.splitAddCardView.DataContext = cce;
            cce.setEditor(Status.OK, CreditCard.DefaultCard);
        }

        private void ReloadGridViewIfNeed()
        {
            if (DateTime.Now.DayOfYear != this.openDate.DayOfYear)
            {
                this.openDate = DateTime.Now;
                this.gvCards.ItemsSource = null;
                this.gvCards.ItemsSource = CreditCardManager.GetInstance().GetAllCards();
            }
        }

        private void CreditCardTapped(object sender, TappedRoutedEventArgs e)
        {
            var ccuc = sender as CreditCardUserControl;
            if (ccuc != null)
            {
                cce.setEditor(Status.Editing, ccuc.DataContext as CreditCard);
            }
        }

        private void AddButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            cce.setEditor(Status.Adding, CreditCard.DefaultCard);
        }

        private void TestCode()
        {
            var zhaoshangCard = new CreditCard(Bank.Zhaoshang, "Young", "8888", 20, 5);
            var jiansheCard = new CreditCard(Bank.Jianshe, "", "1234", 5, 20);
            var jiaotongCard = new CreditCard(Bank.Jiaotong, "中中中中中中中中中中", "2333", 25, 8);
            var zhongxinCard = new CreditCard(Bank.Zhongxin, "iBai", "6666", 10, 3);
            ccm.AddCard(zhaoshangCard);
            ccm.AddCard(jiansheCard);
            ccm.AddCard(jiaotongCard);
            ccm.AddCard(zhongxinCard);
        }

    }
}
