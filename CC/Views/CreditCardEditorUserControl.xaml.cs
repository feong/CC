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

        private void AcceptTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.tbNO.Text.Length == 4)
            {
                var card = new CreditCard((Bank)this.cbBank.SelectedItem, this.tbNickname.Text, this.tbNO.Text, (int)this.cbOrderDay.SelectedItem, (int)this.cbPayDay.SelectedItem);
                CreditCardManager.GetInstance().AddCard(card);
                var ccm = this.DataContext as CreditCardEditor;
                ccm.Card = card;
                ccm.Status = Status.OK;
            }
        }

        private void CancelTapped(object sender, TappedRoutedEventArgs e)
        {
            var ccm = this.DataContext as CreditCardEditor;
            ccm.Status = Status.OK;
        }
    }
}
