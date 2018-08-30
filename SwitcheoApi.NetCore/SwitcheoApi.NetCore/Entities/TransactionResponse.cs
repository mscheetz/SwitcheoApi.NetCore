using System;

namespace SwitcheoApi.NetCore.Entities
{
    public class TransactionResponse
    {
        public string id { get; set; }
        public Transaction transaction { get; set; }
        public ScriptParams script_params { get; set; }
    }
}
