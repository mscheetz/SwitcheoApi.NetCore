using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class WithdrawalResponse
    {
        public string event_type { get; set; }
        public decimal amount { get; set; }
        public string asset_id { get; set; }
        public string status { get; set; }
        public Guid id { get; set; }
        public string blockchain { get; set; }
        public int reason_code { get; set; }
        public string address { get; set; }
        public string transaction_hash { get; set; }
        public DateTimeOffset created_at { get; set; }
        public DateTimeOffset update_at { get; set; }
        public string contract_hash { get; set; }
    }
}
