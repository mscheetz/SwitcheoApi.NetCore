using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class TransactionOutput
    {
        public string assetId { get; set; }
        public string scriptHash { get; set; }
        public decimal value { get; set; }
    }
}
