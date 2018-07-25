using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace SwitcheoApi.NetCore.Core
{
    public class Security
    {
        /// <summary>
        /// Create a signature
        /// </summary>
        /// <typeparam name="T">Object type of parameters</typeparam>
        /// <param name="paramList">Parameter object</param>
        /// <param name="privateKey">Private key</param>
        /// <returns>String of signature</returns>
        public string CreateSignature<T>(T paramObject, string privateKey)
        {
            var messageString = CreateMessageString(paramObject);

            var signature = SignMessage(messageString, privateKey);

            return signature;
        }

        /// <summary>
        /// Create string for message to sign
        /// </summary>
        /// <typeparam name="T">Object type of parameters</typeparam>
        /// <param name="paramObject">Data object with parameters</param>
        /// <returns>String to sign</returns>
        public string CreateMessageString<T>(T paramObject)
        {
            var paramStringify = JsonConvert.SerializeObject(paramObject);

            byte[] byteString = Encoding.Default.GetBytes(paramStringify);

            var hex = BitConverter.ToString(byteString);

            hex = hex.Replace("-", "");

            var lenHex = (hex.Length / 2).ToString().PadLeft(2, '0');

            var concatenatedString = lenHex + hex;

            var ledgerCompatibleString = "010001f0" + concatenatedString + "0000";

            return ledgerCompatibleString;
        }

        /// <summary>
        /// Sign a message
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="privateKey">Private key to sign with</param>
        /// <returns>String of signature</returns>
        public string SignMessage(string message, string privateKey)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(privateKey);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA256 crypotgrapher = new HMACSHA256(keyBytes);

            byte[] bytes = crypotgrapher.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
