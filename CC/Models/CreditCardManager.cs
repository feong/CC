using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace CC.Models
{
    class CreditCardManager
    {

        private static String LOCAL_SETTINGS = "LOCAL_SETTINGS";

        #region Singleton
        private static CreditCardManager instance;

        private static Object obj = new object();

        public static CreditCardManager GetInstance()
        {
            lock (obj)
            {
                if (instance == null)
                {
                    instance = new CreditCardManager();
                    instance.cards = new ObservableCollection<CreditCard>();
                    instance.LoadCards();
                    instance.cards.CollectionChanged += CardsCollectionChanged;
                    CreditCardManager.UpdatePrimaryTile();
                }
            }
            return instance;
        }

        private static void CardsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            instance.SaveCards();
            CreditCardManager.UpdatePrimaryTile();
        }


        private static void UpdatePrimaryTile()
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
  </visual>
</tile>";

            try
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueueForWide310x150(true);
                updater.EnableNotificationQueueForSquare150x150(true);
                updater.EnableNotificationQueue(true);
                updater.Clear();

                foreach (var card in instance.GetAllCards())
                {


                    var bank = card.Bank;
                    ResourceDictionary dic = new ResourceDictionary { Source = new Uri("ms-appx:///Models/BankInfos.xaml") };
                    var bankInfo = dic[bank.ToString()] as BankInfo;

                    var doc = new XmlDocument();
                    var payDay = card.LeftPayDays() == 0 ? "今天" : card.CurrentPayDate().ToString("MM/dd");
                    var xml = string.Format(TileTemplateXml, bankInfo.ImageName, bankInfo.Title, payDay, card.CurrentFreeDays());
                    doc.LoadXml(WebUtility.HtmlDecode(xml), new XmlLoadSettings
                    {
                        ProhibitDtd = false,
                        ValidateOnParse = false,
                        ElementContentWhiteSpace = false,
                        ResolveExternals = false
                    });

                    updater.Update(new TileNotification(doc));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

    private CreditCardManager() { }
        #endregion

        private ObservableCollection<CreditCard> cards;

        public ObservableCollection<CreditCard> GetAllCards()
        {
            return this.cards;
        }

        public void AddCard(CreditCard card)
        {
            if (!this.cards.Contains(card))
            {
                this.cards.Add(card);
            }
        }

        public void ReplaceCard(CreditCard oldCard, CreditCard newCard)
        {
            if (this.cards.Contains(oldCard))
            {
                var index = this.cards.IndexOf(oldCard);
                this.cards.Remove(oldCard);
                this.cards.Insert(index, newCard);
            }
        }
        
        public void RemoveCard(CreditCard card)
        {
            this.cards.Remove(card);
        }

        public override string ToString()
        {
            String output = "";
            foreach (var card in cards)
            {
                output += card.ToString();
            }
            return output;
        }
       
        private void SaveCards()
        {
            if (cards == null) return;

            String configs = "";
            foreach (var card in cards)
            {
                configs += card.ToString();
            }
            ApplicationData.Current.LocalSettings.Values[LOCAL_SETTINGS] = configs;
        }

        public bool LoadCards(String cards)
        {
            if (cards == null || cards == "") return false;

            var result = true;
            this.cards.Clear();

            // 36_Young_8888200500.|16__1234052000.|17_中中中中中中中中中中_2333250800.|38_iBai_6666100300.|
            var cardStrings = cards.Replace(".|", "|").Split('|');
            for (int i = 0; i < cardStrings.Length - 1; i++)
            {
                var str = cardStrings[i];
                if (str.Length < 13)
                {
                    result = false;
                    continue;
                }
                if (Regex.Matches(str, "_").Count < 2)
                {
                    result = false;
                    continue;
                }

                var index1 = str.IndexOf("_");
                var index2 = str.LastIndexOf("_");
                var strBank = str.Substring(0, index1);
                var strNickName = str.Substring(index1 + 1, index2 - index1 - 1);
                var strNO = str.Substring(index2 + 1, 4);
                var strOrderDay = str.Substring(index2 + 5, 2);
                var strPayDay = str.Substring(index2 + 7, 2);

                int bank;
                if (!Int32.TryParse(strBank, out bank) || bank < 0 || bank > (int)Bank.Zhongxin)
                {
                    result = false;
                    continue;
                }
                int no;
                if (!Int32.TryParse(strNO, out no) || no < 0)
                {
                    result = false;
                    continue;
                }
                int orderDay;
                if (!Int32.TryParse(strOrderDay, out orderDay) || orderDay < 0 || orderDay > 31)
                {
                    result = false;
                    continue;
                }
                int payDay;
                if (!Int32.TryParse(strPayDay, out payDay) || payDay < 0 || payDay > 31)
                {
                    result = false;
                    continue;
                }
                this.AddCard(new CreditCard((Bank)bank, strNickName, strNO, orderDay, payDay));
            }
            return result;
        }

        private bool LoadCards()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(LOCAL_SETTINGS))
            {
                return this.LoadCards((String)ApplicationData.Current.LocalSettings.Values[LOCAL_SETTINGS]);
            }
            return false;
        }
    }
}
