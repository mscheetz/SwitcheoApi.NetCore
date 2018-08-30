namespace SwitcheoApi.NetCore.Entities
{
    public class OrderRequest
    {
        public string blockchain { get; set; }
        public string contract_hash { get; set; }
        public string order_type { get; set; }
        public string pair { get; set; }
        public string price { get; set; }
        public string side { get; set; }
        public long timestamp { get; set; }
        public bool use_native_tokens { get; set; }
        public string want_amount { get; set; }
    }
}
