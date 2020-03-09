pragma solidity ^0.6.1;
pragma experimental ABIEncoderV2;

import "./IPoTypes.sol";

contract SignatureChecker
{
    address owner = msg.sender;

    mapping(uint256 => bool) usedNonces;

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