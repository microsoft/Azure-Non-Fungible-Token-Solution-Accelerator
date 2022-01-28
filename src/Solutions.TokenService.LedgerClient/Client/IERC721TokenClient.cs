// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.LedgerClient.Model;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;

namespace Microsoft.TokenService.LedgerClient.Client
{
    public interface IERC721TokenClient
    {
        Task<TransactionReciept> Approve(string ContractAddress, string CallerId, string ApproverId, long TokenId);
        Task<long> BalanceOf(string ContractAddress, string CallerId);
        Task<TransactionReciept> Burn(string ContractAddress, string CallerId, long TokenId);
        Task<TransactionReciept> DeployNewToken(string TokenOwnerId, string TokenName, string TokenSymbol);
        Task<string> GetApproved(string ContractAddress, string CallerId, long TokenId);
        Task<bool> IsApprovedForAll(string ContractAddress, string CallerId, string TokenOwner, string OperatorId);
        Task<TransactionReciept> MintToken(string ContractAddress, string TokenMinterId, string TokenMinteeId, long TokenId, string TokenURI);
        Task<string> Name(string ContractAddress, string CallerId);
        Task<string> OwnerOf(string ContractAddress, string CallerId, long TokenId);
        Task<TransactionReciept> SafeTransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId);
        Task<TransactionReciept> SetApprovalForAll(string ContractAddress, string CallerId, string ApproveeId, bool Approved);
        Task<string> Symbol(string ContractAddress, string CallerId);
        Task<string> TokenURI(string ContractAddress, string CallerId, long TokenId);
        Task<TransactionReciept> Transfer(string ContractAddress, string SenderId, string RecipientId, long TokenId);
        Task<TransactionReciept> TransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId);
    }
}