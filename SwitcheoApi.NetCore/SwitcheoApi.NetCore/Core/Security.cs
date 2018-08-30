using Neo;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using SwitcheoApi.NetCore.Entities;
using System;
using System.Text;

namespace SwitcheoApi.NetCore.Core
{
    public class Security
    {
        /// <summary>
        /// Sign a message
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="wallet">Wallet for signature</param>
        /// <returns>String of signature</returns>
        public string SignMessage(string message, NeoWallet wallet)
        {
            var serializedTransaction = PrepAndSerializeMessage(message);

            return GenerateSignature(serializedTransaction, wallet);
        }

        /// <summary>
        /// Sign a message
        /// </summary>
        /// <param name="toSign">Message to sign</param>
        /// <param name="wallet">Wallet for signature</param>
        /// <returns>String of signature</returns>
        public string SignTransaction(byte[] toSign, NeoWallet wallet)
        {
            return GenerateSignature(toSign, wallet);
        }

        /// <summary>
        /// Prepare a message to sign
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <returns>String of prepared message</returns>
        private string PrepMessage(string message)
        {
            var msgBytes = Encoding.UTF8.GetBytes(message);
            var parameterHexString = msgBytes.ToHexString();
            var lengthHex = (parameterHexString.Length / 2).ToString("X").PadLeft(2, '0');
            var concatString = lengthHex + parameterHexString;
            var serializedTransaction = "010001f0" + concatString + "0000";

            return serializedTransaction;
        }

        /// <summary>
        /// Prepare and serialze a message
        /// </summary>
        /// <param name="message">Mesage to prep</param>
        /// <returns>Byte array of message</returns>
        private byte[] PrepAndSerializeMessage(string message)
        {
            var serializedTransaction = PrepMessage(message);

            return serializedTransaction.HexToBytes();
        }

        /// <summary>
        /// Generate a signature
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="wallet">Wallet for signature</param>
        /// <returns>Message signature</returns>
        private string GenerateSignature(byte[] message, NeoWallet wallet)
        {
            var privateKey = wallet.privateKey;

            var curve = SecNamedCurves.GetByName("secp256r1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

            var priv = new ECPrivateKeyParameters("ECDSA", (new BigInteger(1, privateKey)), domain);
            var signer = new ECDsaSigner();
            var fullsign = new byte[64];

            var hash = new Sha256Digest();
            hash.BlockUpdate(message, 0, message.Length);

            var result = new byte[32];
            hash.DoFinal(result, 0);

            message = result;

            signer.Init(true, priv);
            var signature = signer.GenerateSignature(message);

            var signedResult = ProcessSignature(signature);

            var signedMessage = BitConverter.ToString(signedResult);

            return signedMessage.Replace("-", "").ToLower();
        }

        /// <summary>
        /// Process a signature
        /// </summary>
        /// <param name="signedMessage">Fully signed message</param>
        /// <returns>Message signature</returns>
        private byte[] ProcessSignature(BigInteger[] signedMessage)
        {
            var ba64 = new byte[64];
            var r = signedMessage[0].ToByteArray();
            var s = signedMessage[1].ToByteArray();
            var rLen = r.Length;
            var sLen = s.Length;

            if (rLen < 32)
            {
                Array.Copy(r, 0, ba64, 32 - rLen, rLen);
            }
            else
            {
                Array.Copy(r, rLen - 32, ba64, 0, 32);
            }

            if (sLen < 32)
            {
                Array.Copy(s, 0, ba64, 64 - sLen, sLen);
            }
            else
            {
                Array.Copy(s, sLen - 32, ba64, 32, 32);
            }

            return ba64;
        }
    }
}
