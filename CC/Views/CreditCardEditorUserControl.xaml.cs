using CC.Common;
using CC.Models;
using System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CC.Views
{

    enum Status
    {
        Adding,
        Editing,
        Deleting,
        OK,
        Unkown
    }

    class CreditCardEditor : BindableBase
    {
        public CreditCard OrignalCard { get; set; }

        private Status status;
        public Status Status
        {
            get { return this.status; }
            set { this.SetProperty(ref this.status, value); }
        }

        private Bank bank;
        public Bank Bank
        {
            get { return this.bank; }
            set { this.SetProperty(ref this.bank, value); }
        }

        private String nickName;
        public String NickName
        {
            get { return nickName; }
            set { this.SetProperty(ref this.nickName, value); }
        }

        private String no;
        public String NO
        {
            get { return no; }
            set { this.SetProperty(ref this.no, value); }
        }

        private int orderDay;
        public int OrderDay
        {
            get { return orderDay; }
            set { this.SetProperty(ref this.orderDay, value); }
        }

        private int payDay;
        public int PayDay
        {
            get { return payDay; }
            set { this.SetProperty(ref this.payDay, value); }
        }

        public void setEditor(Status status, CreditCard card)
        {
            this.OrignalCard = card;
            this.Status = status;
            this.Bank = card.Bank;
            this.NickName = card.NickName;
            this.NO = card.NO;
            this.OrderDay = card.OrderDay;
            this.PayDay = card.PayDay;
        }
    }


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
                        cce.Status = Status.OK;
                    }
                }
                else if (symbol == Symbol.Edit)
                {
                    if (CreditCardManager.GetInstance().GetAllCards().Contains(card) && !card.Equals(cce.OrignalCard))
                    {
                        new MessageDialog("卡片已经存在！", "提示").ShowAsync();
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("确定要修改卡片信息吗？", "询问");
                        md.Commands.Add(new UICommand("确定", cmd =>
                        {
                            CreditCardManager.GetInstance().ReplaceCard(cce.OrignalCard, card);
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
                    CreditCardManager.GetInstance().RemoveCard(cce.OrignalCard);
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
