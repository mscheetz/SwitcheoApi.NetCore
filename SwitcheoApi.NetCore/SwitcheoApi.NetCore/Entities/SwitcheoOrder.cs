using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcheoApi.NetCore.Entities
{
    public class SwitcheoOrder
    {
        public string id { get; set; }
        public string blockchain { get; set; }
        public string address { get; set; }
        public string side { get; set; }
        public string offerAsset { get; set; }
        public string wantAsset { get; set; }
        public decimal originalQuantity { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public string status { get; set; }
        public string orderStatus { get; set; }
        public decimal avgFillPrice { get; set; }
        public decimal filledQuanity { get; set; }
        public decimal remainingQuantity { get; set; }
        public bool useSWTH { get; set; }
        public string feeAsset { get; set; }
        public decimal feeAmount { get; set; }
        public decimal offerAmount { get; set; }
        public decimal wantAmount { get; set; }
        
        public string pair
        {
            get
            {
                return this.wantAsset + "_" + this.offerAsset;
            }
        }

        public decimal price
        {
            get
            {
                return this.wantAmount < this.offerAmount 
                    ? Math.Round(this.wantAmount / this.offerAmount, 8)
                    : Math.Round(this.offerAmount / this.wantAmount, 8);
            }
        }
    }
}
