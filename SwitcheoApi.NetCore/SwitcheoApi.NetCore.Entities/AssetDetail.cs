using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class AssetDetail
    {
        public string event_type { get; set; }
        public string asset_id { get; set; }
        public decimal amount { get; set; }
        public string transaction_hash { get; set; }
        public DateTimeOffset created_at { get; set; }
    }
}
