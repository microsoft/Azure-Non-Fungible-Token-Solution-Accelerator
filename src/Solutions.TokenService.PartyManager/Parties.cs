// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.PartyManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Solutions.CosmosDB.SQL;
using Microsoft.Solutions.CosmosDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;

namespace Microsoft.TokenService.PartyManager
{
    public class Parties : SQLEntityCollectionBase<Party>, IPartyManager
    {
        public Parties(string DataConnectionString, string CollectionName) : base(DataConnectionString, CollectionName)
        {
        }

        public Parties(IConfiguration Config, CosmosConnectionStrings cosmosConnectionStrings) : base(cosmosConnectionStrings.PrimaryReadWriteKey, Config["App:ManagementCollection"])
        {
        }

        public async Task<Party> RegisterParty(string PartyName, string Description)
        {
            var party = new Party()
            {
                PartyName = PartyName,
                Description = Description
            };

            await this.EntityCollection.AddAsync(party);
            return party;
        }

        public async Task<IEnumerable<Party>> GetAllParty()
        {
            return await this.EntityCollection.GetAllAsync();
        }

        public async Task<Party> GetParty(Guid Id)
        {
            return await this.EntityCollection.FindAsync(new GenericSpecification<Party>(x => x.Id == Id));
        }


        public async Task UnRegisterParty(Guid Id)
        {
            var party = await GetParty(Id);
            await this.EntityCollection.DeleteAsync(party, party.__partitionkey);
        }
    }
}
