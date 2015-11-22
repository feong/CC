﻿using System;
using System.Collections;
using System.Collections.Generic;
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
                }
            }
            return instance;
        }

        private CreditCardManager() { }
        #endregion

        private List<CreditCard> cards;

        public CreditCard[] GetAllCards()
        {
            return this.cards.ToArray();
        }

        public void AddCard(CreditCard card)
        {
            this.cards.Add(card);
        }

        public void RemoveCard(CreditCard card)
        {
            this.cards.Remove(card);
        }
    }
}