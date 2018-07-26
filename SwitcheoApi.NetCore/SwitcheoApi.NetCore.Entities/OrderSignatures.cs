using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class OrderSignatures
    {
        public string[] fills { get; set; }
        public string[] makes { get; set; }
    }
}
