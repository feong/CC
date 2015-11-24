using CC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CC.Views
{
    public class BankToBankIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var uri = BankInfo.DefaultUri;

            var bank = (Bank)value;
            ResourceDictionary dic = new ResourceDictionary { Source = new Uri("ms-appx:///Models/BankInfos.xaml") };
            var bankInfo = dic[bank.ToString()] as BankInfo;
            uri = bankInfo.Uri;
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
            var name = BankInfo.DefaultTitle;
            
            var bank = (Bank)value;
            ResourceDictionary dic = new ResourceDictionary { Source = new Uri("ms-appx:///Models/BankInfos.xaml") };
            var bankInfo = dic[bank.ToString()] as BankInfo;
            name = bankInfo.Title;
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    
    public class BankToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bank = (Bank)value;
            ResourceDictionary dic = new ResourceDictionary { Source = new Uri("ms-appx:///Models/BankInfos.xaml") };
            var bankInfo = dic[bank.ToString()] as BankInfo;
            return bankInfo.Color;
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
                dateFormat = date.ToString("MM/dd");
            }
            return dateFormat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class LeftDaysToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            String str = null;
            int days = (int)value;
            if (days > 0)
            {
                str = @" (还剩" + days + "天)";
            } else if(days == 0)
            {
                str = @" (今天！)";
            } else
            {
                str = @" (过去" + Math.Abs(days) + "天)";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
