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
        private string _address;
        private string _privateKey;
        private string _scriptHash;

        public SwitcheoApiClientTests()
        {
            TestObjects objs = new TestObjects();
            _address = objs.GetAddress();
            _privateKey = objs.GetWIF();
            _scriptHash = objs.GetScriptHash();
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
