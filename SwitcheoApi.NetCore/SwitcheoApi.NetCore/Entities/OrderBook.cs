using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class OrderBook
    {
        public SwitcheoOffer[] bids { get; set; }
        public SwitcheoOffer[] asks { get; set; }
    }
}
