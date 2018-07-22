using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class DepositResponse
    {
        public Guid id { get; set; }
        public DepositDetail transaction { get; set; }
        public DepositParams script_params { get; set; }
    }
}
