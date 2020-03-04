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
        public static string BYTECODE = "6080604052600080546001600160a01b031916331790556105da806100256000396000f3fe608060405234801561001057600080fd5b506004361061004c5760003560e01c8063640fbe761461005157806397aba7f914610196578063a7bb580314610241578063e40769a714610307575b600080fd5b61017a6004803603604081101561006757600080fd5b810190602081018135600160201b81111561008157600080fd5b82018360208201111561009357600080fd5b803590602001918460018302840111600160201b831117156100b457600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295949360208101935035915050600160201b81111561010657600080fd5b82018360208201111561011857600080fd5b803590602001918460018302840111600160201b8311171561013957600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295506103ab945050505050565b604080516001600160a01b039092168252519081900360200190f35b61017a600480360360408110156101ac57600080fd5b81359190810190604081016020820135600160201b8111156101cd57600080fd5b8201836020820111156101df57600080fd5b803590602001918460018302840111600160201b8311171561020057600080fd5b91908080601f016020809104026020016040519081016040528093929190818152602001838380828437600092019190915250929550610438945050505050565b6102e56004803603602081101561025757600080fd5b810190602081018135600160201b81111561027157600080fd5b82018360208201111561028357600080fd5b803590602001918460018302840111600160201b831117156102a457600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295506104bf945050505050565b6040805160ff9094168452602084019290925282820152519081900360600190f35b61017a6004803603602081101561031d57600080fd5b810190602081018135600160201b81111561033757600080fd5b82018360208201111561034957600080fd5b803590602001918460018302840111600160201b8311171561036a57600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295506104ee945050505050565b600080610424846040516020018082805190602001908083835b602083106103e45780518252601f1990920191602091820191016103c5565b6001836020036101000a03801982511681845116808217855250505050505090500191505060405160208183030381529060405280519060200120610553565b90506104308184610438565b949350505050565b600080600080610447856104bf565b92509250925060018684848460405160008152602001604052604051808581526020018460ff1660ff1681526020018381526020018281526020019450505050506020604051602081039080840390855afa1580156104aa573d6000803e3d6000fd5b5050604051601f190151979650505050505050565b600080600083516041146104d257600080fd5b5050506020810151604082015160609092015160001a92909190565b60008061054060405160200180807f74657374206d6573736167652031323334353637383930000000000000000000815250601701905060405160208183030381529060405280519060200120610553565b905061054c8184610438565b9392505050565b604080517f19457468657265756d205369676e6564204d6573736167653a0a333200000000602080830191909152603c8083019490945282518083039094018452605c90910190915281519101209056fea2646970667358221220f5904e0ebf2c937d5be1c182b234fda3eb1e0087d43c63c326710984eb5cc99f64736f6c63430006010033";
        public SignatureCheckerDeploymentBase() : base(BYTECODE) { }
        public SignatureCheckerDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetSignerAddressFromFixedMessageAndSignatureFunction : GetSignerAddressFromFixedMessageAndSignatureFunctionBase { }

    [Function("getSignerAddressFromFixedMessageAndSignature", "address")]
    public class GetSignerAddressFromFixedMessageAndSignatureFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "signature", 1)]
        public virtual byte[] Signature { get; set; }
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

    public partial class GetSignerAddressFromFixedMessageAndSignatureOutputDTO : GetSignerAddressFromFixedMessageAndSignatureOutputDTOBase { }

    [FunctionOutput]
    public class GetSignerAddressFromFixedMessageAndSignatureOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
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
