namespace SwitcheoApi.NetCore.Entities
{
    public class ScriptParams
    {
        public string scriptHash { get; set; }
        public string operation { get; set; }
        public string[] args { get; set; }
    }
}
