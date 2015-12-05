using System;

namespace CC.Common.Models
{
    public class CreditCard
    {
        public static CreditCard DefaultCard = new CreditCard(Bank.Nanchang,"", "1989", 10, 10);

        public Bank Bank { get; private set; }
        public string NickName { get; private set; }
        public string NO { get; private set; }
        public int OrderDay { get; private set; }
        public int PayDay { get; private set; }
        public int UsedTimes { get; private set; }

        // Default
        public CreditCard(Bank bank, String nickName, String no, int orderDay, int payDay,int userdTiems)
        {
            this.Bank = bank;
            this.NickName = nickName;
            this.NO = no;
            this.OrderDay = orderDay;
            this.PayDay = payDay;
            this.UsedTimes = userdTiems;
        }

        public CreditCard(Bank bank, String nickName, String no, int orderDay, int payDay) : this(bank, nickName, no, orderDay, payDay, 0) { }
        
        public DateTime CurrentOrderDate()
        {
            return this.getCurrentOrderDate(this.OrderDay);
        }

        public DateTime CurrentPayDate()
        {
            return this.PayDateForOrder(this.CurrentOrderDate(), this.PayDay);
        }

        public int LeftPayDays()
        {
            return this.CurrentPayDate().Subtract(DateTime.Now).Days;          // Consider whether it should -1.
        }

        public int CurrentFreeDays()
        {
            var nextOrderDate = this.CurrentOrderDate().AddMonths(1);
            var payDateForNextOrder = this.PayDateForOrder(nextOrderDate, this.PayDay);
            return payDateForNextOrder.Subtract(DateTime.Now).Days;          // Consider whether it should -1.
        }

        public int CurrentTotalFreeDays()
        {
            var nextOrderDate = this.CurrentOrderDate().AddMonths(1);
            var payDateForNextOrder = this.PayDateForOrder(nextOrderDate, this.PayDay);
            return payDateForNextOrder.Subtract(this.CurrentOrderDate()).Days;          // Consider whether it should -1.
        }

        public int ToNextOrderDay()
        {
            var nextOrderDate = this.CurrentOrderDate().AddMonths(1);
            return nextOrderDate.Subtract(DateTime.Now).Days;          // Consider whether it should -1.
        }

        public int NextTotalFreeDays()
        {
            var nextOrderDate = this.CurrentOrderDate().AddMonths(1);
            var nextNextOrderDate = nextOrderDate.AddMonths(1);
            var payDateForNextNextOrder = this.PayDateForOrder(nextNextOrderDate, this.PayDay);
            return payDateForNextNextOrder.Subtract(nextOrderDate).Days;          // Consider whether it should -1.
        }

        public void AddUsedTime()
        {
            this.AddUsedTimes(1);
        }

        public void RemoveUsedTime()
        {
            this.RemoveUsedTimes(1);
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
        
        private void AddUsedTimes(int times)
        {
            this.UsedTimes += times;
        }

        private void RemoveUsedTimes(int times)
        {
            this.UsedTimes -= times;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CreditCard)) return false;
            if (base.Equals(obj)) return true;

            var other = obj as CreditCard;
            return this.Bank == other.Bank && this.NO == other.NO && this.OrderDay == other.OrderDay && this.PayDay == other.PayDay;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public override string ToString()
        {
            return String.Format("{0}_{1}_{2}{3}{4}{5}.|", (int)this.Bank, this.NickName, this.NO, this.OrderDay.ToString("D2"), this.PayDay.ToString("D2"), this.UsedTimes.ToString("D2"));
        }
    }
}
