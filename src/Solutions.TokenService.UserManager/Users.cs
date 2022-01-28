// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.TokenService.BlockchainNetworkManager;
using Microsoft.TokenService.KeyManagement;
using Microsoft.TokenService.PartyManager;
using Microsoft.TokenService.UserManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Solutions.CosmosDB.SQL;
using Microsoft.Solutions.CosmosDB;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;

namespace Microsoft.TokenService.UserManager
{
    public class Users : SQLEntityCollectionBase<User>, IUserManager
    {
        Parties _parties;
        BlockchainNetworks _blockchainNetworks;
        AzureKeyVaultKeyClient _akvKeyClient;

        public Users(IConfiguration Config, CosmosConnectionStrings cosmosConnectionStrings, AzureKeyVaultKeyClient akvKeyClient) : base(cosmosConnectionStrings.PrimaryReadWriteKey, Config["App:ManagementCollection"])
        {
            _parties = new Parties(cosmosConnectionStrings.PrimaryReadWriteKey, Config["App:ManagementCollection"]);
            _blockchainNetworks = new BlockchainNetworks(cosmosConnectionStrings.PrimaryReadWriteKey, Config["App:ManagementCollection"]);
            _akvKeyClient = akvKeyClient;
        }

        public async Task<User> RegisterUser(string Name, string Description, Guid PartyID, Guid BlockchainNetworkID)
        {
            var newUser = new User()
            {
                Name = Name,
                PublicAddress = "",
                Description = Description,
                Party = await _parties.GetParty(PartyID),
                BlockchainNetwork = await _blockchainNetworks.GetBlockchainNetwork(BlockchainNetworkID)
            };

            var userIdentifier = newUser.Id.ToString();
            var result = await _akvKeyClient.SetKey(userIdentifier);
            var publicKey = await _akvKeyClient.GetPublicKey(userIdentifier);

            newUser.PublicAddress = publicKey;

            await this.EntityCollection.AddAsync(newUser);
            return newUser;
        }

        public async Task<User> GetUser(Guid Id)
        {
            return await this.EntityCollection.FindAsync(new GenericSpecification<User>(x => x.Id == Id));
        }

        public async Task<User> GetUserByPublicAddress(string PublicAddress)
        {
            return await this.EntityCollection.FindAsync(new GenericSpecification<User>(x => x.PublicAddress == PublicAddress));
        }

        public async Task UnRegistUser(Guid Id)
        {
            _ = await _akvKeyClient.DeleteKey(Id.ToString());
            var user = await GetUser(Id);
            await this.EntityCollection.DeleteAsync(user, user.__partitionkey);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await this.EntityCollection.GetAllAsync();
        }
    }
}
