using Channels.Contracts;
using Channels.Contracts.ReceiverPays;
using Channels.Contracts.ReceiverPays.ContractDefinition;
using Channels.Contracts.SignatureChecker;
using Channels.Contracts.SignatureChecker.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using static Channels.Contracts.ContractEnums;
using Nethereum.ABI;
using FluentAssertions;

namespace Channels.Micropayments.Tests
{
    public class PaymentTests
    {
        private readonly ITestOutputHelper _output;

        // Nethereum testchain
        private const string _blockchainUrl = "http://testchain.nethereum.com:8545";
        private const string _privateKey = "0x7580e7fb49df1c861f0050fae31c2224c6aba908e116b8da44ee8cd927b990b0";

        // Rinkeby
        //private const string _blockchainUrl = "https://rinkeby.infura.io/v3/7238211010344719ad14a89db874158c";
        //private const string _privateKey = "0x517311d936323b28ca55379280d3b307d354f35ae35b214c6349e9828e809adc";            

        public PaymentTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void SigningTestPo()
        {
            var web3 = new Web3(new Account(_privateKey), _blockchainUrl);

            var po2 = new Po() { 
                BuyerAddress = "0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610",
                PoNumber = 1,
                ApproverAddress = "0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610",
                ReceiverAddress = "0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610",
                BuyerWalletAddress = "0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610",
                CurrencyAddress = "0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610",
                CurrencySymbol = "DAI",
                PoType = PoType.Cash,
                SellerId = "Nethereum.eShop",
                PoCreateDate = 100,
                PoItemCount = 2,
                PoItems = new List<PoItem>()
                {
                    new PoItem()
                    {
                        PoNumber = 1,
                        PoItemNumber = 10,
                        SoNumber = "so1",
                        SoItemNumber = "100",
                        ProductId = "gtin1111",
                        Quantity = 1,
                        Unit = "EA",
                        QuantitySymbol = "NA",
                        QuantityAddress = "0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610",
                        CurrencyValue = 11,
                        Status = PoItemStatus.Created,
                        GoodsIssuedDate = 100,
                        GoodsReceivedDate = 0,
                        PlannedEscrowReleaseDate = 100,
                        ActualEscrowReleaseDate = 110,
                        IsEscrowReleased = false,
                        CancelStatus = PoItemCancelStatus.Initial
                    }
                  }
                };
            var encoded = new ABIEncode().GetABIEncoded(new ABIValue(new TupleType(), po2));
            var hashEncoded = new ABIEncode().GetSha3ABIEncoded(new ABIValue(new TupleType(), po2));
            var signed = new EthereumMessageSigner().Sign(hashEncoded, _privateKey);
            var account2 = new Account(_privateKey);

            // Prepare new PO
            var po = CreatePo(1, "123", 1);

            // SIGN. Doesnt need web3, done offline.
            // In:  Po, Signer Private Key
            // Out: Signature (65 bytes)
            Log("--- SIGN with Nethereum ---");
            var abiEncoder = new ABIEncode();
            var encodedValue = abiEncoder.GetABIEncoded(po);
            var signer = new EthereumMessageSigner();
            var signature = signer.HashAndSign(encodedValue, _privateKey);
            Log($"signature: {signature} is for message: [byte array not shown here yet]");
            Log("");


            // RECOVER with Solidity
            // In: Po, Signature
            // Out: Signer Address
            Log("--- RECOVER with Solidity ---");
            Log($"Deploying contract SignatureChecker...");
            var signatureCheckerDeployment = new SignatureCheckerDeployment();
            var signatureCheckerService = await SignatureCheckerService.DeployContractAndGetServiceAsync(web3, signatureCheckerDeployment);

            // See Solidity code Channels\Channels.Contracts\Contracts\SignatureChecker.sol:
            var bytesForSignature = signature.HexToByteArray();
            var signerAddressRecoveredSolidity = await signatureCheckerService.GetSignerAddressFromPoAndSignatureQueryAsync(
               po, bytesForSignature);
            Log($"Actual Signer Address Recovered using Solidity: {signerAddressRecoveredSolidity}");
            Log($"Expected Signer address: {web3.TransactionManager.Account.Address}");  // since web3 created using same private key as was used to sign

            signerAddressRecoveredSolidity.Should().Be(web3.TransactionManager.Account.Address);
        }

