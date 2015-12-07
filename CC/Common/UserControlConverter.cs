using CC.Common.Models;
using CC.Views;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CC.Common
{
    public class BankToInt32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)value;
        }
    }

    public class BankToBankIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bank = (Bank)value;
            var dic = BankInfosReader.GetInstance().Dic;
            var bankInfo = dic[bank.ToString()] as BankInfo;
            return new BitmapImage(bankInfo.Uri);
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
            var bank = (Bank)value;
            var dic = BankInfosReader.GetInstance().Dic;
            var bankInfo = dic[bank.ToString()] as BankInfo;
            return bankInfo.Title;
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
            var dic = BankInfosReader.GetInstance().Dic;
            var bankInfo = dic[bank.ToString()] as BankInfo;
            return this.ToBrush(bankInfo.Color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        
        private Brush ToBrush(String color)
        {
            return new SolidColorBrush(this.ToColor(color));
        }

        private Color ToColor(string color)
        {
            if (color.StartsWith("#"))
                color = color.Replace("#", string.Empty);
            int v = int.Parse(color, System.Globalization.NumberStyles.HexNumber);
            return new Color()
            {
                A = System.Convert.ToByte((v >> 24) & 255),
                R = System.Convert.ToByte((v >> 16) & 255),
                G = System.Convert.ToByte((v >> 8) & 255),
                B = System.Convert.ToByte((v >> 0) & 255)
            };
        }
    }

    public class DateToDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int day = 0;
            if (value != null)
            {
                var card = (CreditCard)value;
                String p = parameter as String;
                if (p == "OrderDay")
                {
                    day = card.CurrentOrderDate().Day;
                }
                else if (p == "PayDay")
                {
                    day = card.CurrentPayDate().Day;
                }
            }
            return day;
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
                var card = (CreditCard)value;
                String p = parameter as String;
                if (p == "OrderDay")
                {
                    dateFormat = card.CurrentOrderDate().ToString("MM/dd");
                }
                else if(p == "PayDay")
                {
                    dateFormat = card.CurrentPayDate().ToString("MM/dd");
                    String str = null;
                    int days = card.LeftPayDays();
                    if (days > 0)
                    {
                        str = String.Format(" (还剩{0}天)", days);
                    }
                    else if (days == 0)
                    {
                        str = @" (今天！)";
                    }
                    else
                    {
                        str = String.Format(" (过去{0}天)", Math.Abs(days));
                    }
                    dateFormat += str;
                }
            }
            return dateFormat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class CardToDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            String dateFormat = "";
            if (value != null)
            {
                var card = (CreditCard)value;
                String p = parameter as String;
                if (p == "Current")
                {
                    dateFormat = String.Format("{0}天 (共{1}天)", card.CurrentFreeDays(), card.CurrentTotalFreeDays());
                }
                else if (p == "Next")
                {
                    dateFormat = String.Format("{0}天 (共{1}天)", card.ToNextOrderDay(), card.NextTotalFreeDays());
                }
            }
            return dateFormat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityToBoolConverter : IValueConverter
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
    
    public class StatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Status status = (Status)value;
            return status == Status.OK || status == Status.Unkown ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Status.Unkown : Status.OK;
        }
    }

    public class StatusToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Status status = (Status)value;
            Symbol symbol = Symbol.Back;
            if (status == Status.Adding)
            {
                symbol = (parameter as String) == "Left" ? Symbol.Accept : Symbol.Cancel;
            }
            else if (status == Status.Editing)
            {
                symbol = (parameter as String) == "Left" ? Symbol.Edit : Symbol.Delete;
            }
            return symbol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class RefreshDayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var day = (double)value;
            var str = "";
            if (day == 0)
            {
                str = "还款日当天更新磁贴";
            }
            else if (day < 15)
            {
                str = String.Format("还款日前{0}天开始更新磁贴", day);
            }
            else
            {
                str = "每天更新磁贴";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ToastTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return String.Format("在{0}点推送消息", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
