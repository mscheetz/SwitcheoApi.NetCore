namespace SwitcheoApi.NetCore.Entities
{
    public class DepositWithdrawalData : DepositWithdrawalParams
    {
        public string address { get; set; }
        public string signature { get; set; }

        public DepositWithdrawalData(DepositWithdrawalParams param)
        {
            base.amount = param.amount;
            base.asset_id = param.asset_id;
            base.blockchain = param.blockchain;
            base.contract_hash = param.contract_hash;
            base.timestamp = param.timestamp;
        }
    }
}
