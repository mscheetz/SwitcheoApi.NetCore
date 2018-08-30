using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class Order
    {
        public string id { get; set; }
        public string blockchain { get; set; }
        public string contract_hash { get; set; }
        public string address { get; set; }
        public string side { get; set; }
        public string offer_asset_id { get; set; }
        public string want_asset_id { get; set; }
        public string offer_amount { get; set; }
        public string want_amount { get; set; }
        public string transfer_amount { get; set; }
        public string priority_gas_amount { get; set; }
        public bool use_native_token { get; set; }
        public decimal native_fee_transer_amount { get; set; }
        public string deposit_txn { get; set; }
        public DateTimeOffset created_at { get; set; }
        public string status { get; set; }
        public Fill[] fills { get; set; }
        public Make[] makes { get; set; }
    }
}
