using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Models
{
    class CreditCardManager
    {

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
            }
        }

        public void RemoveCard(CreditCard card)
        {
            this.cards.Remove(card);
        }
        
    }
}
