using SwitcheoApi.NetCore.Core;
using SwitcheoApi.NetCore.Entities;
using System.Security;
using Xunit;

namespace SwitcheoApi.NetCore.Tests
{
    public class EntitiesTests
    {
        private string _address = "AGA7VMVRpRDULskJ7sWsUt9YuhVj6CHz8y";
        private string _privateKey = "L3SDs1rP2Fs489VGFY4Lt2NAg3Km1PqJsBkQd4QsN8UvotGif1yZ";
        private string _scriptHash = "0x3161dab9941504e080db38f56ed9c722c7b43404";
        private Helper _helper;
        private SecureString _securePK;

        public EntitiesTests()
        {
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
