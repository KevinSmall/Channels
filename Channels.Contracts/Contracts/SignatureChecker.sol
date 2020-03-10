pragma solidity ^0.6.1;
pragma experimental ABIEncoderV2;

import "./IPoTypes.sol";

contract SignatureChecker
{
    address owner = msg.sender;

    mapping(uint256 => bool) usedNonces;
    
    //IPoTypes.Po po;
    
    
    constructor() public payable {}
    
    function getSignerAddressFromPoAndSignature(IPoTypes.Po memory po, bytes memory signature) public pure returns (address)
    {
        // this recreates the message that was signed on the client
        //IPoTypes.PoItem memory poi = po.poItems[0];
        bytes32 messageAsClient1 = prefixed(keccak256(abi.encode(po)));
        //bytes32 messageAsClient2 = prefixed(keccak256(abi.encode(poi)));
        //bytes32 messageAsClient3 = prefixed(keccak256(abi.encodePacked(poi)));

        // this recovers the signer's address
        return recoverSigner(messageAsClient1, signature);
    }
    
    function getAbiEncode() public pure returns (bytes memory encoded) {
    
        IPoTypes.Po memory po; 
        IPoTypes.PoItem[] memory poItems = new IPoTypes.PoItem[](1);
        IPoTypes.PoItem memory poItem;
        
        poItem.poNumber = 1;
        poItem.poItemNumber = 10;
        poItem.soNumber = "so1";
        poItem.soItemNumber = "100";
        poItem.productId = "gtin1111";
        poItem.quantity = 1;
        poItem.unit = "EA";
        poItem.quantitySymbol = "NA";
        poItem.quantityAddress = 0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610;
        poItem.currencyValue = 11;
        poItem.status = IPoTypes.PoItemStatus.Created;
        poItem.goodsIssuedDate = 100;
        poItem.goodsReceivedDate = 0;
        poItem.plannedEscrowReleaseDate = 100;
        poItem.actualEscrowReleaseDate = 110;
        poItem.isEscrowReleased = false;
        poItem.cancelStatus = IPoTypes.PoItemCancelStatus.Initial;
        poItems[0] = poItem;
        
        po.poNumber = 1;
        po.buyerAddress = 0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610;
        po.approverAddress = 0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610;
        po.receiverAddress = 0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610;
        po.buyerWalletAddress = 0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610;
        po.currencyAddress = 0x40eD4f49EC2C7BDCCe8631B1A7b54ed5d4Aa9610;
        po.currencySymbol = "DAI";
        po.poType = IPoTypes.PoType.Cash;
        po.sellerId = "Nethereum.eShop";
        po.poCreateDate = 100;
        po.poItemCount = 2;
        po.poItems = poItems;
        
        
        return abi.encode(po);
    }
    
    function getAbiEncodeHash() public pure returns (bytes32 encoded) {
        return keccak256(getAbiEncode());
    }
    
    //0x865277490402dfe5a138c7ad3697eec1fe2849251e2c06fcfecde48b5ef7e6da76e635a0f72f3cf1dc0abb0eedd61c7106de907ce4142c5363a8c12eaf743e711b
    //0x94618601FE6cb8912b274E5a00453949A57f8C1e
    //0x94618601FE6cb8912b274E5a00453949A57f8C1e
    function getSigner(bytes memory signature) public pure returns(address recovered) {
 
        bytes32 messageAsClient = prefixed(getAbiEncodeHash());
        return recoverSigner(messageAsClient, signature);
    }

    function getSignerAddressFromMessageAndSignature(string memory message, bytes memory signature) public pure returns (address)
    {
        // this recreates the message that was signed on the client
        bytes32 messageAsClient = prefixed(keccak256(abi.encodePacked(message)));

        // this recovers the signer's address
        return recoverSigner(messageAsClient, signature);
    }

    // signature methods
    function splitSignature(bytes memory sig) public pure returns (uint8 v, bytes32 r, bytes32 s)
    {
        require(sig.length == 65);
        assembly {
            // first 32 bytes, after the length prefix.
            r := mload(add(sig, 32))
            // second 32 bytes.
            s := mload(add(sig, 64))
            // final byte (first byte of the next 32 bytes).
            v := byte(0, mload(add(sig, 96)))
        }
        return (v, r, s);
    }

    function recoverSigner(bytes32 message, bytes memory sig) public pure returns (address)
    {
        (uint8 v, bytes32 r, bytes32 s) = splitSignature(sig);
        return ecrecover(message, v, r, s);
    }

    // builds a prefixed hash to mimic the behavior of eth_sign.
    function prefixed(bytes32 hash) internal pure returns (bytes32)
    {
        return keccak256(abi.encodePacked("\x19Ethereum Signed Message:\n32", hash));
    }
}