using System;

using CC.Models;

namespace CC.Models
{

    class CreditCard
    {
        public Bank Bank { get; private set; }
        public string NO { get; private set; }
        public int UsedTimes { get; }

        private int OrderDay { get; set; }
        private int PayDay { get; set; }
        

        public CreditCard(Bank bank, String no, int orderDay, int payDay)
        {
            this.Bank = bank;
            this.NO = no;
            this.OrderDay = orderDay;
            this.PayDay = payDay;
        }

        private DateTime currentOrderDate(int orderDay)
        {
            DateTime now = DateTime.Now;
            return now.Day > orderDay ? now.AddDays(orderDay - now.Day) : now.AddDays(orderDay - now.Day).AddMonths(-1);
        }

        private DateTime payDateForOrder(DateTime orderDate, int payDay)
        {
            return payDay > orderDate.Day ? orderDate.AddDays(payDay - orderDate.Day) : orderDate.AddDays(payDay - orderDate.Day).AddMonths(1);
        }


        public DateTime currentOrderDate()
        {
            return this.currentOrderDate(this.OrderDay);
        }

        public DateTime currentPayDate()
        {
            return this.payDateForOrder(this.currentOrderDate(), this.PayDay);
        }

        public int currentFreeDays()
        {
            var nextOrderDate = this.currentOrderDate().AddMonths(1);
            var payDateForNextOrder = this.payDateForOrder(nextOrderDate, this.PayDay);
            return payDateForNextOrder.Subtract(DateTime.Now).Days;          // Consider whether it should -1.
        }

        public int TotalFreeDays()
        {
            var nextOrderDate = this.currentOrderDate().AddMonths(1);
            var payDateForNextOrder = this.payDateForOrder(nextOrderDate, this.PayDay);
            return payDateForNextOrder.Subtract(this.currentOrderDate()).Days;          // Consider whether it should -1.
        }
    }
}
