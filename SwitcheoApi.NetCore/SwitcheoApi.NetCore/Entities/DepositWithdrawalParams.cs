namespace SwitcheoApi.NetCore.Entities
{
    public class DepositWithdrawalParams
    {
        public string amount { get; set; }
        public string asset_id { get; set; }
        public string blockchain { get; set; }
        public string contract_hash { get; set; }
        public int timestamp { get; set; }
    }
}
