using CC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Models
{
    enum Status
    {
        Adding,
        Editing,
        Deleting,
        OK,
        Unkown
    }

    class CreditCardEditor:BindableBase
    {
        private Status status;
        public Status Status
        {
            get { return this.status; }
            set { this.SetProperty(ref this.status, value); }
        }

        private CreditCard card;
        public CreditCard Card
        {
            get { return this.card; }
            set { this.SetProperty(ref this.card, value); }
        }

    }
}
