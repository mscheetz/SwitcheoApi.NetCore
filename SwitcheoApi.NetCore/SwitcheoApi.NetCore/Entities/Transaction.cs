using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class Transaction
    {
        public string offerHash { get; set; }
        public string hash { get; set; }
        public string sha256 { get; set; }
        public Invoke invoke { get; set; }
        public int type { get; set; }
        public int version { get; set; }
        public TransactionAttributes[] attributes { get; set; }
        public TransactionInput[] inputs { get; set; }
        public TransactionOutput[] outputs { get; set; }
        public string[] scripts { get; set; }
        public string script { get; set; }
        public decimal gas { get; set; }
    }
}
