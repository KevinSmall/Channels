using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;
using static Channels.Contracts.ContractEnums;

namespace Channels.Contracts.SignatureChecker.ContractDefinition
{
    public partial class PoItem
    {
        [Parameter("uint256", "poNumber", 1)]
        public new BigInteger PoNumber { get; set; }

        [Parameter("uint8", "poItemNumber", 2)]
        public new uint PoItemNumber { get; set; }

        [Parameter("bytes32", "soNumber", 3)]
        public new string SoNumber { get; set; }

        [Parameter("bytes32", "soItemNumber", 4)]
        public new string SoItemNumber { get; set; }

        [Parameter("bytes32", "productId", 5)]
        public new string ProductId { get; set; }

        [Parameter("uint256", "quantity", 6)]
        public new BigInteger Quantity { get; set; }

        [Parameter("bytes32", "unit", 7)]
        public new string Unit { get; set; }

        [Parameter("bytes32", "quantitySymbol", 8)]
        public new string QuantitySymbol { get; set; }

        [Parameter("address", "quantityAddress", 9)]
        public new string QuantityAddress { get; set; }

        [Parameter("uint256", "currencyValue", 10)]
        public new BigInteger CurrencyValue { get; set; }
        
        [Parameter("uint8", "status", 11)]
        public new PoItemStatus Status { get; set; }

        [Parameter("uint256", "goodsIssuedDate", 12)]
        public new BigInteger GoodsIssuedDate { get; set; }

        [Parameter("uint256", "goodsReceivedDate", 13)]
        public new BigInteger GoodsReceivedDate { get; set; }

        [Parameter("uint256", "plannedEscrowReleaseDate", 14)]
        public new BigInteger PlannedEscrowReleaseDate { get; set; }

        [Parameter("uint256", "actualEscrowReleaseDate", 15)]
        public new BigInteger ActualEscrowReleaseDate { get; set; }

        [Parameter("bool", "isEscrowReleased", 16)]
        public new bool IsEscrowReleased { get; set; }

        [Parameter("uint8", "cancelStatus", 17)]
        public new PoItemCancelStatus CancelStatus { get; set; }
    }
}
