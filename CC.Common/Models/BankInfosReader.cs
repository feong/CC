using System;
using System.Collections.Generic;

namespace CC.Common.Models
{
    public class BankInfosReader
    {
        public Dictionary<String, BankInfo> Dic { get; private set; }

        #region Singleton
        private static BankInfosReader instance;

        private static Object obj = new object();

        public static BankInfosReader GetInstance()
        {
            lock (obj)
            {
                if (instance == null)
                {
                    instance = new BankInfosReader();
                    //var file = File.OpenRead(@"C:\Users\feong\Documents\GitHubVisualStudio\CC\CC.Common\Models\BankInfos.xaml");
                    //instance.Dic = new ResourceDictionary { Source = new Uri("ms-appx:///CC.Common/Models/BankInfos.xaml") };

                    instance.init();
                    
                }
            }
            return instance;
        }

        private BankInfosReader() { }
        #endregion

        private void init()
        {
            var dic = new Dictionary<String, BankInfo>();
            dic.Add("Baoshang", new BankInfo() { Bank = Bank.Baoshang, Title = "包商银行", Color = "#FF0695A0" });
            dic.Add("Beijing", new BankInfo() { Bank = Bank.Beijing, Title = "北京银行", Color = "#FFDD271E" });

            dic.Add("Chengdunongshang", new BankInfo() { Bank = Bank.Chengdunongshang, Title = "成都农商银行", Color = "#FF04683D" });
            dic.Add("Chongqing", new BankInfo() { Bank = Bank.Chongqing, Title = "重庆银行", Color = "#FF06B56C" });
            dic.Add("Chongqingnongshang", new BankInfo() { Bank = Bank.Chongqingnongshang, Title = "重庆农村商业银行", Color = "#FFE40422" });

            dic.Add("Gongshang", new BankInfo() { Bank = Bank.Gongshang, Title = "工商银行", Color = "#FFC60506" });
            dic.Add("Guangda", new BankInfo() { Bank = Bank.Guangda, Title = "光大银行", Color = "#FF59056B" });
            dic.Add("Guangfa", new BankInfo() { Bank = Bank.Guangfa, Title = "广发银行", Color = "#FFE90424" });
            dic.Add("Guangzhou", new BankInfo() { Bank = Bank.Guangzhou, Title = "广州银行", Color = "#FFB1080C" });

            dic.Add("Haerbin", new BankInfo() { Bank = Bank.Haerbin, Title = "哈尔滨银行", Color = "#FF7E0F72" });
            dic.Add("Hangzhou", new BankInfo() { Bank = Bank.Hangzhou, Title = "杭州银行", Color = "#FF19A4E6" });
            dic.Add("Hankou", new BankInfo() { Bank = Bank.Hankou, Title = "汉口银行", Color = "#FF44D1F3" });
            dic.Add("Hebei", new BankInfo() { Bank = Bank.Hebei, Title = "河北银行", Color = "#FF096BA4" });
            dic.Add("Huaxia", new BankInfo() { Bank = Bank.Huaxia, Title = "华夏银行", Color = "#FFFF2C29" });
            dic.Add("Huishang", new BankInfo() { Bank = Bank.Huishang, Title = "徽商银行", Color = "#FFEC262B" });

            dic.Add("Jiangsu", new BankInfo() { Bank = Bank.Jiangsu, Title = "江苏银行", Color = "#FF2380CB" });
            dic.Add("Jianshe", new BankInfo() { Bank = Bank.Jianshe, Title = "建设银行", Color = "#FF1B52AA" });
            dic.Add("Jiaotong", new BankInfo() { Bank = Bank.Jiaotong, Title = "交通银行", Color = "#FF043273" });

            dic.Add("Minsheng", new BankInfo() { Bank = Bank.Minsheng, Title = "民生银行", Color = "#FF089A83" });

            dic.Add("Nanchang", new BankInfo() { Bank = Bank.Nanchang, Title = "南昌银行", Color = "#FF049491" });
            dic.Add("Nanjing", new BankInfo() { Bank = Bank.Nanjing, Title = "南京银行", Color = "#FFCD0404" });
            dic.Add("Ningbo", new BankInfo() { Bank = Bank.Ningbo, Title = "宁波银行", Color = "#FFF3BD0F" });
            dic.Add("Nongye", new BankInfo() { Bank = Bank.Nongye, Title = "农业银行", Color = "#FF059889" });

            dic.Add("Pingan", new BankInfo() { Bank = Bank.Pingan, Title = "平安银行", Color = "#FFEF5B22" });
            dic.Add("Pufa", new BankInfo() { Bank = Bank.Pufa, Title = "浦发银行", Color = "#FF043572" });

            dic.Add("Shanghai", new BankInfo() { Bank = Bank.Shanghai, Title = "上海银行", Color = "#FFEDBB1C" });
            dic.Add("Shanghainongshang", new BankInfo() { Bank = Bank.Shanghainongshang, Title = "上海农商银行", Color = "#FF04439D" });
            dic.Add("Shangrao", new BankInfo() { Bank = Bank.Shangrao, Title = "上饶银行", Color = "#FFF5AD04" });
            dic.Add("Shengjing", new BankInfo() { Bank = Bank.Shengjing, Title = "盛京银行", Color = "#FFE50411" });

            dic.Add("Tianjing", new BankInfo() { Bank = Bank.Tianjing, Title = "天津银行", Color = "#FF054BA4" });

            dic.Add("Wenzhou", new BankInfo() { Bank = Bank.Wenzhou, Title = "温州银行", Color = "#FFFE9A04" });
            dic.Add("Wulumuqishangye", new BankInfo() { Bank = Bank.Wulumuqishangye, Title = "乌鲁木齐商业银行", Color = "#FF0D439E" });

            dic.Add("Xingye", new BankInfo() { Bank = Bank.Xingye, Title = "兴业银行", Color = "#FF1F4686" });

            dic.Add("Yaodunongshang", new BankInfo() { Bank = Bank.Yaodunongshang, Title = "尧都农村商业银行", Color = "#FF34822C" });
            dic.Add("Youzheng", new BankInfo() { Bank = Bank.Youzheng, Title = "中国邮政储蓄银行", Color = "#FF056744" });

            dic.Add("Zhada", new BankInfo() { Bank = Bank.Zhada, Title = "渣打银行", Color = "#FF05993A" });
            dic.Add("Zhaoshang", new BankInfo() { Bank = Bank.Zhaoshang, Title = "招商银行", Color = "#FFD10404" });
            dic.Add("Zhongguo", new BankInfo() { Bank = Bank.Zhongguo, Title = "中国银行", Color = "#FFAE0505" });
            dic.Add("Zhongxin", new BankInfo() { Bank = Bank.Zhongxin, Title = "中信银行", Color = "#FFDF3324" });



            instance.Dic = dic;
        }
    }
}
