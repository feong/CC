using CC.Models;
using CC.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

        public CardsPage()
        {
            this.InitializeComponent();
            this.Init();
            this.TestCode();
        }

        #region Navigation
        
        private void TipsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TipsPage));
        }

        private void SettingsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        #endregion

        private void Init()
        {
            this.gvCards.ItemsSource = CreditCardManager.GetInstance().GetAllCards();
            this.splitAddCardView.DataContext = cce;
            cce.setEditor(Status.OK, CreditCard.DefaultCard);
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
            var ccm = CreditCardManager.GetInstance();
            var zhaoshangCard = new CreditCard(Bank.Zhaoshang, "Young", "8888", 20, 5);
            var jiansheCard = new CreditCard(Bank.Jianshe, "", "1234", 5, 20);
            var jiaotongCard = new CreditCard(Bank.Jiaotong, "", "2333", 25, 8);
            var zhongxinCard = new CreditCard(Bank.Zhongxin, "iBai", "6666", 10, 3);
            ccm.AddCard(zhaoshangCard);
            ccm.AddCard(jiansheCard);
            ccm.AddCard(jiaotongCard);
            ccm.AddCard(zhongxinCard);
        }
    }
}
