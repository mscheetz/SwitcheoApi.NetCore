using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class TransactionInput
    {
        public string prevHash { get; set; }
        public uint prevIndex { get; set; }
    }
}
