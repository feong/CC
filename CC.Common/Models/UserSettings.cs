using System;
using Windows.Storage;

namespace CC.Common.Models
{
    public class UserSettings
    {

        private static String IS_TOAST_ON = "IS_TOAST_ON";
        private static String IS_TILE_ON = "IS_TILE_ON";
        private static String TOAST_TIME = "TOAST_TIME";
        private static String TILE_DAY = "TILE_DAY";

        public static bool IsToastOn
        {
            get
            {
                bool result = false;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(IS_TOAST_ON))
                {
                    result = (bool)ApplicationData.Current.LocalSettings.Values[IS_TOAST_ON];
                }
                return result;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[IS_TOAST_ON] = value;
            }
        }

        public static bool IsTileOn
        {
            get
            {
                bool result = false;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(IS_TILE_ON))
                {
                    result = (bool)ApplicationData.Current.LocalSettings.Values[IS_TILE_ON];
                }
                return result;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[IS_TILE_ON] = value;
            }
        }

        // It is hour
        public static double ToastTime
        {
            get
            {
                double result = 10;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(TOAST_TIME))
                {
                    result = (double)ApplicationData.Current.LocalSettings.Values[TOAST_TIME];
                }
                return result;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[TOAST_TIME] = value;
            }
        }


        public static double TileDay
        {
            get
            {
                double result = 3;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(TILE_DAY))
                {
                    result = (double)ApplicationData.Current.LocalSettings.Values[TILE_DAY];
                }
                return result;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[TILE_DAY] = value;
            }
        }

    }
}
