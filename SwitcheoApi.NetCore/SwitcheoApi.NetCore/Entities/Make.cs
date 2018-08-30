using System;
using System.Collections.Generic;

namespace SwitcheoApi.NetCore.Entities
{
    public class Make
    {
        public string id { get; set; }
        public string offer_hash { get; set; }
        public string available_amount { get; set; }
        public string offer_asset_id { get; set; }
        public decimal offer_amount { get; set; }
        public string want_asset_id { get; set; }
        public decimal want_amount { get; set; }
        public string fill_amount { get; set; }
        public Transaction txn { get; set; }
        public Transaction cancel_txn { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public DateTimeOffset created_at { get; set; }
        public string transaction_hash { get; set; }
        public TradeDetail[] trades { get; set; }
    }
}
