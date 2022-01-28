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
import "./modified-openzeppelin/token/ERC721/ERC721Basic.sol";
import "./modified-openzeppelin/token/ERC721/ERC721DelegableBasic.sol";
import "./modified-openzeppelin/token/ERC721/ERC721Burnable.sol";
import "./modified-openzeppelin/token/ERC721/ERC721DelegableBurn.sol";
import "./modified-openzeppelin/token/ERC721/ERC721MetadataMintable.sol";
import "./modified-openzeppelin/token/ERC721/ERC721Transferable.sol";
import "./modified-openzeppelin/token/ERC721/ERC721DelegableTransfer.sol";

contract FooToken is ERC721Basic, ERC721DelegableBasic, ERC721Burnable, ERC721DelegableBurn, ERC721MetadataMintable, ERC721Transferable, ERC721DelegableTransfer
{
    address private _owner;

    constructor(string memory name, string memory symbol) ERC721Basic(name, symbol)
    public {
        _owner = msg.sender;
    }
    
}