using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class TransactionDetail
    {
        public string hash { get; set; }
        public string sha256 { get; set; }
        public int type { get; set; }
        public int version { get; set; }
        public string[] attributes { get; set; }
        public string[] inputs { get; set; }
        public string[] outputs { get; set; }
        public string[] scripts { get; set; }
        public string script { get; set; }
        public decimal gas { get; set; }
    }
}
