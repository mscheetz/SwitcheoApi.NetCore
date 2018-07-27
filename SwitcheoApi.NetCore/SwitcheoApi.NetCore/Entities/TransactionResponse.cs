using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class TransactionResponse
    {
        public Guid id { get; set; }
        public TransactionDetail transaction { get; set; }
        public ScriptParams script_params { get; set; }
    }
}
