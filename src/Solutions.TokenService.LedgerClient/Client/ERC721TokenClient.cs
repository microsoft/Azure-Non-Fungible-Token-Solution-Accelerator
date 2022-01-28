// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Microsoft.TokenService.KeyManagement;
using Microsoft.TokenService.LedgerClient.Model;
using Microsoft.TokenService.LedgerClient.ERC721Token.ContractDefinition;
using Microsoft.TokenService.UserManager;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Threading.Tasks;

namespace Microsoft.TokenService.LedgerClient.Client
{
    public class ERC721TokenClient : LedgerClientBase, IERC721TokenClient
    {
        private Users users;
        private MapperConfiguration mapConfig;
        public ERC721TokenClient(IConfiguration config, CosmosConnectionStrings cosmosConnectionStrings, AzureKeyVaultKeyClient akvKeyClient) : base(akvKeyClient)
        {
            users = new Users(config, cosmosConnectionStrings, akvKeyClient);

            mapConfig =
                new MapperConfiguration(cfg =>
                                                cfg.CreateMap<TransactionReceipt,
                                                              Model.TransactionReciept>()
                                                              .ForMember(dest => dest.TransactionHash, opt => opt.MapFrom(src => src.TransactionHash))
                                                              .ForMember(dest => dest.TransactionIndex, opt => opt.MapFrom(src => src.TransactionIndex.ToLong()))
                                                              .ForMember(dest => dest.BlockHash, opt => opt.MapFrom(src => src.BlockHash))
                                                              .ForMember(dest => dest.BlockNumber, opt => opt.MapFrom(src => src.BlockNumber.ToLong()))
                                                              .ForMember(dest => dest.CumulativeGasUsed, opt => opt.MapFrom(src => src.CumulativeGasUsed.ToLong()))
                                                              .ForMember(dest => dest.GasUsed, opt => opt.MapFrom(src => src.GasUsed.ToLong()))
                                                              .ForMember(dest => dest.ContractAddress, opt => opt.MapFrom(src => src.ContractAddress))
                                                              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToLong()))
                                                              );
        }

        private async Task<Web3> getWeb3WithUserID(string UserID)
        {
            var user = await users.GetUser(Guid.Parse(UserID));
            return await GetWeb3(user.Id.ToString(),
                                        user.BlockchainNetwork.BlockchainNode);
        }

        private async Task<ERC721Token.ERC721Token> getTokenService(string ContractAddress, string UserID)
        {
            return new ERC721Token.ERC721Token(await getWeb3WithUserID(UserID),
                                                ContractAddress);
        }

        public async Task<TransactionReciept> DeployNewToken(string TokenOwnerId, string TokenName, string TokenSymbol)
        {
            var receipt = await ERC721Token.ERC721Token.DeployContractAndWaitForReceiptAsync(
                                                await getWeb3WithUserID(TokenOwnerId),
                                                new NFmbtgTokenDeployment()
                                                {
                                                    Name = TokenName,
                                                    Symbol = TokenSymbol
                                                });
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);

        }

        public async Task<string> Name(string ContractAddress, string CallerId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.NameQueryAsync();
        }

        public async Task<string> Symbol(string ContractAddress, string CallerId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.SymbolQueryAsync();
        }

        public async Task<string> OwnerOf(string ContractAddress, string CallerId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.OwnerOfQueryAsync(TokenId);
        }

        public async Task<long> BalanceOf(string ContractAddress, string CallerId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            var retBalance = await tokenSvc.BalanceOfQueryAsync(
                                                    (await users.GetUser(Guid.Parse(CallerId))).PublicAddress);
            return (long)retBalance;
        }

        public async Task<string> TokenURI(string ContractAddress, string CallerId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.TokenURIQueryAsync(TokenId);
        }

        public async Task<bool> IsApprovedForAll(string ContractAddress, string CallerId, string TokenOwner, string OperatorId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);

            return await tokenSvc.IsApprovedForAllQueryAsync(
                (await users.GetUser(Guid.Parse(TokenOwner))).PublicAddress,
                (await users.GetUser(Guid.Parse(OperatorId))).PublicAddress);

        }

        public async Task<string> GetApproved(string ContractAddress, string CallerId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            return await tokenSvc.GetApprovedQueryAsync(
                                                    TokenId);
        }

        public async Task<TransactionReciept> Approve(string ContractAddress, string CallerId, string ApproverId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.ApproveRequestAndWaitForReceiptAsync(
                                                                        (await users.GetUser(Guid.Parse(ApproverId))).PublicAddress,
                                                                        TokenId);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


        public async Task<TransactionReciept> SetApprovalForAll(string ContractAddress, string CallerId, string ApproveeId, bool Approved)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.SetApprovalForAllRequestAndWaitForReceiptAsync(
                                                                        (await users.GetUser(Guid.Parse(ApproveeId))).PublicAddress,
                                                                        Approved);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }

        public async Task<TransactionReciept> TransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.TransferFromRequestAndWaitForReceiptAsync(
                                                                        (await users.GetUser(Guid.Parse(SenderId))).PublicAddress,
                                                                        (await users.GetUser(Guid.Parse(RecipientId))).PublicAddress,
                                                                        TokenId);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }

        public async Task<TransactionReciept> Transfer(string ContractAddress, string SenderId, string RecipientId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, SenderId);
            var receipt = await tokenSvc.TransferRequestAndWaitForReceiptAsync(
                                                                        (await users.GetUser(Guid.Parse(RecipientId))).PublicAddress,
                                                                        TokenId);
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


        public async Task<TransactionReciept> SafeTransferFrom(string ContractAddress, string CallerId, string SenderId, string RecipientId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.SafeTransferFromRequestAndWaitForReceiptAsync(
                                                                        (await users.GetUser(Guid.Parse(SenderId))).PublicAddress,
                                                                        (await users.GetUser(Guid.Parse(RecipientId))).PublicAddress,
                                                                        TokenId);
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);

        }

        public async Task<TransactionReciept> Burn(string ContractAddress, string CallerId, long TokenId)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, CallerId);
            var receipt = await tokenSvc.BurnRequestAndWaitForReceiptAsync(TokenId);

            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


        public async Task<TransactionReciept> MintToken(string ContractAddress, string TokenMinterId, string TokenMinteeId, long TokenId, string TokenURI)
        {
            ERC721Token.ERC721Token tokenSvc = await getTokenService(ContractAddress, TokenMinterId);
            var receipt = await tokenSvc.MintWithTokenURIRequestAndWaitForReceiptAsync(
                                                                    (await users.GetUser(Guid.Parse(TokenMinteeId))).PublicAddress,
                                                                    TokenId,
                                                                    TokenURI);
            var mapper = mapConfig.CreateMapper();
            return mapper.Map<TransactionReceipt, Model.TransactionReciept>(receipt);
        }


    }
}
