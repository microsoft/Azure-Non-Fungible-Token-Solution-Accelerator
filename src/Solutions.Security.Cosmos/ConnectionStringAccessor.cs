// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using Azure.Identity;
using Microsoft.Solutions.HTTP;
using System.Threading.Tasks;
using Azure.Core;

namespace Microsoft.Solutions.CosmosDB.Security.ManagedIdentity
{
    public class ConnectionStringAccessor
    {
        public ConnectionStringAccessor()
        {
        }

        public static ConnectionStringAccessor Create(string SubscriptionId, string ResourceGroupName, string CosmosDBAccountName)
        {
            return new ConnectionStringAccessor(SubscriptionId, ResourceGroupName, CosmosDBAccountName);
        }

        private string listConnectionStringAPIUrl = string.Empty;
        private DefaultAzureCredential azureCredential = null;

        private ConnectionStringAccessor(string SubscriptionId, string ResourceGroupName, string CosmosDBAccountName)
        {
            // Setup the List Connection Strings API to get the Azure Cosmos DB keys.
            listConnectionStringAPIUrl = @$"https://management.azure.com/subscriptions/{SubscriptionId}/resourceGroups/{ResourceGroupName}/providers/Microsoft.DocumentDB/databaseAccounts/{CosmosDBAccountName}/listConnectionStrings?api-version=2021-04-15";
        }


        /// <summary>
        /// Retrive Cosmos DB Connection string with ManagedIdentity
        /// </summary>
        /// <param name="ManagedIdentityClientId">The ManagedIdentity should have Cosmos DB Account Reader Role</param>
        /// <returns>CosmosConnectionStrings (ReadWrite / ReadOnly)</returns>
        public async Task<CosmosConnectionStrings> GetConnectionStringsAsync(string ManagedIdentityClientId)
        {
            azureCredential = new DefaultAzureCredential(
                                    new DefaultAzureCredentialOptions()
                                    {
                                        ManagedIdentityClientId = ManagedIdentityClientId,
                                        Retry =
                                                {
                                                    Delay = TimeSpan.FromSeconds(2),
                                                    MaxDelay = TimeSpan.FromSeconds(16),
                                                    MaxRetries =5,
                                                    Mode = RetryMode.Exponential
                                                }
                                    });

            var AccessToken = await azureCredential.GetTokenAsync(
                                         new Azure.Core.TokenRequestContext(
                                             new[] { "https://management.azure.com//.default" }));

            var result = await HttpClient.PostJsonAsync<ConnectionStrings>(listConnectionStringAPIUrl, string.Empty, AccessToken.Token);

            return new CosmosConnectionStrings()
            {
                PrimaryReadWriteKey = result.connectionStrings[0].connectionString,
                SecondaryReadWriteKey = result.connectionStrings[1].connectionString,
                PrimaryReadOnlyKey = result.connectionStrings[2].connectionString,
                SecondaryReadOnlyKey = result.connectionStrings[3].connectionString
            };
        }
    }

    public class CosmosConnectionStrings
    {
        public string PrimaryReadWriteKey { get; set; }
        public string SecondaryReadWriteKey { get; set; }
        public string PrimaryReadOnlyKey { get; set; }
        public string SecondaryReadOnlyKey { get; set; }
    }

    internal class ConnectionStrings
    {
        public ConnectionString[] connectionStrings { get; set; }
    }

    internal class ConnectionString
    {
        public string connectionString { get; set; }
        public string description { get; set; }
    }
}
