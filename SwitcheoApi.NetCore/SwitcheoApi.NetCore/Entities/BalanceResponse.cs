using System.Collections.Generic;

namespace SwitcheoApi.NetCore.Entities
{
    public class BalanceResponse
    {
        public Dictionary<string, AssetDetail[]> confirming { get; set; }
        public Dictionary<string, decimal> confirmed { get; set; }
        public Dictionary<string, decimal> locked { get; set; }
    }
}
