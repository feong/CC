﻿using CC.Models;
using CC.Views;
using System;
using System.IO;
using System.Linq;
using System.Net;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
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
        }

        private void SettingsPageTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage), this.splitView.IsPaneOpen);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null && e.Parameter is bool)
            {
                this.splitView.IsPaneOpen = Window.Current.Bounds.Width < App.MIN_WIDTH ? false : (bool)e.Parameter;
            }
            Frame.BackStack.Clear();
        }

        #endregion

        private void Init()
        {
            this.gvCards.ItemsSource = CreditCardManager.GetInstance().GetAllCards();
            this.splitAddCardView.DataContext = cce;
            cce.setEditor(Status.OK, CreditCard.DefaultCard);

            //this.BackButtonHandler();
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
                this.ResetBackStack(frame);
                this.Frame.GoBack();
                e.Handled = true;
            }
        }

        private void ResetBackStack(Frame frame)
        {
            PageStackEntry mainPage = frame.BackStack.Where(b => b.SourcePageType == typeof(CreditCard)).FirstOrDefault();
            frame.BackStack.Clear();
            if (mainPage != null)
            {
                frame.BackStack.Add(mainPage);
            }
        }

    }
}
