// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.BlockchainNetworkManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Solutions.CosmosDB.SQL;
using Microsoft.Solutions.CosmosDB;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Microsoft.Extensions.Configuration;

namespace Microsoft.TokenService.BlockchainNetworkManager
{
    public class BlockchainNetworks : SQLEntityCollectionBase<BlockchainNetwork> , IBlockchainNetworkManager
    {
        public BlockchainNetworks(string DataConnectionString, string CollectionName) : base(DataConnectionString, CollectionName)
        {
        }

        public BlockchainNetworks(IConfiguration Config, CosmosConnectionStrings cosmosConnectionStrings) : base(cosmosConnectionStrings.PrimaryReadWriteKey, Config["App:ManagementCollection"])
        {
        }

        public async Task<BlockchainNetwork> RegisterBlockchainNetwork(string BlockchainNetworkName, string TransactionNodeURL, ChainId ChainId, string Description)
        {
            var blockchainNetwork = new BlockchainNetwork()
            {
                Name = BlockchainNetworkName,
                BlockchainNode = TransactionNodeURL,
                BlockchainPlatformName = "Quorum",
                ChainId = ChainId,
                BlockchainPlatformType = "Ethereum",
                Description = Description
            };

            await this.EntityCollection.AddAsync(blockchainNetwork);
            return blockchainNetwork;
        }

        public async Task<IEnumerable<BlockchainNetwork>> GetAllBlockchainNetworks()
        {
            return await this.EntityCollection.GetAllAsync();
        }

        public async Task<BlockchainNetwork> GetBlockchainNetwork(Guid Id)
        {
            return await this.EntityCollection.FindAsync(new GenericSpecification<BlockchainNetwork>(x => x.Id == Id));
        }

        public async Task<BlockchainNetwork> GetBlockchainNetwork(String blockchainName)
        {
            return await this.EntityCollection.FindAsync(new GenericSpecification<BlockchainNetwork>(x => x.Name == blockchainName));
        }

        public async Task UnRegisterBlockchainNetwork(Guid Id)
        {
            var blockchainNetwork = await GetBlockchainNetwork(Id);
            await this.EntityCollection.DeleteAsync(blockchainNetwork, blockchainNetwork.__partitionkey);
        }
    }
}
