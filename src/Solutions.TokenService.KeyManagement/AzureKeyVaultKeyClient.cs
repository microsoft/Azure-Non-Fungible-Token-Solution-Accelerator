// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Nethereum.RPC.Accounts;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using System;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Keys;
using Azure.Identity;
using Azure.Core;

namespace Microsoft.TokenService.KeyManagement
{
    public class AzureKeyVaultKeyClient
    {
        private KeyClient kvClient;
        private TokenCredential clientSecretCredential;

        public AzureKeyVaultKeyClient(string KeyVaultBaseURL, string ClientID, string ClientSecret, string TenantID)
        {
            clientSecretCredential = new ClientSecretCredential(TenantID, ClientID, ClientSecret);
            kvClient = AzureKeyVaultKeyClientBuilder.Create(clientSecretCredential)
                                                    .SetKeyVaultEndpoint(KeyVaultBaseURL)
                                                    .Build();

        }

        public AzureKeyVaultKeyClient()
        {
        }


        public static AzureKeyVaultKeyClient Create()
        {
            return new AzureKeyVaultKeyClient();
        }

        public AzureKeyVaultKeyClient Build(string KeyVaultBaseURL, string SubscriptionId, string ResourceGroupName, string ManagedIdentityClientId)
        {

            var azureCredential = new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions()
                    {
                        ManagedIdentityClientId = ManagedIdentityClientId,
                    }
                );

            clientSecretCredential = azureCredential;

            kvClient = new KeyClient(new Uri(KeyVaultBaseURL), azureCredential, new KeyClientOptions()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries =5,
                    Mode = RetryMode.Exponential
                }
            });

            return this;
        }


        public async Task<KeyVaultKey> ReadKey(string keyIdentifier)
        {
            return await kvClient.GetKeyAsync(keyIdentifier);
        }

        public async Task<DeleteKeyOperation> DeleteKey(string KeyIdentifier)
        {
            return await kvClient.StartDeleteKeyAsync(KeyIdentifier);
        }

        public async Task<KeyVaultKey> SetKey(string keyIdentifier)
        {
            return await kvClient.CreateEcKeyAsync(new CreateEcKeyOptions(keyIdentifier) { CurveName = "P-256K" });
        }

        public async Task<string> GetPublicKey(string keyIdentifier)
        {

            var kvKey = (await ReadKey(keyIdentifier)).Key;
            var xLen = kvKey.X.Length;
            var yLen = kvKey.Y.Length;
            var publicKey = new byte[1 + xLen + yLen];
            publicKey[0] = 0x04;
            var offset = 1;
            Buffer.BlockCopy(kvKey.X, 0, publicKey, offset, xLen);
            offset = offset + xLen;
            Buffer.BlockCopy(kvKey.Y, 0, publicKey, offset, yLen);
            //return publicKey;
            return new EthECKey(publicKey, false).GetPublicAddress();
        }

        public async Task<IAccount> SetUpExternalAccountFromKeyVaultByKey(string keyIdentifier)
        {
            var signer = new AzureKeyVaultExternalSigner(kvClient, keyIdentifier, clientSecretCredential);
            var externalAccount = new ExternalAccount(signer, (int)Nethereum.Signer.Chain.Private);
            await externalAccount.InitialiseAsync();
            return externalAccount;
        }
    }

}
