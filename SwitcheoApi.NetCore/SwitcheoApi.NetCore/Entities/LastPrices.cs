using System.Collections.Generic;

namespace SwitcheoApi.NetCore.Entities
{
    public class LastPrices
    {
        public Dictionary<string, Dictionary<string, decimal>> prices { get; set; }
    }
}
