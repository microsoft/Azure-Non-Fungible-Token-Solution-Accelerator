//----------------------------------------------------------------------------------------------
// The MIT License (MIT)
//
// Copyright (c) 2016-2019 zOS Global Limited
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------------------------------------------------------

pragma solidity ^0.5.2;

import "./NonFungibleMetadata.sol";
import "./IERC721.sol";
import "./IERC721Metadata.sol";
import "./IERC721Receiver.sol";
import "../../introspection/ERC165.sol";

/**
 * @title ERC721 Non-Fungible Token Standard basic implementation
 * @dev see https://eips.ethereum.org/EIPS/eip-721
 */
contract ERC721Basic is ERC165, IERC721, IERC721Metadata, NonFungibleMetadata {

    bytes4 internal constant _INTERFACE_ID_ERC721 = 0x80ac58cd;
    /*
     *
     * 0x80ac58cd ===
     *     bytes4(keccak256('balanceOf(address)')) ^
     *     bytes4(keccak256('ownerOf(uint256)')) ^
     *     bytes4(keccak256('approve(address,uint256)')) ^
     *     bytes4(keccak256('getApproved(uint256)')) ^
     *     bytes4(keccak256('setApprovalForAll(address,bool)')) ^
     *     bytes4(keccak256('isApprovedForAll(address,address)')) ^
     *     bytes4(keccak256('transferFrom(address,address,uint256)')) ^
     *     bytes4(keccak256('safeTransferFrom(address,address,uint256)')) ^
     *     bytes4(keccak256('safeTransferFrom(address,address,uint256,bytes)'))
     */

    bytes4 private constant _INTERFACE_ID_ERC721_METADATA = 0x5b5e139f;
    /*
     * 0x5b5e139f ===
     *     bytes4(keccak256('name()')) ^
     *     bytes4(keccak256('symbol()')) ^
     *     bytes4(keccak256('tokenURI(uint256)'))
     */

    /**
     * @dev Constructor function
     */
    constructor (string memory name, string memory symbol) ERC165() NonFungibleMetadata(name, symbol) public {
        // register the supported interfaces to conform to ERC721 via ERC165
        _registerInterface(_INTERFACE_ID_ERC721);
        
        // register the supported interfaces to conform to ERC721 via ERC165
        _registerInterface(_INTERFACE_ID_ERC721_METADATA);
    }

    /// @notice UnimplementedInterface: Revert as this token contract is non-delegable
    function approve(address , uint256 ) public {
        revert("");
    }

    /// @notice UnimplementedInterface: Revert as this token contract is non-delegable
    function getApproved(uint256 ) public view returns (address) {
        revert("");
    }

    /// @notice UnimplementedInterface: Revert as this token contract is non-delegable
    function setApprovalForAll(address , bool ) public {
        revert("");
    }

    /// @notice UnimplementedInterface: Revert as this token contract is non-delegable
    function isApprovedForAll(address , address ) public view returns (bool) {
        revert("");
    }

    /// @notice UnimplementedInterface: Revert as this token contract is non-delegable
    function transferFrom(address , address , uint256 ) public {
        revert("");
    }

    /// @notice UnimplementedInterface: Revert as this token contract is non-delegable
    function safeTransferFrom(address , address , uint256 ) public {
        revert("");
    }

    /// @notice UnimplementedInterface: Revert as this token contract is non-delegable
    function safeTransferFrom(address , address , uint256 , bytes memory ) public {
        revert("");
    }
}
