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

namespace Channels.Contracts.SignatureChecker.ContractDefinition
{


    public partial class SignatureCheckerDeployment : SignatureCheckerDeploymentBase
    {
        public SignatureCheckerDeployment() : base(BYTECODE) { }
        public SignatureCheckerDeployment(string byteCode) : base(byteCode) { }
    }

    public class SignatureCheckerDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "6080604052600080546001600160a01b031916331790556104c6806100256000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c8063640fbe761461004657806397aba7f91461018b578063a7bb580314610236575b600080fd5b61016f6004803603604081101561005c57600080fd5b810190602081018135600160201b81111561007657600080fd5b82018360208201111561008857600080fd5b803590602001918460018302840111600160201b831117156100a957600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295949360208101935035915050600160201b8111156100fb57600080fd5b82018360208201111561010d57600080fd5b803590602001918460018302840111600160201b8311171561012e57600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295506102fc945050505050565b604080516001600160a01b039092168252519081900360200190f35b61016f600480360360408110156101a157600080fd5b81359190810190604081016020820135600160201b8111156101c257600080fd5b8201836020820111156101d457600080fd5b803590602001918460018302840111600160201b831117156101f557600080fd5b91908080601f016020809104026020016040519081016040528093929190818152602001838380828437600092019190915250929550610389945050505050565b6102da6004803603602081101561024c57600080fd5b810190602081018135600160201b81111561026657600080fd5b82018360208201111561027857600080fd5b803590602001918460018302840111600160201b8311171561029957600080fd5b91908080601f016020809104026020016040519081016040528093929190818152602001838380828437600092019190915250929550610410945050505050565b6040805160ff9094168452602084019290925282820152519081900360600190f35b600080610375846040516020018082805190602001908083835b602083106103355780518252601f199092019160209182019101610316565b6001836020036101000a0380198251168184511680821785525050505050509050019150506040516020818303038152906040528051906020012061043f565b90506103818184610389565b949350505050565b60008060008061039885610410565b92509250925060018684848460405160008152602001604052604051808581526020018460ff1660ff1681526020018381526020018281526020019450505050506020604051602081039080840390855afa1580156103fb573d6000803e3d6000fd5b5050604051601f190151979650505050505050565b6000806000835160411461042357600080fd5b5050506020810151604082015160609092015160001a92909190565b604080517f19457468657265756d205369676e6564204d6573736167653a0a333200000000602080830191909152603c8083019490945282518083039094018452605c90910190915281519101209056fea264697066735822122011e2fbe5f821edab6a26b8d5fd25374b066c8c223b05d690fabec6ed638cfe5664736f6c63430006010033";
        public SignatureCheckerDeploymentBase() : base(BYTECODE) { }
        public SignatureCheckerDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetSignerAddressFromMessageAndSignatureFunction : GetSignerAddressFromMessageAndSignatureFunctionBase { }

    [Function("getSignerAddressFromMessageAndSignature", "address")]
    public class GetSignerAddressFromMessageAndSignatureFunctionBase : FunctionMessage
    {
        [Parameter("string", "message", 1)]
        public virtual string Message { get; set; }
        [Parameter("bytes", "signature", 2)]
        public virtual byte[] Signature { get; set; }
    }

    public partial class RecoverSignerFunction : RecoverSignerFunctionBase { }

    [Function("recoverSigner", "address")]
    public class RecoverSignerFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "message", 1)]
        public virtual byte[] Message { get; set; }
        [Parameter("bytes", "sig", 2)]
        public virtual byte[] Sig { get; set; }
    }

    public partial class SplitSignatureFunction : SplitSignatureFunctionBase { }

    [Function("splitSignature", typeof(SplitSignatureOutputDTO))]
    public class SplitSignatureFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "sig", 1)]
        public virtual byte[] Sig { get; set; }
    }

    public partial class GetSignerAddressFromMessageAndSignatureOutputDTO : GetSignerAddressFromMessageAndSignatureOutputDTOBase { }

    [FunctionOutput]
    public class GetSignerAddressFromMessageAndSignatureOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class RecoverSignerOutputDTO : RecoverSignerOutputDTOBase { }

    [FunctionOutput]
    public class RecoverSignerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class SplitSignatureOutputDTO : SplitSignatureOutputDTOBase { }

    [FunctionOutput]
    public class SplitSignatureOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint8", "v", 1)]
        public virtual byte V { get; set; }
        [Parameter("bytes32", "r", 2)]
        public virtual byte[] R { get; set; }
        [Parameter("bytes32", "s", 3)]
        public virtual byte[] S { get; set; }
    }
}
