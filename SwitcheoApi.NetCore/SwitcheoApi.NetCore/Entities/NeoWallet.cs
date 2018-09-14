using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using System.Security;

namespace SwitcheoApi.NetCore.Entities
{
    /// <summary>
    /// Represents a NEO Wallet
    /// </summary>
    public class NeoWallet
    {
        private Core.Helper _helper = new Core.Helper();
        public string address { get; set; }
        public string scriptHash { get; set; }
        public string publicKey { get; set; }
        public byte[] privateKey { get; set; }
        public KeyPair keyPair { get; set; }
        public string exchangeAddress
        {
            get
            {
                return string.IsNullOrEmpty(scriptHash) ? "" : scriptHash.Substring(2);
            }
        }

        /// <summary>
        /// Constructor to set address
        /// </summary>
        /// <param name="address">String of public address or script hash</param>
        public NeoWallet(string loginValue)
        {
            if (loginValue.Substring(0, 1).Equals("A"))
            {
                this.address = loginValue;
                this.scriptHash = loginValue.ToScriptHash().ToString();
            }
            else
            {
                this.scriptHash = loginValue;
                var valArray = _helper.ValueToByteArray(loginValue);
                var hash = new UInt160(valArray);                
                this.address = hash.ToAddress();
            }
            
        }

        /// <summary>
        /// Constructor to get address and script hash
        /// </summary>
        /// <param name="wif">SecureString of WIF</param>
        public NeoWallet(SecureString wif)
        {
            var pkString = _helper.SecureStringToString(wif);
            var privateKey = Wallet.GetPrivateKeyFromWif(pkString);
            
            var myKeys = new KeyPair(privateKey);
            var publicKeyString = myKeys.PublicKey.ToString();
            
            var hash = NeoModules.KeyPairs.Helper.CreateSignatureRedeemScript(myKeys.PublicKey).ToScriptHash();
            var hashBytes = myKeys.PublicKeyHash;
            var address = hash.ToAddress();

            this.keyPair = myKeys;
            this.publicKey = publicKeyString;
            this.privateKey = privateKey;
            this.address = address;
            this.scriptHash = hash.ToString();
        }
    }
}
