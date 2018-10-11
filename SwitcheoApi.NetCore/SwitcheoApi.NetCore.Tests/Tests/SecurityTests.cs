using SwitcheoApi.NetCore.Entities;
using SwitcheoApi.NetCore.Tests;
using Xunit;

namespace SwitcheoApi.NetCore.Core.Tests
{
    public class SecurityTests
    {
        private Security _security;
        private Helper _helper;
        // Empty neo wallet for testing purposes only
        private string _privateKey;

        public SecurityTests()
        {
            TestObjects objs = new TestObjects();
            _privateKey = objs.GetWIF();
            _security = new Security();
            _helper = new Helper();
        }

        [Fact]
        public void Test()
        {
            
        }
    }
}
