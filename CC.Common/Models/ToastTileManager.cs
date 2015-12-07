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
    <binding template='TileMedium' hint-textStacking='center'>
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
    <binding template='TileWide'>
      <group>
        <subgroup hint-weight='10'>
          <image src='CC.Common/BankIcons/{0}.png' hint-crop='circle'/>
        </subgroup>
        <subgroup hint-textStacking='center'>
          <text hint-style='body'>{1}</text>
        </subgroup>
      </group>
      <text/>
      <text/>
      <text/>
      <text/>
      <text/>
      <group>
        <subgroup hint-textStacking='bottom'>
          <text hint-style='caption' hint-align='right'>还款日: {2}</text>
          <text hint-style='caption' hint-align='right'>今日免息期: {3}</text>
        </subgroup>
      </group>
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
                //updater.Clear();

                if (!UserSettings.IsTileOn) return;

                foreach (var card in CreditCardManager.GetInstance().GetAllCards())
                {
                    if (UserSettings.TileDay != UserSettings.All_The_Time && card.LeftPayDays() > UserSettings.TileDay) continue;

                    var bank = card.Bank;
                    var dic = BankInfosReader.GetInstance().Dic;
                    var bankInfo = dic[bank.ToString()] as BankInfo;

                    var doc = new XmlDocument();

                    //var delayOneMinuteTime = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute + 1, DateTimeOffset.Now.Second, DateTimeOffset.Now.Offset);
                    var delaThreeMinutesTime = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, DateTimeOffset.Now.Second + 3, DateTimeOffset.Now.Offset);
                    var delayToTomorrowTime = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day + 1, 0, 0, 3, DateTimeOffset.Now.Offset);

                    var payDay = (isBackgroundCall && card.LeftPayDays() == 1) || card.LeftPayDays() == 0 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                    var xml = string.Format(TileTemplateXml, bankInfo.Bank.ToString(), bankInfo.Title, payDay, card.CurrentFreeDays());
                    doc.LoadXml(xml);
                    
                    var noti = isBackgroundCall ? new ScheduledTileNotification(doc, delayToTomorrowTime) : new ScheduledTileNotification(doc, delaThreeMinutesTime);
                    noti.Tag = card.Bank.ToString() + card.NO;
                    updater.AddToSchedule(noti);
                }
            }
            catch (Exception)
            {
                // ignored
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
