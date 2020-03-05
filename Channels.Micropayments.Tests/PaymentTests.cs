using Channels.Contracts;
using Channels.Contracts.ReceiverPays;
using Channels.Contracts.ReceiverPays.ContractDefinition;
using Channels.Contracts.SignatureChecker;
using Channels.Contracts.SignatureChecker.ContractDefinition;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Nethereum.Hex.HexConvertors.Extensions;

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
        public async void SigningTest()
        {
            // the content of the message itself, this is what is being signed
            var msg1 = "test message 1234567890";
            Log($"Expected Signer address: 0x12890d2cce102216644c59dae5baed380d84830c");

            // SIGNER 1
            // private key of the signer's address "0x12890d2cce102216644c59dae5baed380d84830c"
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var signer1 = new EthereumMessageSigner();
            var signature1 = signer1.EncodeUTF8AndSign(msg1, new EthECKey(privateKey)); // signature is 65 bytes long, as hex string 130 chars
            Log($"signature1: {signature1} is for message: {msg1}");

            // Check Nethereum recovers address as we expect
            var addressRec1 = signer1.EncodeUTF8AndEcRecover(msg1, signature1);
            Log($"Actual Signer address1 from EncodeUTF8AndEcRecover: {addressRec1}");


            // SIGNER 2
            var signer2 = new EthereumMessageSigner();
            var signature2 = signer2.HashAndSign(msg1, privateKey);
            Log($"signature2: {signature2} is for message: {msg1}");
            var addressRec2 = signer2.HashAndEcRecover(msg1, signature2);
            Log($"Actual Signer address1 from HashAndEcRecover: {addressRec2}");



            // Now try to get solidity to recover address
            var bytesForSig1 = signature1.HexToByteArray();
            var bytesForSig2 = signature2.HexToByteArray();

            // Web3
            var web3 = new Web3(new Account(privateKey), blockchainUrl);

            Log($"Deploying contract SignatureChecker...");
            var signatureCheckerDeployment = new SignatureCheckerDeployment();
            var signatureCheckerService = await SignatureCheckerService.DeployContractAndGetServiceAsync(web3, signatureCheckerDeployment);
            Log($"SignatureChecker contract address is: {signatureCheckerService.ContractHandler.ContractAddress}");


            var signerAddressA = await signatureCheckerService.GetSignerAddressFromMessageAndSignatureQueryAsync(msg1, bytesForSig1);
            Log($"Signer addressA: {signerAddressA}");

            var signerAddressB = await signatureCheckerService.GetSignerAddressFromMessageAndSignatureQueryAsync(msg1, bytesForSig2);
            Log($"Signer addressB: {signerAddressB}");



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
