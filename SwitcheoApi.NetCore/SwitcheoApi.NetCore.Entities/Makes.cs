using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class Makes
    {
        public Guid id { get; set; }
        public string offer_hash { get; set; }
        public decimal available_amount { get; set; }
        public string offer_asset_id { get; set; }
        public decimal offer_amount { get; set; }
        public string want_asset_id { get; set; }
        public decimal want_amount { get; set; }
        public decimal fill_amount { get; set; }
        public object[] txn { get; set; }
        public object[] cancel_txn { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public DateTimeOffset created_at { get; set; }
        public string transaction_hash { get; set; }
        public TradeDetail[] trades { get; set; }
    }
}
