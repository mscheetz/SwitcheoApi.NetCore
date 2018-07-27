namespace SwitcheoApi.NetCore.Entities
{
    public class OrderRequestSigned : OrderRequest
    {
        public string address { get; set; }
        public string signature { get; set; }

        public OrderRequestSigned(OrderRequest request)
        {
            base.blockchain = request.blockchain;
            base.contract_hash = request.contract_hash;
            base.order_type = request.order_type;
            base.pair = request.pair;
            base.price = request.price;
            base.side = request.side;
            base.timestamp = request.timestamp;
            base.use_native_tokens = request.use_native_tokens;
            base.want_amount = request.want_amount;
        }
    }
}
