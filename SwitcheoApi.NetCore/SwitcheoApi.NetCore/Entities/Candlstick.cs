namespace SwitcheoApi.NetCore.Entities
{
    public class Candlstick
    {
        public string pair { get; set; }
        public long time { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public double volume { get; set; }
        public decimal quote_volume { get; set; }
    }
}
