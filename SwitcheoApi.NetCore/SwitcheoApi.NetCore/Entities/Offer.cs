using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class Offer
    {
        public Guid id { get; set; }
        public string offer_asset { get; set; }
        public string want_asset { get; set; }
        public decimal available_amount { get; set; }
        public decimal offer_amount { get; set; }
        public decimal want_amount { get; set; }
    }
}
