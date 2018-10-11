using SwitcheoApi.NetCore.Core;
using SwitcheoApi.NetCore.Entities;
using System.Security;
using Xunit;

namespace SwitcheoApi.NetCore.Tests
{
    public class EntitiesTests
    {
        private string _address;
        private string _privateKey;
        private string _scriptHash;
        private Helper _helper;
        private SecureString _securePK;

        public EntitiesTests()
        {
            TestObjects objs = new TestObjects();
            _address = objs.GetAddress();
            _privateKey = objs.GetWIF();
            _scriptHash = objs.GetScriptHash();
            _helper = new Helper();
            _securePK = _helper.GetSecureString(_privateKey);
        }

        [Fact]
        public void NeoWalletPrivateKey_Test()
        {
            var neoWallet = new NeoWallet(_securePK);

            Assert.True(_address == neoWallet.address);
            Assert.True(_scriptHash == neoWallet.scriptHash);
        }

        [Fact]
        public void NeoWalletAddress_Test()
        {
            var neoWallet = new NeoWallet(_address);

            Assert.True(_scriptHash == neoWallet.scriptHash);
        }

        [Fact]
        public void NeoWalletScriptHash_Test()
        {
            var neoWallet = new NeoWallet(_scriptHash);

            Assert.True(_address == neoWallet.address);
        }
    }
}
