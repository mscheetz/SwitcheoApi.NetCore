using Neo;
using NeoModules.NEP6.Transactions;
using SwitcheoApi.NetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static NeoModules.NEP6.Transactions.SignedTransaction;

namespace SwitcheoApi.NetCore.Core
{
    public class TransactionProcessor
    {
        /// <summary>
        /// Get a SignedTransaction from a Transaction
        /// </summary>
        /// <param name="txn">Transaction to serialize</param>
        /// <returns>SignedTransaction object</returns>
        public SignedTransaction GetSignedTransaction(Transaction txn)
        {
            var signedTxn = new SignedTransaction
            {
                Attributes = GetTransactionAttributes(txn.attributes),
                Gas = txn.gas,
                Inputs = GetTransactionInputs(txn.inputs),
                Outputs = GetTransactionOutputs(txn.outputs),
                Script = txn.script.HexToBytes(),
                Type = (TransactionType)txn.type,
                Version = (byte)txn.version
            };

            return signedTxn;
        }

        /// <summary>
        /// Get TransactionAttribute array from TransactionAttributes array
        /// </summary>
        /// <param name="attrs">TransactionAttributes to convert</param>
        /// <returns>TransactionAttribute array</returns>
        public TransactionAttribute[] GetTransactionAttributes(TransactionAttributes[] attrs)
        {
            var attrList = new List<TransactionAttribute>();

            for (int i = 0; i < attrs.Length; i++)
            {
                var txnAttr = new TransactionAttribute
                {
                    Data = attrs[i].data.HexToBytes(),
                    Usage = (TransactionAttributeUsage)attrs[i].usage
                };

                attrList.Add(txnAttr);
            }

            return attrList.ToArray();
        }

        /// <summary>
        /// Get Input array from TransactionInput array
        /// </summary>
        /// <param name="inputs">TransactionInput to convert</param>
        /// <returns>Input array</returns>
        public Input[] GetTransactionInputs(TransactionInput[] inputs)
        {
            var inputList = new List<Input>();

            for (int i = 0; i < inputs.Length; i++)
            {
                var input = new Input
                {
                    PrevHash = inputs[i].prevHash.HexToBytes().Reverse().ToArray(),// Encoding.UTF8.GetBytes(inputs[i].prevHash),
                    PrevIndex = (uint)inputs[i].prevIndex
                };

                inputList.Add(input);
            }

            return inputList.ToArray();
        }

        /// <summary>
        /// Get Output array from TransactionOutput array
        /// </summary>
        /// <param name="outputs">TransactionOutput to convert</param>
        /// <returns>Output array</returns>
        public Output[] GetTransactionOutputs(Entities.TransactionOutput[] outputs)
        {
            var outputList = new List<Output>();

            for (int i = 0; i < outputs.Length; i++)
            {
                var output = new Output
                {
                    AssetId = outputs[i].assetId.HexToBytes().Reverse().ToArray(),
                    ScriptHash = outputs[i].scriptHash.HexToBytes().Reverse().ToArray(),
                    Value = outputs[i].value
                };

                outputList.Add(output);
            }

            return outputList.ToArray();
        }

        /// <summary>
        /// Get Witness array from string array
        /// </summary>
        /// <param name="scripts">scripts to convert</param>
        /// <returns>Witness array</returns>
        public Witness[] GetTransactionWitnesses(string[] scripts)
        {
            var witnessList = new List<Witness>();

            if (scripts.Length > 0)
            {
                var witness = new Witness
                {
                    InvocationScript = Encoding.UTF8.GetBytes(scripts[0]),
                    VerificationScript = Encoding.UTF8.GetBytes(scripts[1])
                };
                witnessList.Add(witness);
            }

            return witnessList.ToArray();
        }
    }
}
