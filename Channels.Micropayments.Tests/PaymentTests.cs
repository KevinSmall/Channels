using Channels.Contracts.ReceiverPays;
using Channels.Contracts.ReceiverPays.ContractDefinition;
using Channels.Contracts.SignatureChecker;
using Channels.Contracts.SignatureChecker.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Xunit;
using Xunit.Abstractions;

namespace Channels.Micropayments.Tests
{
    public class PaymentTests
    {
        private readonly ITestOutputHelper _output;

        // Nethereum testchain
        private const string blockchainUrl = "http://testchain.nethereum.com:8545";
        private const string privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";

        // Rinkeby
        //private const string blockchainUrl = "https://rinkeby.infura.io/v3/7238211010344719ad14a89db874158c";
        //private const string privateKey = "0x517311d936323b28ca55379280d3b307d354f35ae35b214c6349e9828e809adc";            

        public PaymentTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void SigningTestSimple()
        {
            // The message itself, this is what is being signed
            var msg = "test message 1234567890";
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var web3 = new Web3(new Account(privateKey), blockchainUrl);

            // SIGN. Doesnt need web3, done offline.
            // In:  Message, Signer Private Key
            // Out: Signature (65 bytes)
            Log("--- SIGN ---");
            var signer = new EthereumMessageSigner();
            var signature = signer.HashAndSign(msg, privateKey);
            Log($"signature: {signature} is for message: {msg}");
            Log("");


            // RECOVER with Nethereum
            // In: Message, Signature
            // Out: Signer Address
            Log("--- RECOVER with Nethereum ---");
            var signerAddressRecovered = signer.HashAndEcRecover(msg, signature);
            Log($"Actual Signer address using C# HashAndEcRecover: {signerAddressRecovered}");
            Log($"Expected Signer address: {web3.TransactionManager.Account.Address}");  // since web3 created using same private key as was used to sign
            Log("");


            // RECOVER with Solidity
            // In: Message, Signature
            // Out: Signer Address
            Log("--- RECOVER with Solidity ---");
            Log($"Deploying contract SignatureChecker...");
            var signatureCheckerDeployment = new SignatureCheckerDeployment();
            var signatureCheckerService = await SignatureCheckerService.DeployContractAndGetServiceAsync(web3, signatureCheckerDeployment);
            Log($"SignatureChecker contract address is: {signatureCheckerService.ContractHandler.ContractAddress}");
            var bytesForSignature = signature.HexToByteArray();
            var signerAddressRecoveredSolidity = await signatureCheckerService.GetSignerAddressFromMessageAndSignatureQueryAsync(msg, bytesForSignature);
            Log($"Actual Signer Address Recovered using Solidity: {signerAddressRecoveredSolidity}");

            // See Solidity code Channels\Channels.Contracts\Contracts\SignatureChecker.sol:
            //   // this recreates the message that was signed on the client
            //   bytes32 messageAsClient = prefixed(keccak256(abi.encodePacked(message)));
        }

        [Fact]
        public async void DeployReceiverPays()
        {
            // Web3
            var web3 = new Web3(new Account(privateKey), blockchainUrl);

            Log($"Deploying contract ReceiverPays...");
            var receiverPaysDeployment = new ReceiverPaysDeployment();
            var receiverPaysService = await ReceiverPaysService.DeployContractAndGetServiceAsync(web3, receiverPaysDeployment);
            Log($"ReceiverPays address is: {receiverPaysService.ContractHandler.ContractAddress}");

        }

        private void Log(string message)
        {
            _output.WriteLine(message);
        }
    }
}
