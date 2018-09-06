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
                return this.want_amount / this.offer_amount;
            }
        }
    }
}
