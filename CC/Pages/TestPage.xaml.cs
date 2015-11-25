using CC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class TestPage : Page
    {

        //private CreditCardEditor cce = new CreditCardEditor();

        public TestPage()
        {
            this.InitializeComponent();
            this.testCode();
        }



        private void testCode()
        {
            //var zhaoshangCard = new CreditCard(Bank.Zhaoshang, "8888", 20, 5);
            //var jiansheCard = new CreditCard(Bank.Jianshe, "1234", 5, 20);
            //var jiaotongCard = new CreditCard(Bank.Jiaotong, "2333", 25, 8);
            //var zhongxinCard = new CreditCard(Bank.Zhongxin, "6666", 10, 3);

            var ccm = CreditCardManager.GetInstance();
            //ccm.AddCard(zhaoshangCard);
            //ccm.AddCard(jiansheCard);
            //ccm.AddCard(jiaotongCard);
            //ccm.AddCard(zhongxinCard);
            this.gvCards.ItemsSource = ccm.GetAllCards();

            var cce = new CreditCardEditor()
            {
                Status = Status.OK,
                Card = CreditCard.DefaultCard
            };
            this.splitAddCardView.DataContext = cce;
        }

        private void SettingTapped(object sender, TappedRoutedEventArgs e)
        {

            Frame.Navigate(typeof(TestPage));
        }

        private void AddToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            var cce = new CreditCardEditor()
            {
                Status = Status.Adding,
                Card = CreditCard.DefaultCard
            };
            this.splitAddCardView.DataContext = cce;
        }
        
        private void CreditCardTapped(object sender, TappedRoutedEventArgs e)
        {
            var gridView = sender as GridView;
            var cce = new CreditCardEditor()
            {
                Status = Status.Editing,
                Card = gridView.SelectedItem as CreditCard
            };
            this.splitAddCardView.DataContext = cce;
        }
    }
}
