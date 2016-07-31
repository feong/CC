using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace CC.Common.Models
{
    public class ToastTileManager
    {
        #region Define Tile & Toast

        private static readonly String TileTemplateXml = @"
<tile> 
  <visual>
    <binding template='TileMedium' hint-textStacking='center' hint-overlay='80'>
      <image src='CC.Common/BankIcons/{0}.png' placement='background'/>
      <text hint-style='caption'>还款日</text>
      <text hint-style='titleNumeral'>{2}</text>
      <text hint-style='captionSubtle'>免息期: {3}</text>
    </binding>
    <binding template='TileWide'>
      <group>
        <subgroup hint-weight='10'>
          <image src='CC.Common/BankIcons/{0}.png' hint-crop='circle'/>
        </subgroup>
        <subgroup hint-textStacking='center'>
          <text hint-style='body'>{1}</text>
        </subgroup>
      </group>
      <text hint-style='caption'/>
      <text hint-style='caption' hint-align='right'>还款日: {2}</text>
      <text hint-style='caption' hint-align='right'>今日免息期: {3}</text>
    </binding>
    <binding template='TileLarge' hint-textStacking='center'>
      <group>
        <subgroup hint-weight='33'>
          <image src='CC.Common/BankIcons/{0}.png' hint-crop='circle'/>
        </subgroup>
        <subgroup hint-textStacking='center'>
          <text hint-style='body'>{1}</text>
          <text hint-style='caption'>还款日: {2}</text>
          <text hint-style='caption'>今日免息期: {3}</text>
        </subgroup>
      </group>
    </binding>
  </visual>
</tile>";

        private static readonly String ToastTemplateXml = @"
<toast>
  <visual>
    <binding template='ToastGeneric'>
      <image src='CC.Common/BankIcons/{0}.png' placement='appLogoOverride' hint-crop='circle'/>
      <text>{1}</text>
      <text>今天是您({2})卡片的还款日，请及时还款。</text>
    </binding>
  </visual>
</toast>";

        #endregion


        public static void UpdatePrimaryTile()
        {
            ToastTileManager.UpdatePrimaryTile(false);
            ToastTileManager.UpdatePrimaryTile(true);
        }

        public static void CancelUpdatePrimaryTile()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();
            foreach (var tile in updater.GetScheduledTileNotifications())
            {
                updater.RemoveFromSchedule(tile);
            }
        }

        public static void UpdatePrimaryTile(bool isBackgroundCall)
        {

            try
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueueForWide310x150(true);
                updater.EnableNotificationQueueForSquare150x150(true);
                updater.EnableNotificationQueueForSquare310x310(true);
                updater.EnableNotificationQueue(true);
                if (!isBackgroundCall)
                {
                    ToastTileManager.CancelUpdatePrimaryTile();
                }

                if (!UserSettings.IsTileOn) return;

                foreach (var card in CreditCardManager.GetInstance().GetAllCards())
                {
                    if (UserSettings.TileDay != UserSettings.All_The_Time && (card.LeftPayDays() < 0 || card.LeftPayDays() > UserSettings.TileDay)) continue;

                    var bank = card.Bank;
                    var dic = BankInfosReader.GetInstance().Dic;
                    var bankInfo = dic[bank.ToString()] as BankInfo;

                    var doc = new XmlDocument();

                    var delayThreeSeconds = DateTimeOffset.Now.AddSeconds(3);
                    var delayThreeSecondsTime = new DateTimeOffset(delayThreeSeconds.Year, delayThreeSeconds.Month, delayThreeSeconds.Day, delayThreeSeconds.Hour, delayThreeSeconds.Minute, delayThreeSeconds.Second, delayThreeSeconds.Offset);
                    var tomorrow = DateTimeOffset.Now.AddDays(1);
                    var delayToTomorrowTime = new DateTimeOffset(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 3, tomorrow.Offset);

                    var payDay = "";
                    if (isBackgroundCall)
                    {
                        payDay = card.LeftPayDays() == 1 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                    }
                    else
                    {
                        if (delayThreeSecondsTime.Day == DateTimeOffset.Now.Day)
                        {
                            payDay = card.LeftPayDays() == 0 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                        }
                        else
                        {
                            payDay = card.LeftPayDays() == 1 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                        }
                    }
                    var xml = string.Format(TileTemplateXml, bankInfo.Bank.ToString(), bankInfo.Title, payDay, card.CurrentFreeDays());
                    doc.LoadXml(xml);

                    var noti = isBackgroundCall ? new ScheduledTileNotification(doc, delayToTomorrowTime) : new ScheduledTileNotification(doc, delayThreeSecondsTime);
                    noti.ExpirationTime = isBackgroundCall ? delayToTomorrowTime.AddDays(1).AddMilliseconds(-3) : delayToTomorrowTime.AddMilliseconds(-3);
                    updater.AddToSchedule(noti);


                    //var bank = card.Bank;
                    //var dic = BankInfosReader.GetInstance().Dic;
                    //var bankInfo = dic[bank.ToString()] as BankInfo;

                    //var doc = new XmlDocument();
                    //var payDay = "";
                    //var scheduleTime = new DateTimeOffset();

                    //if (UserSettings.TileDay != UserSettings.All_The_Time && (card.LeftPayDays() < 0 || card.LeftPayDays() > UserSettings.TileDay))
                    //{
                    //    scheduleTime = DateTimeOffset.Now.AddDays(30);
                    //    payDay = card.CurrentPayDate().ToString("MM/dd");
                    //}
                    //else
                    //{
                    //    if (isBackgroundCall)
                    //    {
                    //        var tomorrow = DateTimeOffset.Now.AddDays(1);
                    //        scheduleTime = new DateTimeOffset(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 3, tomorrow.Offset);
                    //        payDay = card.LeftPayDays() == 1 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                    //    }
                    //    else
                    //    {
                    //        scheduleTime = DateTimeOffset.Now.AddSeconds(3);
                    //        if (scheduleTime.Day == DateTimeOffset.Now.Day)
                    //        {
                    //            payDay = card.LeftPayDays() == 0 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                    //        }
                    //        else
                    //        {
                    //            payDay = card.LeftPayDays() == 1 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                    //        }
                    //    }
                    //}

                    //var xml = string.Format(TileTemplateXml, bankInfo.Bank.ToString(), bankInfo.Title, payDay, card.CurrentFreeDays());
                    //doc.LoadXml(xml);

                    //var noti = new ScheduledTileNotification(doc, scheduleTime);
                    //noti.Tag = card.Bank.ToString() + card.NO;
                    //if (!isBackgroundCall)
                    //{
                    //    noti.Tag = "T" + noti.Tag;
                    //    var temp = DateTimeOffset.Now.AddDays(1);
                    //    var tomorrow = new DateTimeOffset(temp.Year, temp.Month, temp.Day, 0, 0, 3, temp.Offset);
                    //    noti.ExpirationTime = tomorrow;
                    //}
                    //updater.AddToSchedule(noti);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void CancelMakeAToast()
        {
            ToastNotifier nt = ToastNotificationManager.CreateToastNotifier();
            foreach (var noti in nt.GetScheduledToastNotifications())
            {
                nt.RemoveFromSchedule(noti);
            }
        }

        public static void MakeAToast()
        {

            if (!UserSettings.IsToastOn) return;

            foreach (var card in CreditCardManager.GetInstance().GetAllCards())
            {

                var payDay = card.CurrentPayDate();
                var notiTime = new DateTime(payDay.Year, payDay.Month, payDay.Day, (int)UserSettings.ToastTime, 0, 0);

                if (DateTime.Now < notiTime)
                {
                    var bank = card.Bank;
                    var dic = BankInfosReader.GetInstance().Dic;
                    var bankInfo = dic[bank.ToString()] as BankInfo;
                    var specifyCard = card.NickName == "" ? "尾号" + card.NO : "别名" + card.NickName;
                    var xml = string.Format(ToastTemplateXml, bankInfo.Bank.ToString(), bankInfo.Title, specifyCard);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    //var time = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute + 1, 0, DateTimeOffset.Now.Offset);
                    var time = new DateTimeOffset(notiTime);
                    var noti = new ScheduledToastNotification(doc, time);
                    noti.Tag = card.Bank.ToString() + card.NO;
                    ToastNotifier nt = ToastNotificationManager.CreateToastNotifier();
                    nt.AddToSchedule(noti);
                }
            }
        }
    }
}
