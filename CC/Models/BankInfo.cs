using System;

namespace CC.Models
{
    enum Bank
    {
        Zhaoshang,
        Zhongxin
    }

    class BankInfo
    {
        public Bank Bank { get; set; }
        public String Name { get; set; }
        public Uri Uri { get; set; }

        public BankInfo() { }

    }

}
