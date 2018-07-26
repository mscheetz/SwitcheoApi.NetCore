using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class OrderRequest
    {
        public string pair { get; set; }
        public string blockchain { get; set; }
        public string side { get; set; }
        public decimal price { get; set; }
        public decimal want_amount { get; set; }
        public bool use_native_tokens { get; set; }
        public string order_type { get; set; }
        public long timestamp { get; set; }
        public string contract_hash { get; set; }
    }
}
