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
using Channels.Contracts.SignatureChecker.ContractDefinition;

namespace Channels.Contracts.SignatureChecker
{
    public partial class SignatureCheckerService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, SignatureCheckerDeployment signatureCheckerDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<SignatureCheckerDeployment>().SendRequestAndWaitForReceiptAsync(signatureCheckerDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, SignatureCheckerDeployment signatureCheckerDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<SignatureCheckerDeployment>().SendRequestAsync(signatureCheckerDeployment);
        }

        public static async Task<SignatureCheckerService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, SignatureCheckerDeployment signatureCheckerDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, signatureCheckerDeployment, cancellationTokenSource);
            return new SignatureCheckerService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public SignatureCheckerService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> GetSignerAddressFromFixedMessageAndSignatureQueryAsync(GetSignerAddressFromFixedMessageAndSignatureFunction getSignerAddressFromFixedMessageAndSignatureFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetSignerAddressFromFixedMessageAndSignatureFunction, string>(getSignerAddressFromFixedMessageAndSignatureFunction, blockParameter);
        }

        
        public Task<string> GetSignerAddressFromFixedMessageAndSignatureQueryAsync(byte[] signature, BlockParameter blockParameter = null)
        {
            var getSignerAddressFromFixedMessageAndSignatureFunction = new GetSignerAddressFromFixedMessageAndSignatureFunction();
                getSignerAddressFromFixedMessageAndSignatureFunction.Signature = signature;
            
            return ContractHandler.QueryAsync<GetSignerAddressFromFixedMessageAndSignatureFunction, string>(getSignerAddressFromFixedMessageAndSignatureFunction, blockParameter);
        }

        public Task<string> GetSignerAddressFromMessageAndSignatureQueryAsync(GetSignerAddressFromMessageAndSignatureFunction getSignerAddressFromMessageAndSignatureFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetSignerAddressFromMessageAndSignatureFunction, string>(getSignerAddressFromMessageAndSignatureFunction, blockParameter);
        }

        
        public Task<string> GetSignerAddressFromMessageAndSignatureQueryAsync(string message, byte[] signature, BlockParameter blockParameter = null)
        {
            var getSignerAddressFromMessageAndSignatureFunction = new GetSignerAddressFromMessageAndSignatureFunction();
                getSignerAddressFromMessageAndSignatureFunction.Message = message;
                getSignerAddressFromMessageAndSignatureFunction.Signature = signature;
            
            return ContractHandler.QueryAsync<GetSignerAddressFromMessageAndSignatureFunction, string>(getSignerAddressFromMessageAndSignatureFunction, blockParameter);
        }

        public Task<string> RecoverSignerQueryAsync(RecoverSignerFunction recoverSignerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RecoverSignerFunction, string>(recoverSignerFunction, blockParameter);
        }

        
        public Task<string> RecoverSignerQueryAsync(byte[] message, byte[] sig, BlockParameter blockParameter = null)
        {
            var recoverSignerFunction = new RecoverSignerFunction();
                recoverSignerFunction.Message = message;
                recoverSignerFunction.Sig = sig;
            
            return ContractHandler.QueryAsync<RecoverSignerFunction, string>(recoverSignerFunction, blockParameter);
        }

        public Task<SplitSignatureOutputDTO> SplitSignatureQueryAsync(SplitSignatureFunction splitSignatureFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<SplitSignatureFunction, SplitSignatureOutputDTO>(splitSignatureFunction, blockParameter);
        }

        public Task<SplitSignatureOutputDTO> SplitSignatureQueryAsync(byte[] sig, BlockParameter blockParameter = null)
        {
            var splitSignatureFunction = new SplitSignatureFunction();
                splitSignatureFunction.Sig = sig;
            
            return ContractHandler.QueryDeserializingToObjectAsync<SplitSignatureFunction, SplitSignatureOutputDTO>(splitSignatureFunction, blockParameter);
        }
    }
}
