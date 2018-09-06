using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class OrderBook : Offer
    {
        public SwitcheoOrder[] bids { get; set; }
        public SwitcheoOrder[] asks { get; set; }
    }
}
