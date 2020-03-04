using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Channels.Contracts.AddressRegistry.ContractDefinition
{


    public partial class AddressRegistryDeployment : AddressRegistryDeploymentBase
    {
        public AddressRegistryDeployment() : base(BYTECODE) { }
        public AddressRegistryDeployment(string byteCode) : base(byteCode) { }
    }

    public class AddressRegistryDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "6080604052600060025534801561001557600080fd5b50610305806100256000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c806321f8a72114610046578063662de3791461006f578063b7dfc97914610084575b600080fd5b61005961005436600461020c565b610097565b604051610066919061025e565b60405180910390f35b61008261007d366004610224565b6100b2565b005b61005961009236600461020c565b6101f1565b6000908152602081905260409020546001600160a01b031690565b6001600160a01b0381166100e15760405162461bcd60e51b81526004016100d890610272565b60405180910390fd5b6000828152602081905260409020546001600160a01b0316806101925760008381526020819052604080822080546001600160a01b0319166001600160a01b0386169081179091556001805480820182558185527fb10e2d527612073b26eecdfd717e6a320cf44b4afac2b0732d9fcbe2b7fa0cf6018790556002805490910190559051909185917f5eb4c6beca7ccf6f6394636d7109497abbd6c326e528c3efab53ae58031188959190a36101ec565b60008381526020819052604080822080546001600160a01b0319166001600160a01b03868116918217909255915191929084169186917fd79605f6cff49b16d2f83477ad235178dbe333807ab20e95d695e875525ab98491a45b505050565b6000602081905290815260409020546001600160a01b031681565b60006020828403121561021d578081fd5b5035919050565b60008060408385031215610236578081fd5b8235915060208301356001600160a01b0381168114610253578182fd5b809150509250929050565b6001600160a01b0391909116815260200190565b60208082526038908201527f416464726573732063616e6e6f74206265203078302c2075736520307831207460408201527f6f2064652d726567697374657220616e2061646472657373000000000000000060608201526080019056fea264697066735822122067dd596c12ef0a66f1d74adc1412dfb435babad81975326d17f6a6a895de0d1e64736f6c63430006010033";
        public AddressRegistryDeploymentBase() : base(BYTECODE) { }
        public AddressRegistryDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AddressMapFunction : AddressMapFunctionBase { }

    [Function("addressMap", "address")]
    public class AddressMapFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "", 1)]
        public virtual byte[] ReturnValue1 { get; set; }
    }

    public partial class GetAddressFunction : GetAddressFunctionBase { }

    [Function("getAddress", "address")]
    public class GetAddressFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "contractName", 1)]
        public virtual byte[] ContractName { get; set; }
    }

    public partial class RegisterAddressFunction : RegisterAddressFunctionBase { }

    [Function("registerAddress")]
    public class RegisterAddressFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "contractName", 1)]
        public virtual byte[] ContractName { get; set; }
        [Parameter("address", "a", 2)]
        public virtual string A { get; set; }
    }

    public partial class ContractAddressChangedEventDTO : ContractAddressChangedEventDTOBase { }

    [Event("ContractAddressChanged")]
    public class ContractAddressChangedEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "contractName", 1, true )]
        public virtual byte[] ContractName { get; set; }
        [Parameter("address", "oldContractAddress", 2, true )]
        public virtual string OldContractAddress { get; set; }
        [Parameter("address", "newContractAddress", 3, true )]
        public virtual string NewContractAddress { get; set; }
    }

    public partial class ContractAddressRegisteredEventDTO : ContractAddressRegisteredEventDTOBase { }

    [Event("ContractAddressRegistered")]
    public class ContractAddressRegisteredEventDTOBase : IEventDTO
    {
        [Parameter("bytes32", "contractName", 1, true )]
        public virtual byte[] ContractName { get; set; }
        [Parameter("address", "contractAddress", 2, true )]
        public virtual string ContractAddress { get; set; }
    }

    public partial class AddressMapOutputDTO : AddressMapOutputDTOBase { }

    [FunctionOutput]
    public class AddressMapOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetAddressOutputDTO : GetAddressOutputDTOBase { }

    [FunctionOutput]
    public class GetAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "a", 1)]
        public virtual string A { get; set; }
    }


}
