// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Microsoft.Solutions.CosmosDB.SQL;
using Microsoft.TokenService.TransactionIndexer.Model;

namespace Microsoft.TokenService.TransactionIndexer
{
    public class TransactionIndexerService : SQLEntityCollectionBase<TransactionInfo>
    {
        public TransactionIndexerService(IConfiguration Config, CosmosConnectionStrings cosmosConnectionStrings) : base(cosmosConnectionStrings.PrimaryReadWriteKey, Config["App:TokenTransactionCollection"]) { }

        public async Task LogTransactionAsync(TransactionInfo transactionInfo)
        {
            await this.EntityCollection.AddAsync(transactionInfo);
        }
    }
}
