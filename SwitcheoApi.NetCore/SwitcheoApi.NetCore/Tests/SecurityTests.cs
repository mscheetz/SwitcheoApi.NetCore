using SwitcheoApi.NetCore.Entities;
using Xunit;

namespace SwitcheoApi.NetCore.Core.Tests
{
    public class SecurityTests
    {
        private Security _security;
        private Helper _helper;
        // Empty neo wallet for testing purposes only
        private string _address = "AGA7VMVRpRDULskJ7sWsUt9YuhVj6CHz8y";
        private string _privateKey = "L3SDs1rP2Fs489VGFY4Lt2NAg3Km1PqJsBkQd4QsN8UvotGif1yZ";
        private string _v2ContractHash = "48756743d524af03aa75729e911651ffd3cbe7d8";

        public SecurityTests()
        {
            _security = new Security();
            _helper = new Helper();
        }

        [Fact]
        public void CreateMessageString_Test()
        {
            // Arrange
            var depositData = new DepositWithdrawalParams
            {
                amount = 5.5M,
                asset_id = "SWTH",
                blockchain = "neo",
                contract_hash = _v2ContractHash,
                timestamp = _helper.UTCtoUnixTime()
            };

            // Act
            var sig = _security.CreateMessageString(depositData);

            // Assert
            Assert.NotNull(sig);
        }
    }
}
