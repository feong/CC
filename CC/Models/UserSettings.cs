using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CC.Models
{
    class UserSettings
    {

        private static String IS_PAY_DAY_NOTIFY = "IS_PAY_DAY_NOTIFY";
        private static String IS_FREE_DAY_NOTIFY = "IS_FREE_DAY_NOTIFY";

        public static bool IsPayDayNotify
        {
            get
            {
                bool result = false;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(IS_PAY_DAY_NOTIFY))
                {
                    result = (bool)ApplicationData.Current.LocalSettings.Values[IS_PAY_DAY_NOTIFY];
                }
                return result;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[IS_PAY_DAY_NOTIFY] = value;
            }
        }

        public static bool IsFreeDayNotify
        {
            get
            {
                bool result = false;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(IS_FREE_DAY_NOTIFY))
                {
                    result = (bool)ApplicationData.Current.LocalSettings.Values[IS_FREE_DAY_NOTIFY];
                }
                return result;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[IS_FREE_DAY_NOTIFY] = value;
            }
        }


    }
}
