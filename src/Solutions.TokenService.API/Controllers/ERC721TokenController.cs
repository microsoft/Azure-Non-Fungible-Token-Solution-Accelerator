// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TokenService.LedgerClient.Client;
using Microsoft.TokenService.LedgerClient.Model;
using Microsoft.TokenService.TransactionIndexer;
using Microsoft.TokenService.TransactionIndexer.Model;
using Microsoft.Extensions.Configuration;

namespace Microsoft.TokenService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ERC721TokenController : ControllerBase
    {
        IERC721TokenClient ERC721TokenClient;
        TransactionIndexerService TxIndexer;
        IMapper Mapper;
        IConfiguration Config;

        public ERC721TokenController(IERC721TokenClient TokenClient, TransactionIndexerService txIndexer, IMapper mapper, IConfiguration configuration)
            => (ERC721TokenClient, TxIndexer, Mapper,Config) = (TokenClient, txIndexer, mapper, configuration);

        [HttpPost]
        [Route("Approve")]
        public async Task<TransactionReciept> Approve(string ContractAddress, string CallerId, string ApproverId, long TokenId)
        {
            var result = await ERC721TokenClient.Approve(ContractAddress, CallerId, ApproverId, TokenId);
            result.ContractAddress = ContractAddress;

            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
               await LogTransaction(TransactionType.Approve, result, CallerId, ApproverId, "", new TokenMeta() { TokenId = TokenId});
            return result;
        }


        [HttpPost]
        [Route("BalanceOf")]
        public async Task<long> BalanceOf(string ContractAddress, string CallerId) => await ERC721TokenClient.BalanceOf(ContractAddress, CallerId);

        [HttpPost]
        [Route("Burn")]
        public async Task<TransactionReciept> Burn(string ContractAddress, string CallerId, long TokenId)
        { 
            var result = await ERC721TokenClient.Burn(ContractAddress, CallerId, TokenId);
            result.ContractAddress = ContractAddress;

            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
                await LogTransaction(TransactionType.TokenBurnt, result, CallerId, "", "", new TokenMeta() { TokenId = TokenId });
            return result;
        }

        [HttpPost]
        [Route("DeployNewToken")]
        public async Task<TransactionReciept> DeployNewToken(string TokenOwnerId, string TokenName, string TokenSymbol)
        {
            var result = await ERC721TokenClient.DeployNewToken(TokenOwnerId, TokenName, TokenSymbol);
            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
                await LogTransaction(TransactionType.TokenDeployed, result, TokenOwnerId, TokenOwnerId, "", null);
            return result;
        }


        [HttpPost]
        [Route("GetApproved")]
        public async Task<string> GetApproved(string ContractAddress, string CallerId, long TokenId)
            => await ERC721TokenClient.GetApproved(ContractAddress, CallerId, TokenId);

        [HttpPost]
        [Route("IsApprovedForAll")]
        public async Task<bool> IsApprovedForAll(string ContractAddress, string CallerId, string TokenOwner, string OperatorId)
            => await ERC721TokenClient.IsApprovedForAll(ContractAddress, CallerId, TokenOwner, OperatorId);

        [HttpPost]
        [Route("MintToken")]
        public async Task<TransactionReciept> MintToken(string ContractAddress, string TokenMinterId, string TokenMinteeId, long TokenId, string TokenURI)
        {
            var result = await ERC721TokenClient.MintToken(ContractAddress, TokenMinterId, TokenMinteeId, TokenId, TokenURI);
            result.ContractAddress = ContractAddress;

            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
                await LogTransaction(TransactionType.TokenMinted, result, TokenMinterId, TokenMinterId, TokenMinteeId, new TokenMeta() { TokenId = TokenId, TokenUrl = TokenURI });

            return result;

        }

        [HttpPost]
        [Route("Name")]
        public async Task<string> Name(string ContractAddress, string CallerId) => await ERC721TokenClient.Name(ContractAddress, CallerId);


        [HttpPost]
        [Route("OwnerOf")]
        public async Task<string> OwnerOf(string ContractAddress, string CallerId, long TokenId) => await ERC721TokenClient.OwnerOf(ContractAddress, CallerId, TokenId);


        [HttpPost]
        [Route("SafeTransferFrom")]
        public async Task<TransactionReciept> SafeTransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
        {
            var result = await ERC721TokenClient.SafeTransferFrom(ContractAddress, CallerId, SenderId, RecipientId, TokenId);
            result.ContractAddress = ContractAddress;

            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
                await LogTransaction(TransactionType.TokenTransferred, result, CallerId, SenderId, RecipientId, new TokenMeta() { TokenId = TokenId });
            return result;
        }


        [HttpPost]
        [Route("SetApprovalForAll")]
        public async Task<TransactionReciept> SetApprovalForAll(string ContractAddress, string CallerId, string ApproveeId, bool Approved)
        {
            var result = await ERC721TokenClient.SetApprovalForAll(ContractAddress, CallerId, ApproveeId, Approved);
            result.ContractAddress = ContractAddress;

            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
                await LogTransaction(TransactionType.SetApprovalForAll, result, CallerId, CallerId, ApproveeId, null);
            return result;
        }

        [HttpPost]
        [Route("Symbol")]
        public async Task<string> Symbol(string ContractAddress, string CallerId) => await ERC721TokenClient.Symbol(ContractAddress, CallerId);

        [HttpPost]
        [Route("TokenURI")]
        public async Task<string> TokenURI(string ContractAddress, string CallerId, long TokenId) => await ERC721TokenClient.TokenURI(ContractAddress, CallerId, TokenId);

        [HttpPost]
        [Route("Transfer")]
        public async Task<TransactionReciept> Transfer(string ContractAddress, string SenderId, string RecipientId, long TokenId)
        {
            var result = await ERC721TokenClient.Transfer(ContractAddress, SenderId, RecipientId, TokenId);
            result.ContractAddress = ContractAddress;

            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
                await LogTransaction(TransactionType.TokenTransferred, result, SenderId, SenderId, RecipientId, new TokenMeta() { TokenId = TokenId });
            return result;
        }

        [HttpPost]
        [Route("TransferFrom")]
        public async Task<TransactionReciept> TransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
        { 
            var result = await ERC721TokenClient.TransferFrom(ContractAddress, CallerId, SenderId, RecipientId, TokenId);
            result.ContractAddress = ContractAddress;

            if (bool.Parse(Config["App:LogTransaction"].ToLower()))
                await LogTransaction(TransactionType.TokenTransferred, result, CallerId, SenderId, RecipientId, new TokenMeta() { TokenId = TokenId });
            return result;
        }


        private async Task LogTransaction(TransactionType txType, Microsoft.TokenService.LedgerClient.Model.TransactionReciept txReceipt, string Caller, string Sender, string Recipient, TokenMeta Meta)
        {
            await TxIndexer.LogTransactionAsync(new TransactionInfo()
            {
                TransactionType = txType,
                Caller = Caller,
                Sender = Sender,
                Receipient = Recipient,
                MetaInfo = Meta,
                TransactionReceiptInfo = Mapper.Map<TransactionIndexer.Model.TransactionRecieptInfo>(txReceipt)
            });
        }

    }
}