        [Fact]
        public async void SigningTestSimple()
        {
            // The message itself, this is what is being signed
            var msg = "test message 1234567890";
            //var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var web3 = new Web3(new Account(_privateKey), _blockchainUrl);

            // SIGN. Doesnt need web3, done offline.
            // In:  Message, Signer Private Key
            // Out: Signature (65 bytes)
            Log("--- SIGN with Nethereum ---");
            var signer = new EthereumMessageSigner();
            var signature = signer.HashAndSign(msg, _privateKey);
            Log($"signature: {signature} is for message: {msg}");
            Log("");


            // RECOVER with Nethereum
            // In: Message, Signature
            // Out: Signer Address
            Log("--- RECOVER with Nethereum ---");
            var signerAddressRecoveredNethereum = signer.HashAndEcRecover(msg, signature);
            Log($"Actual Signer address using C# HashAndEcRecover: {signerAddressRecoveredNethereum}");
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

            signerAddressRecoveredSolidity.Should().Be(signerAddressRecoveredNethereum.ToLowerInvariant());

            // See Solidity code Channels\Channels.Contracts\Contracts\SignatureChecker.sol:
            //   // this recreates the message that was signed on the client
            //   bytes32 messageAsClient = prefixed(keccak256(abi.encodePacked(message)));
        }

        //[Fact]
        //public async void DeployReceiverPays()
        //{
        //    // Web3
        //    var web3 = new Web3(new Account(_privateKey), _blockchainUrl);

        //    Log($"Deploying contract ReceiverPays...");
        //    var receiverPaysDeployment = new ReceiverPaysDeployment();
        //    var receiverPaysService = await ReceiverPaysService.DeployContractAndGetServiceAsync(web3, receiverPaysDeployment);
        //    Log($"ReceiverPays address is: {receiverPaysService.ContractHandler.ContractAddress}");

        //}

        private void Log(string message)
        {
            _output.WriteLine(message);
        }

        public static Po CreatePo(uint poNumber, string approverAddress, uint quoteId)
        {
            return new Po()
            {
                PoNumber = poNumber,
                BuyerAddress = "0x37ed4f49ec2c7bdcce8631b1a7b54ed5d4aa9610",
                ReceiverAddress = "0x36ed4f49ec2c7bdcce8631b1a7b54ed5d4aa9610",
                BuyerWalletAddress = "0x39ed4f49ec2c7bdcce8631b1a7b54ed5d4aa9610",
                CurrencySymbol = "DAI",
                CurrencyAddress = "0x41ed4f49ec2c7bdcce8631b1a7b54ed5d4aa9610",
                QuoteId = quoteId,
                QuoteExpiryDate = 1,
                ApproverAddress = approverAddress,
                PoType = PoType.Cash,
                SellerId = "Nethereum.eShop",
                PoCreateDate = 100,
                PoItemCount = 2,
                PoItems = new List<PoItem>()
                {
                    new PoItem()
                    {
                        PoNumber = poNumber,
                        PoItemNumber = 10,
                        SoNumber = "so1",
                        SoItemNumber = "100",
                        ProductId = "gtin1111",
                        Quantity = 1,
                        Unit = "EA",
                        QuantitySymbol = "NA",
                        QuantityAddress = "0x40ed4f49ec2c7bdcce8631b1a7b54ed5d4aa9610",
                        CurrencyValue = 11,
                        Status = PoItemStatus.Created,
                        GoodsIssuedDate = 100,
                        GoodsReceivedDate = 0,
                        PlannedEscrowReleaseDate = 100,
                        ActualEscrowReleaseDate = 110,
                        IsEscrowReleased = false,
                        CancelStatus = PoItemCancelStatus.Initial
                    },
                    new PoItem()
                    {
                        PoNumber = poNumber,
                        PoItemNumber = 20,
                        SoNumber = "so1",
                        SoItemNumber = "200",
                        ProductId = "gtin2222",
                        Quantity = 2,
                        Unit = "EA",
                        QuantitySymbol = "NA",
                        QuantityAddress = "0x42ed4f49ec2c7bdcce8631b1a7b54ed5d4aa9610",
                        CurrencyValue = 22,
                        Status = PoItemStatus.Created,
                        GoodsIssuedDate = 200,
                        GoodsReceivedDate = 0,
                        PlannedEscrowReleaseDate = 200,
                        ActualEscrowReleaseDate = 210,
                        IsEscrowReleased = false,
                        CancelStatus = PoItemCancelStatus.Initial
                    }
                }
            };
        }
    }
}
