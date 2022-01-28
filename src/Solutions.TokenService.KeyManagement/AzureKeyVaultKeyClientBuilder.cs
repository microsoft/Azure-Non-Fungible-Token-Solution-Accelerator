// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Azure.Core;
using Azure.Security.KeyVault.Keys;
using System;

namespace Microsoft.TokenService.KeyManagement
{
    internal class AzureKeyVaultKeyClientBuilder
    {
        private KeyClient keyClient;
        private TokenCredential _credential;

        private AzureKeyVaultKeyClientBuilder(TokenCredential credential)
        {
            _credential = credential;
        }

        public static AzureKeyVaultKeyClientBuilder Create(TokenCredential credential)
        {
            return new AzureKeyVaultKeyClientBuilder(credential);
        }

        public AzureKeyVaultKeyClientBuilder SetKeyVaultEndpoint(string KeyValutUrl)
        {
            KeyClientOptions options = new KeyClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            keyClient = new KeyClient(new Uri(KeyValutUrl), _credential, options);

            return this;
        }

        public KeyClient Build() => keyClient;


    }
}
