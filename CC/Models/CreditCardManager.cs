using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Windows.Storage;

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
                }
            }
            return instance;
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
                this.SaveCards();
            }
        }

        public void ReplaceCard(CreditCard oldCard, CreditCard newCard)
        {
            if (this.cards.Contains(oldCard))
            {
                var index = this.cards.IndexOf(oldCard);
                this.cards.Remove(oldCard);
                this.cards.Insert(index, newCard);
                this.SaveCards();
            }
        }
        
        public void RemoveCard(CreditCard card)
        {
            this.cards.Remove(card);
            this.SaveCards();
        }

        public override string ToString()
        {
            String output = "";
            foreach (var card in cards)
            {
                output += card.ToString();
            }
            this.LoadCards(output);
            return output;
        }
        
        private bool LoadCards()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(LOCAL_SETTINGS))
            {
                return this.LoadCards((String)ApplicationData.Current.LocalSettings.Values[LOCAL_SETTINGS]);
            }
            return false;
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
    }
}
