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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CC.Views
{
    public sealed partial class AddCreditCardUserControl : UserControl
    {
        public AddCreditCardUserControl()
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
        }
        
    }
}
