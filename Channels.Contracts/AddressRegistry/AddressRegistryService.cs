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
using Channels.Contracts.AddressRegistry.ContractDefinition;

namespace Channels.Contracts.AddressRegistry
{
    public partial class AddressRegistryService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, AddressRegistryDeployment addressRegistryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<AddressRegistryDeployment>().SendRequestAndWaitForReceiptAsync(addressRegistryDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, AddressRegistryDeployment addressRegistryDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<AddressRegistryDeployment>().SendRequestAsync(addressRegistryDeployment);
        }

        public static async Task<AddressRegistryService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, AddressRegistryDeployment addressRegistryDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, addressRegistryDeployment, cancellationTokenSource);
            return new AddressRegistryService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public AddressRegistryService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddressMapQueryAsync(AddressMapFunction addressMapFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AddressMapFunction, string>(addressMapFunction, blockParameter);
        }

        
        public Task<string> AddressMapQueryAsync(byte[] returnValue1, BlockParameter blockParameter = null)
        {
            var addressMapFunction = new AddressMapFunction();
                addressMapFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<AddressMapFunction, string>(addressMapFunction, blockParameter);
        }

        public Task<string> GetAddressQueryAsync(GetAddressFunction getAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetAddressFunction, string>(getAddressFunction, blockParameter);
        }

        
        public Task<string> GetAddressQueryAsync(byte[] contractName, BlockParameter blockParameter = null)
        {
            var getAddressFunction = new GetAddressFunction();
                getAddressFunction.ContractName = contractName;
            
            return ContractHandler.QueryAsync<GetAddressFunction, string>(getAddressFunction, blockParameter);
        }

        public Task<string> RegisterAddressRequestAsync(RegisterAddressFunction registerAddressFunction)
        {
             return ContractHandler.SendRequestAsync(registerAddressFunction);
        }

        public Task<TransactionReceipt> RegisterAddressRequestAndWaitForReceiptAsync(RegisterAddressFunction registerAddressFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerAddressFunction, cancellationToken);
        }

        public Task<string> RegisterAddressRequestAsync(byte[] contractName, string a)
        {
            var registerAddressFunction = new RegisterAddressFunction();
                registerAddressFunction.ContractName = contractName;
                registerAddressFunction.A = a;
            
             return ContractHandler.SendRequestAsync(registerAddressFunction);
        }

        public Task<TransactionReceipt> RegisterAddressRequestAndWaitForReceiptAsync(byte[] contractName, string a, CancellationTokenSource cancellationToken = null)
        {
            var registerAddressFunction = new RegisterAddressFunction();
                registerAddressFunction.ContractName = contractName;
                registerAddressFunction.A = a;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerAddressFunction, cancellationToken);
        }
    }
}
