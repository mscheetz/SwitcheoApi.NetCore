using Switcheo.NetCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SwitcheoApi.NetCore.Tests
{
    public class SwitcheoApiClientTests
    {
        // Empty neo wallet for testing purposes only
        private string _privateKey = "L3SDs1rP2Fs489VGFY4Lt2NAg3Km1PqJsBkQd4QsN8UvotGif1yZ";
        private string _address = "AGA7VMVRpRDULskJ7sWsUt9YuhVj6CHz8y";
        private string _scriptHash = "0x3161dab9941504e080db38f56ed9c722c7b43404";

        public SwitcheoApiClientTests()
        {
        }

        [Fact]
        public void GetWallet_Test_ExceptionThrown()
        {
            var client = new SwitcheoApiClient();

            Assert.Throws<Exception>(() => client.GetWallet());
        }

        [Fact]
        public void GetWallet_Test_PKLoad()
        {
            var client = new SwitcheoApiClient(_privateKey);

            var wallet = client.GetWallet();

            Assert.True(wallet.address == _address);
            Assert.True(wallet.scriptHash == _scriptHash);
        }

        [Fact]
        public void GetWallet_Test_AddyLoad()
        {
            var client = new SwitcheoApiClient(_address);

            var wallet = client.GetWallet();

            Assert.True(wallet.scriptHash == _scriptHash);
        }

        [Fact]
        public void GetWallet_Test_SCLoad()
        {
            var client = new SwitcheoApiClient(_scriptHash);

            var wallet = client.GetWallet();

            Assert.True(wallet.address == _address);
        }
    }
}
