using System;

namespace CC.Models
{
    class CreditCard
    {
        public static CreditCard DefaultCard = new CreditCard(Bank.Nanchang,"", "1989", 10, 10);

        public Bank Bank { get; private set; }
        public string NickName { get; private set; }
        public string NO { get; private set; }
        public int UsedTimes { get; private set; }
        
        private int OrderDay { get; set; }
        private int PayDay { get; set; }

        public CreditCard(Bank bank, String nickName, String no, int orderDay, int payDay)
        {
            this.Bank = bank;
            this.NickName = nickName;
            this.NO = no;
            this.OrderDay = orderDay;
            this.PayDay = payDay;
        }
        
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
            return this.Bank == other.Bank && this.NickName == other.NickName && this.NO == other.NO && this.OrderDay == other.OrderDay && this.PayDay == other.PayDay;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
