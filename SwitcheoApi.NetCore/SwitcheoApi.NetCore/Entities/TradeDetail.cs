using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class TradeDetail
    {
        public Guid id { get; set; }
        public decimal fill_amount { get; set; }
        public decimal take_amount { get; set; }
        public DateTimeOffset event_time { get; set; }
        public bool is_buy { get; set; }
    }
}
