using SwitcheoApi.NetCore.Entities;
using Xunit;

namespace SwitcheoApi.NetCore.Core.Tests
{
    public class SecurityTests
    {
        private Security _security;
        private Helper _helper;
        // Empty neo wallet for testing purposes only
        private string _privateKey = "L3SDs1rP2Fs489VGFY4Lt2NAg3Km1PqJsBkQd4QsN8UvotGif1yZ";

        public SecurityTests()
        {
            _security = new Security();
            _helper = new Helper();
        }

        [Fact]
        public void Test()
        {
            
        }
    }
}
