using System;

using CC.Models;

namespace CC.Models
{
    class DesignCreditCard
    {
        public Bank Bank { get; set; }
        public String NO { get; set; }
        public int OrderDay { get; set; }
        public int PayDay { get; set; }
        public int UsedTimes { get; set; }

        public DateTime CurrentOrderDate2
        {
            get
            {
                return this.CurrentOrderDate(this.OrderDay);
            }
        }

        public DateTime CurrentOrderDate()
        {
            return this.CurrentOrderDate(this.OrderDay);
        }

        public DateTime CurrentPayDate()
        {
            return this.PayDateForOrder(this.CurrentOrderDate(), this.PayDay);
        }
        public int CurrentFreeDays
        {
            get
            {
                var nextOrderDate = this.CurrentOrderDate().AddMonths(1);
                var payDateForNextOrder = this.PayDateForOrder(nextOrderDate, this.PayDay);
                return payDateForNextOrder.Subtract(DateTime.Now).Days;          // Consider whether it should -1.
            }
        }

        public int TotalFreeDays
        {
            get
            {
                var nextOrderDate = this.CurrentOrderDate().AddMonths(1);
                var payDateForNextOrder = this.PayDateForOrder(nextOrderDate, this.PayDay);
                return payDateForNextOrder.Subtract(this.CurrentOrderDate()).Days;          // Consider whether it should -1.
            }
        }
        private DateTime CurrentOrderDate(int orderDay)
        {
            DateTime now = DateTime.Now;
            return now.Day > orderDay ? now.AddDays(orderDay - now.Day) : now.AddDays(orderDay - now.Day).AddMonths(-1);
        }
        private DateTime PayDateForOrder(DateTime orderDate, int payDay)
        {
            return payDay > orderDate.Day ? orderDate.AddDays(payDay - orderDate.Day) : orderDate.AddDays(payDay - orderDate.Day).AddMonths(1);
        }
    }

    class CreditCard
    {
        public Bank Bank { get; private set; }
        public string NO { get; set; }
        public int UsedTimes { get; private set; }

        public DateTime CurrentOrderDate
        {
            get
            {
                return this.getCurrentOrderDate(this.OrderDay);
            }
        }

        public DateTime CurrentPayDate
        {
            get
            {
                return this.PayDateForOrder(this.CurrentOrderDate, this.PayDay);
            }
        }

        public int CurrentFreeDays
        {
            get
            {
                var nextOrderDate = this.CurrentOrderDate.AddMonths(1);
                var payDateForNextOrder = this.PayDateForOrder(nextOrderDate, this.PayDay);
                return payDateForNextOrder.Subtract(DateTime.Now).Days;          // Consider whether it should -1.
            }
        }

        public int TotalFreeDays
        {
            get
            {
                var nextOrderDate = this.CurrentOrderDate.AddMonths(1);
                var payDateForNextOrder = this.PayDateForOrder(nextOrderDate, this.PayDay);
                return payDateForNextOrder.Subtract(this.CurrentOrderDate).Days;          // Consider whether it should -1.
            }
        }

        private int OrderDay { get; set; }
        private int PayDay { get; set; }

        public CreditCard(Bank bank, String no, int orderDay, int payDay)
        {
            this.Bank = bank;
            this.NO = no;
            this.OrderDay = orderDay;
            this.PayDay = payDay;
        }


        private DateTime getCurrentOrderDate(int orderDay)
        {
            DateTime now = DateTime.Now;
            return now.Day > orderDay ? now.AddDays(orderDay - now.Day) : now.AddDays(orderDay - now.Day).AddMonths(-1);
        }

        private DateTime PayDateForOrder(DateTime orderDate, int payDay)
        {
            return payDay > orderDate.Day ? orderDate.AddDays(payDay - orderDate.Day) : orderDate.AddDays(payDay - orderDate.Day).AddMonths(1);
        }

        public void AddUsedTime()
        {
            this.AddUsedTimes(1);
        }

        public void RemoveUsedTime()
        {
            this.RemoveUsedTimes(1);
        }

        private void AddUsedTimes(int times)
        {
            this.UsedTimes += times;
        }

        private void RemoveUsedTimes(int times)
        {
            this.UsedTimes -= times;
        }
    }
}
