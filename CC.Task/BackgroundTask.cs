using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using CC.Models;

namespace CC.Task
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTask.UpdatePrimaryTile();
            BackgroundTask.MakeAToast();
        }

        public static async Task<BackgroundTaskRegistration> RegisterBackgroundTask(Type taskEntryPoint,
                                                                string taskName,
                                                                IBackgroundTrigger trigger,
                                                                IBackgroundCondition condition)
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.Denied)
            {
                return null;
            }

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == taskName)
                {
                    cur.Value.Unregister(true);
                }
            }

            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = taskEntryPoint.FullName
            };

            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();
            return task;
        }



        public static void UpdatePrimaryTile()
        {
            string TileTemplateXml = @"
<tile> 
  <visual>
    <binding template='TileMedium' hint-textStacking='center'>
      <group>
        <subgroup hint-weight='33'>
          <image src='Assets/BankIcons/{0}' hint-crop='circle'/>
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
          <image src='Assets/BankIcons/{0}' hint-crop='circle'/>
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
          <image src='Assets/BankIcons/{0}' hint-crop='circle'/>
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

            try
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueueForWide310x150(true);
                updater.EnableNotificationQueueForSquare150x150(true);
                updater.EnableNotificationQueue(true);
                updater.Clear();

                if (!UserSettings.IsTileOn) return;

                foreach (var card in CreditCardManager.GetInstance().GetAllCards())
                {
                    var bank = card.Bank;
                    ResourceDictionary dic = new ResourceDictionary { Source = new Uri("ms-appx:///Models/BankInfos.xaml") };
                    var bankInfo = dic[bank.ToString()] as BankInfo;

                    var doc = new XmlDocument();

                    if (card.LeftPayDays() > UserSettings.TileDay) continue;
                    var payDay = card.LeftPayDays() == 0 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                    var xml = string.Format(TileTemplateXml, bankInfo.ImageName, bankInfo.Title, payDay, card.CurrentFreeDays());
                    doc.LoadXml(xml);
                    updater.Update(new TileNotification(doc));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void MakeAToast()
        {
            string ToastTemplateXml = @"
<toast>
  <visual>
    <binding template='ToastGeneric'>
      <image src='Assets/BankIcons/{0}' placement='appLogoOverride' hint-crop='circle'/>
      <text>{1}</text>
      <text>今天是您的还款日，请及时还款。</text>
    </binding>
  </visual>
</toast>";
            foreach (var card in CreditCardManager.GetInstance().GetAllCards())
            {
                var bank = card.Bank;
                ResourceDictionary dic = new ResourceDictionary { Source = new Uri("ms-appx:///Models/BankInfos.xaml") };
                var bankInfo = dic[bank.ToString()] as BankInfo;

                if (card.LeftPayDays() != 0) continue;
                if (DateTime.Now.Hour < UserSettings.ToastTime) continue;

                var xml = string.Format(ToastTemplateXml, bankInfo.ImageName, bankInfo.Title);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                ToastNotification notification = new ToastNotification(doc);
                ToastNotifier nt = ToastNotificationManager.CreateToastNotifier();
                nt.Show(notification);
            }
        }
    }
}
