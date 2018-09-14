using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class Fill
    {
        public string id { get; set; }
        public string offer_hash { get; set; }
        public string offer_asset_id { get; set; }
        public string want_asset_id { get; set; }
        public string fill_amount { get; set; }
        public string want_amount { get; set; }
        public string filled_amount { get; set; }
        public string fee_asset_id { get; set; }
        public string fee_amount { get; set; }
        public string price { get; set; }
        public Transaction txn { get; set; }
        public string status { get; set; }
        public DateTimeOffset created_at { get; set; }
        public string transaction_hash { get; set; }
    }
}
