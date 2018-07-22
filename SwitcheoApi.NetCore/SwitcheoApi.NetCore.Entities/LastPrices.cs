using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class LastPrices
    {
        public Dictionary<string, Dictionary<string, decimal>> prices { get; set; }
    }
}
