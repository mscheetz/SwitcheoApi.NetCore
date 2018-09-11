using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class SwitcheoOrder : Offer
    {
        public decimal price
        {
            get
            {
                return this.want_amount < this.offer_amount 
                    ? Math.Round(this.want_amount / this.offer_amount, 8)
                    : Math.Round(this.offer_amount / this.want_amount, 8);
            }
        }
    }
}
