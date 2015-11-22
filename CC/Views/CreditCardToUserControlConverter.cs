using CC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CC.Views
{
    public class BankToBankIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // TODO: value is an enum of Bank, need convert Bank to BankInfo at first

            var uri = BankInfo.DefaultUri;

            var bankInfo = value as BankInfo;
            if (bankInfo != null && bankInfo.Uri.IsFile)
            {
                uri = bankInfo.Uri;
            }
            return new BitmapImage(uri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BankToBankNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // TODO: value is an enum of Bank, need convert Bank to BankInfo at first

            var name = BankInfo.DefaultName;

            var bankInfo = value as BankInfo;
            if (bankInfo != null && bankInfo.Name != null)
            {
                name = bankInfo.Name;
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DateToFormatDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            String dateFormat = null;
            if (value != null)
            {
                var date = (DateTime)value;
                dateFormat = date.ToString("yyyy/MM/dd");
            }
            return dateFormat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
