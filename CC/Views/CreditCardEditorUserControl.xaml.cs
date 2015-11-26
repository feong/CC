using CC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CC.Views
{
    public sealed partial class CreditCardEditorUserControl : UserControl
    {
        public CreditCardEditorUserControl()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var text = sender.Text;
            if (text == null || text == "") return;

            foreach (var c in text)
            {
                if (c < '0' || c > '9')
                {
                    sender.Text = text.Remove(text.IndexOf(c), 1);
                    sender.SelectionStart = sender.Text.Trim().Length;
                }
            }
            if (text.Length == 4)
            {
                sender.BorderBrush = Application.Current.Resources["ComboBoxDisabledForegroundThemeBrush"] as Brush;
            }
            else
            {
                sender.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void LeftTapped(object sender, TappedRoutedEventArgs e)
        {
            var cce = this.DataContext as CreditCardEditor;
            if (this.tbNO.Text.Length == 4)
            {
                var symbol = (sender as SymbolIcon).Symbol;
                var card = new CreditCard((Bank)this.cbBank.SelectedItem, this.tbNickname.Text, this.tbNO.Text, (int)this.cbOrderDay.SelectedItem, (int)this.cbPayDay.SelectedItem);
                if (symbol == Symbol.Accept)
                {
                    if (CreditCardManager.GetInstance().GetAllCards().Contains(card))
                    {
                        new MessageDialog("卡片已经存在！", "提示").ShowAsync();
                    }
                    else
                    {
                        CreditCardManager.GetInstance().AddCard(card);
                        //cce.Card = card;
                        cce.Status = Status.OK;
                    }
                }
                else if (symbol == Symbol.Edit)
                {
                    if (CreditCardManager.GetInstance().GetAllCards().Contains(card))
                    {
                        new MessageDialog("卡片已经存在！", "提示").ShowAsync();
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("确定要修改卡片信息吗？", "询问");
                        md.Commands.Add(new UICommand("确定", cmd =>
                        {
                            CreditCardManager.GetInstance().ReplaceCard(cce.Card, card);
                            cce.Status = Status.OK;
                        }, 0));
                        md.Commands.Add(new UICommand("放弃", cmd =>
                        {
                        }, 1));
                        md.DefaultCommandIndex = 0;
                        md.CancelCommandIndex = 1;
                        md.ShowAsync();
                    }
                }
            }
        }

        new private void RightTapped(object sender, TappedRoutedEventArgs e)
        {
            var cce = this.DataContext as CreditCardEditor;
            
            var symbol = (sender as SymbolIcon).Symbol;
            if (symbol == Symbol.Cancel)
            {
                cce.Status = Status.OK;
            }
            else if (symbol == Symbol.Delete)
            {
                MessageDialog md = new MessageDialog("确定要删除卡片吗？", "询问");
                md.Commands.Add(new UICommand("确定", cmd =>
                {
                    CreditCardManager.GetInstance().RemoveCard(cce.Card);
                    cce.Status = Status.OK;
                }, 0));
                md.Commands.Add(new UICommand("放弃", cmd =>
                {
                }, 1));
                md.DefaultCommandIndex = 0;
                md.CancelCommandIndex = 1;
                md.ShowAsync();
            }
        }
    }
}
