using System;

namespace CC.Models
{
    enum Bank
    {
        Zhada,
        Zhaoshang,
        Zhongxin
    }

    class BankInfo
    {
        public static Uri DefaultUri = new Uri(@"ms-appx:///Assets/BankIcons/Zada.png");
        public static String DefaultName = @"渣打银行";

        public Bank Bank { get; set; }
        public String Name { get; set; }
        public Uri Uri { get; set; }

        public BankInfo() { }

        public BankInfo(Bank bank, String name, String imagePath)
        {
            this.Bank = bank;
            this.Name = name;
            this.Uri = new Uri(@"ms-appx:///Assets/BankIcons/" + imagePath);
        }

    }

}
