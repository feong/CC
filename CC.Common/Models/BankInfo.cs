using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace CC.Common.Models
{
    public enum Bank
    {
        Baoshang, Beijing,
        Chengdunongshang, Chongqing, Chongqingnongshang,
        Gongshang, Guangda, Guangfa, Guangzhou,
        Haerbin, Hangzhou, Hankou, Hebei, Huaxia, Huishang,
        Jiangsu, Jianshe, Jiaotong,
        Minsheng,
        Nanchang, Nanjing, Ningbo, Nongye,
        Pingan, Pufa,
        Shanghai, Shanghainongshang, Shangrao, Shengjing,
        Tianjing,
        Wenzhou, Wulumuqishangye,
        Xingye,
        Yaodunongshang, Youzheng,
        Zhada, Zhaoshang, Zhongguo, Zhongxin
    }

    public class BankInfo
    {
        private static Uri baseUri = new Uri(@"ms-appx:///CC.Common/BankIcons/");
        public static BankInfo DefaultBankInfo = new BankInfo
        {
            Bank = Bank.Nanchang,
            Title = "南昌银行",
            Color = "#FF049491"
        };

        public Bank Bank { get; set; }
        public String Title { get; set; }
        public String Color { get; set; }

        public Uri Uri
        {
            get { return new Uri(baseUri, this.Bank.ToString() + ".png"); }
        }
    }

}
