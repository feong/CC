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
        public string Name { get; set; }
        public string Uri { get; set; }

        public BankInfo() { }

    }

}
