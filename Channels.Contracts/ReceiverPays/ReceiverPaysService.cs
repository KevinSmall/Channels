using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Channels.Contracts.ReceiverPays.ContractDefinition;

namespace Channels.Contracts.ReceiverPays
{
    public partial class ReceiverPaysService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ReceiverPaysDeployment receiverPaysDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ReceiverPaysDeployment>().SendRequestAndWaitForReceiptAsync(receiverPaysDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ReceiverPaysDeployment receiverPaysDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ReceiverPaysDeployment>().SendRequestAsync(receiverPaysDeployment);
        }

        public static async Task<ReceiverPaysService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ReceiverPaysDeployment receiverPaysDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, receiverPaysDeployment, cancellationTokenSource);
            return new ReceiverPaysService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ReceiverPaysService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> ClaimPaymentRequestAsync(ClaimPaymentFunction claimPaymentFunction)
        {
             return ContractHandler.SendRequestAsync(claimPaymentFunction);
        }

        public Task<TransactionReceipt> ClaimPaymentRequestAndWaitForReceiptAsync(ClaimPaymentFunction claimPaymentFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimPaymentFunction, cancellationToken);
        }

        public Task<string> ClaimPaymentRequestAsync(BigInteger amount, BigInteger noncePay, byte[] signature)
        {
            var claimPaymentFunction = new ClaimPaymentFunction();
                claimPaymentFunction.Amount = amount;
                claimPaymentFunction.NoncePay = noncePay;
                claimPaymentFunction.Signature = signature;
            
             return ContractHandler.SendRequestAsync(claimPaymentFunction);
        }

        public Task<TransactionReceipt> ClaimPaymentRequestAndWaitForReceiptAsync(BigInteger amount, BigInteger noncePay, byte[] signature, CancellationTokenSource cancellationToken = null)
        {
            var claimPaymentFunction = new ClaimPaymentFunction();
                claimPaymentFunction.Amount = amount;
                claimPaymentFunction.NoncePay = noncePay;
                claimPaymentFunction.Signature = signature;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimPaymentFunction, cancellationToken);
        }

        public Task<string> ShutdownRequestAsync(ShutdownFunction shutdownFunction)
        {
             return ContractHandler.SendRequestAsync(shutdownFunction);
        }

        public Task<string> ShutdownRequestAsync()
        {
             return ContractHandler.SendRequestAsync<ShutdownFunction>();
        }

        public Task<TransactionReceipt> ShutdownRequestAndWaitForReceiptAsync(ShutdownFunction shutdownFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(shutdownFunction, cancellationToken);
        }

        public Task<TransactionReceipt> ShutdownRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<ShutdownFunction>(null, cancellationToken);
        }
    }
}
