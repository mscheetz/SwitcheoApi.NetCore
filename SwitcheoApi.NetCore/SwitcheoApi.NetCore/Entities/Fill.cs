using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class Fill
    {
        public Guid id { get; set; }
        public string offer_hash { get; set; }
        public string offer_asset_id { get; set; }
        public string want_asset_id { get; set; }
        public decimal fill_amount { get; set; }
        public decimal want_amount { get; set; }
        public decimal filled_amount { get; set; }
        public string fee_asset_id { get; set; }
        public decimal fee_amount { get; set; }
        public decimal price { get; set; }
        public object[] txn { get; set; }
        public string status { get; set; }
        public DateTimeOffset created_at { get; set; }
        public string transaction_hash { get; set; }
    }
}
