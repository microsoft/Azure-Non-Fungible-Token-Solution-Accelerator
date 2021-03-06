// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

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

namespace Microsoft.TokenService.LedgerClient.ERC721Token.ContractDefinition
{


    public partial class NFmbtgTokenDeployment : NFmbtgTokenDeploymentBase
    {
        public NFmbtgTokenDeployment() : base(BYTECODE) { }
        public NFmbtgTokenDeployment(string byteCode) : base(byteCode) { }
    }

    public class NFmbtgTokenDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040523480156200001157600080fd5b506040516200155138038062001551833981018060405260408110156200003757600080fd5b8101908080516401000000008111156200005057600080fd5b820160208101848111156200006457600080fd5b81516401000000008111828201871017156200007f57600080fd5b505092919060200180516401000000008111156200009c57600080fd5b82016020810184811115620000b057600080fd5b8151640100000000811182820187101715620000cb57600080fd5b50909350849250839150829050816200010d7f01ffc9a700000000000000000000000000000000000000000000000000000000640100000000620001db810204565b8151620001229060039060208501906200032d565b508051620001389060049060208401906200032d565b506200017191507f80ac58cd000000000000000000000000000000000000000000000000000000009050640100000000620001db810204565b620001a57f5b5e139f00000000000000000000000000000000000000000000000000000000640100000000620001db810204565b5050620001c13362000248640100000000026401000000009004565b505060098054600160a060020a03191633179055620003d2565b7fffffffff0000000000000000000000000000000000000000000000000000000080821614156200020b57600080fd5b7fffffffff00000000000000000000000000000000000000000000000000000000166000908152602081905260409020805460ff19166001179055565b62000263600882640100000000620010f56200029a82021704565b604051600160a060020a038216907f6ae172837ea30b801fbfcdd4108aa1d5bf8ff775444fd70256b44e6bf3dfc3f690600090a250565b600160a060020a0381161515620002b057600080fd5b620002c58282640100000000620002f5810204565b15620002d057600080fd5b600160a060020a0316600090815260209190915260409020805460ff19166001179055565b6000600160a060020a03821615156200030d57600080fd5b50600160a060020a03166000908152602091909152604090205460ff1690565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106200037057805160ff1916838001178555620003a0565b82800160010185558215620003a0579182015b82811115620003a057825182559160200191906001019062000383565b50620003ae929150620003b2565b5090565b620003cf91905b80821115620003ae5760008155600101620003b9565b90565b61116f80620003e26000396000f3fe608060405234801561001057600080fd5b506004361061011d576000357c0100000000000000000000000000000000000000000000000000000000900480636352211e116100b4578063a9059cbb11610083578063a9059cbb14610425578063b88d4fde14610451578063c87b56dd14610517578063e985e9c5146105345761011d565b80636352211e1461039a57806370a08231146103b757806395d89b41146103ef578063a22cb465146103f75761011d565b806323b872dd116100f057806323b872dd1461025657806342842e0e1461028c57806342966c68146102c257806350bb4e7f146102df5761011d565b806301ffc9a71461012257806306fdde0314610172578063081812fc146101ef578063095ea7b314610228575b600080fd5b61015e6004803603602081101561013857600080fd5b50357bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916610562565b604080519115158252519081900360200190f35b61017a610596565b6040805160208082528351818301528351919283929083019185019080838360005b838110156101b457818101518382015260200161019c565b50505050905090810190601f1680156101e15780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b61020c6004803603602081101561020557600080fd5b503561062d565b60408051600160a060020a039092168252519081900360200190f35b6102546004803603604081101561023e57600080fd5b50600160a060020a03813516906020013561063e565b005b6102546004803603606081101561026c57600080fd5b50600160a060020a0381358116916020810135909116906040013561064c565b610254600480360360608110156102a257600080fd5b50600160a060020a03813581169160208101359091169060400135610671565b610254600480360360208110156102d857600080fd5b503561068d565b61015e600480360360608110156102f557600080fd5b600160a060020a038235169160208101359181019060608101604082013564010000000081111561032557600080fd5b82018360208201111561033757600080fd5b8035906020019184600183028401116401000000008311171561035957600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295506106ae945050505050565b61020c600480360360208110156103b057600080fd5b50356106e2565b6103dd600480360360208110156103cd57600080fd5b5035600160a060020a0316610714565b60408051918252519081900360200190f35b61017a61074c565b6102546004803603604081101561040d57600080fd5b50600160a060020a03813516906020013515156107ad565b61015e6004803603604081101561043b57600080fd5b50600160a060020a0381351690602001356107b7565b6102546004803603608081101561046757600080fd5b600160a060020a038235811692602081013590911691604082013591908101906080810160608201356401000000008111156104a257600080fd5b8201836020820111156104b457600080fd5b803590602001918460018302840111640100000000831117156104d657600080fd5b91908080601f0160208091040260200160405190810160405280939291908181526020018383808284376000920191909152509295506107c4945050505050565b61017a6004803603602081101561052d57600080fd5b50356107ec565b61015e6004803603604081101561054a57600080fd5b50600160a060020a03813581169160200135166108a1565b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff191660009081526020819052604090205460ff1690565b60038054604080516020601f60026000196101006001881615020190951694909404938401819004810282018101909252828152606093909290918301828280156106225780601f106105f757610100808354040283529160200191610622565b820191906000526020600020905b81548152906001019060200180831161060557829003601f168201915b505050505090505b90565b6000610638826108b4565b92915050565b61064882826108e6565b5050565b610656338261099c565b151561066157600080fd5b61066c8383836109e4565b505050565b61066c83838360206040519081016040528060008152506107c4565b610697338261099c565b15156106a257600080fd5b6106ab816109f8565b50565b60006106b933610a0a565b15156106c457600080fd5b6106ce8484610a1d565b6106d88383610acd565b5060019392505050565b60006106ed82610b00565b15156106f857600080fd5b50600090815260016020526040902054600160a060020a031690565b6000600160a060020a038216151561072b57600080fd5b600160a060020a038216600090815260026020526040902061063890610b1d565b60048054604080516020601f60026000196101006001881615020190951694909404938401819004810282018101909252828152606093909290918301828280156106225780601f106105f757610100808354040283529160200191610622565b6106488282610b21565b60006106383384846109e4565b6107cf84848461064c565b6107db84848484610ba5565b15156107e657600080fd5b50505050565b60606107f782610b00565b151561080257600080fd5b60008281526005602090815260409182902080548351601f6002600019610100600186161502019093169290920491820184900484028101840190945280845290918301828280156108955780601f1061086a57610100808354040283529160200191610895565b820191906000526020600020905b81548152906001019060200180831161087857829003601f168201915b50505050509050919050565b60006108ad8383610d22565b9392505050565b60006108bf82610b00565b15156108ca57600080fd5b50600090815260066020526040902054600160a060020a031690565b60006108f1826106e2565b9050600160a060020a03838116908216141561090c57600080fd5b33600160a060020a038216148061092857506109288133610d22565b151561093357600080fd5b600082815260066020526040808220805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0387811691821790925591518593918516917f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b92591a4505050565b60006109a88383610d50565b806109cc575082600160a060020a03166109c1836108b4565b600160a060020a0316145b806108ad57506108ad6109de836106e2565b84610d22565b6109ed81610d77565b61066c838383610dbf565b6106ab610a04826106e2565b82610ea5565b600061063860088363ffffffff610eb816565b600160a060020a0382161515610a3257600080fd5b610a3b81610b00565b15610a4557600080fd5b6000818152600160209081526040808320805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a038716908117909155835260029091529020610a9190610eef565b6040518190600160a060020a038416906000907fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef908290a45050565b610ad682610b00565b1515610ae157600080fd5b6000828152600560209081526040909120825161066c9284019061101d565b600090815260016020526040902054600160a060020a0316151590565b5490565b600160a060020a038216331415610b3757600080fd5b336000818152600760209081526040808320600160a060020a03871680855290835292819020805460ff1916861515908117909155815190815290519293927f17307eab39ab6107e8899845ad3d59bd9653f200f220920489ca2b5937696c31929181900390910190a35050565b6000610bb984600160a060020a0316610ef8565b1515610bc757506001610d1a565b6040517f150b7a020000000000000000000000000000000000000000000000000000000081523360048201818152600160a060020a03888116602485015260448401879052608060648501908152865160848601528651600095928a169463150b7a029490938c938b938b939260a4019060208501908083838e5b83811015610c5a578181015183820152602001610c42565b50505050905090810190601f168015610c875780820380516001836020036101000a031916815260200191505b5095505050505050602060405180830381600087803b158015610ca957600080fd5b505af1158015610cbd573d6000803e3d6000fd5b505050506040513d6020811015610cd357600080fd5b50517bffffffffffffffffffffffffffffffffffffffffffffffffffffffff19167f150b7a0200000000000000000000000000000000000000000000000000000000149150505b949350505050565b600160a060020a03918216600090815260076020908152604080832093909416825291909152205460ff1690565b6000610d5b826106e2565b600160a060020a031683600160a060020a031614905092915050565b600081815260066020526040902054600160a060020a0316156106ab576000908152600660205260409020805473ffffffffffffffffffffffffffffffffffffffff19169055565b82600160a060020a0316610dd2826106e2565b600160a060020a031614610de557600080fd5b600160a060020a0382161515610dfa57600080fd5b600160a060020a0383166000908152600260205260409020610e1b90610f00565b600160a060020a0382166000908152600260205260409020610e3c90610eef565b600081815260016020526040808220805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0386811691821790925591518493918716917fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef91a4505050565b610eae81610d77565b6106488282610f17565b6000600160a060020a0382161515610ecf57600080fd5b50600160a060020a03166000908152602091909152604090205460ff1690565b80546001019055565b6000903b1190565b8054610f1390600163ffffffff610f5f16565b9055565b610f218282610f74565b60008181526005602052604090205460026000196101006001841615020190911604156106485760008181526005602052604081206106489161109b565b600082821115610f6e57600080fd5b50900390565b81600160a060020a0316610f87826106e2565b600160a060020a031614610f9a57600080fd5b600160a060020a0382166000908152600260205260409020610fbb90610f00565b600081815260016020526040808220805473ffffffffffffffffffffffffffffffffffffffff1916905551829190600160a060020a038516907fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef908390a45050565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061105e57805160ff191683800117855561108b565b8280016001018555821561108b579182015b8281111561108b578251825591602001919060010190611070565b506110979291506110db565b5090565b50805460018160011615610100020316600290046000825580601f106110c157506106ab565b601f0160209004906000526020600020908101906106ab91905b61062a91905b8082111561109757600081556001016110e1565b600160a060020a038116151561110a57600080fd5b6111148282610eb8565b1561111e57600080fd5b600160a060020a0316600090815260209190915260409020805460ff1916600117905556fea165627a7a72305820492a3ec8185e451de14e5c6cfd535c2ee0cf1ba0af3d3758de09dfa755f5b2580029";
        public NFmbtgTokenDeploymentBase() : base(BYTECODE) { }
        public NFmbtgTokenDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("string", "name", 1)]
        public virtual string Name { get; set; }
        [Parameter("string", "symbol", 2)]
        public virtual string Symbol { get; set; }
    }

    public partial class SupportsInterfaceFunction : SupportsInterfaceFunctionBase { }

    [Function("supportsInterface", "bool")]
    public class SupportsInterfaceFunctionBase : FunctionMessage
    {
        [Parameter("bytes4", "interfaceId", 1)]
        public virtual byte[] InterfaceId { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class GetApprovedFunction : GetApprovedFunctionBase { }

    [Function("getApproved", "address")]
    public class GetApprovedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class BurnFunction : BurnFunctionBase { }

    [Function("burn")]
    public class BurnFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class MintWithTokenURIFunction : MintWithTokenURIFunctionBase { }

    [Function("mintWithTokenURI", "bool")]
    public class MintWithTokenURIFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("string", "tokenURI", 3)]
        public virtual string TokenURI { get; set; }
    }

    public partial class OwnerOfFunction : OwnerOfFunctionBase { }

    [Function("ownerOf", "address")]
    public class OwnerOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class SetApprovalForAllFunction : SetApprovalForAllFunctionBase { }

    [Function("setApprovalForAll")]
    public class SetApprovalForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("bool", "approved", 2)]
        public virtual bool Approved { get; set; }
    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
    }

    //public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

    //[Function("safeTransferFrom")]
    //public class SafeTransferFromFunctionBase : FunctionMessage
    //{
    //    [Parameter("address", "from", 1)]
    //    public virtual string From { get; set; }
    //    [Parameter("address", "to", 2)]
    //    public virtual string To { get; set; }
    //    [Parameter("uint256", "tokenId", 3)]
    //    public virtual BigInteger TokenId { get; set; }
    //    [Parameter("bytes", "data", 4)]
    //    public virtual byte[] Data { get; set; }
    //}

    public partial class TokenURIFunction : TokenURIFunctionBase { }

    [Function("tokenURI", "string")]
    public class TokenURIFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class IsApprovedForAllFunction : IsApprovedForAllFunctionBase { }

    [Function("isApprovedForAll", "bool")]
    public class IsApprovedForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "operator", 2)]
        public virtual string Operator { get; set; }
    }

    public partial class MinterAddedEventDTO : MinterAddedEventDTOBase { }

    [Event("MinterAdded")]
    public class MinterAddedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class MinterRemovedEventDTO : MinterRemovedEventDTOBase { }

    [Event("MinterRemoved")]
    public class MinterRemovedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3, true )]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "approved", 2, true )]
        public virtual string Approved { get; set; }
        [Parameter("uint256", "tokenId", 3, true )]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase { }

    [Event("ApprovalForAll")]
    public class ApprovalForAllEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "operator", 2, true )]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 3, false )]
        public virtual bool Approved { get; set; }
    }

    public partial class SupportsInterfaceOutputDTO : SupportsInterfaceOutputDTOBase { }

    [FunctionOutput]
    public class SupportsInterfaceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetApprovedOutputDTO : GetApprovedOutputDTOBase { }

    [FunctionOutput]
    public class GetApprovedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }











    public partial class OwnerOfOutputDTO : OwnerOfOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }







    public partial class TokenURIOutputDTO : TokenURIOutputDTOBase { }

    [FunctionOutput]
    public class TokenURIOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class IsApprovedForAllOutputDTO : IsApprovedForAllOutputDTOBase { }

    [FunctionOutput]
    public class IsApprovedForAllOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }
}
