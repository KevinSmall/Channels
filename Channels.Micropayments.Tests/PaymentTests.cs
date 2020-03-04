using Channels.Contracts;
using Channels.Contracts.AddressRegistry;
using Channels.Contracts.AddressRegistry.ContractDefinition;
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

            // private key of the signer's address "0x12890d2cce102216644c59dae5baed380d84830c"
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var signer1 = new EthereumMessageSigner();
            var signature1 = signer1.EncodeUTF8AndSign(msg1, new EthECKey(privateKey));
            Log($"signature: {signature1} is for message: {msg1}");

            // signature: 0x54a4ef561d486a569b3e46783c5ded35d2bd6bf130b4de0569dde559d36d066b365bbaa8114925889eea42e378b0ee70f1fd2ece924978fbcd472fbab2b15a181c 
            // is for message: test message 1234567890


            // Web3
            var web3 = new Web3(new Account(privateKey), blockchainUrl);

            Log($"Deploying contract SignatureChecker...");
            var signatureCheckerDeployment = new SignatureCheckerDeployment();
            var signatureCheckerService = await SignatureCheckerService.DeployContractAndGetServiceAsync(web3, signatureCheckerDeployment);
            Log($"SignatureChecker contract address is: {signatureCheckerService.ContractHandler.ContractAddress}");

            string sigNo0x = signature1.Substring(2);
            byte[] sigBytes = SigToBytes(sigNo0x);
            var signerAddress = await signatureCheckerService.GetSignerAddressFromFixedMessageAndSignatureQueryAsync(sigBytes);
            Log($"Signer address: {signerAddress}");
        }

        private byte[] SigToBytes(string sig)
        {
            byte[] array = new byte[130]; // or 65 ??
            byte[] bytes = Encoding.UTF8.GetBytes(sig);
            if (bytes.Length != 130)
            {
                throw new ArgumentException("After retrieving the UTF8 bytes for the sig, it is not 130 bytes");
            }
            Array.Copy(bytes, 0, array, 0, bytes.Length);
            return array;
        }

        [Fact]
        public async void DeployAddressReg()
        {
            // Web3
            var web3 = new Web3(new Account(privateKey), blockchainUrl);

            var contractName = "AddressRegistry";
            Log($"Deploying {contractName}...");
            var addressRegDeployment = new AddressRegistryDeployment();
            var addressRegistryService = await AddressRegistryService.DeployContractAndGetServiceAsync(web3, addressRegDeployment);
            Log($"{contractName} address is: {addressRegistryService.ContractHandler.ContractAddress}");

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
