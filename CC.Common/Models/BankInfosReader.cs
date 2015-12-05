using System;
using Windows.UI.Xaml;

namespace CC.Common.Models
{
    public class BankInfosReader
    {
        public ResourceDictionary Dic { get; private set; }

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
                    instance.Dic = new ResourceDictionary { Source = new Uri("ms-appdata:///CC.Common/Models/BankInfos.xaml", UriKind.Absolute) };
                }
            }
            return instance;
        }

        private BankInfosReader() { }
        #endregion


    }
}
