// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Nethereum.Signer;
using Nethereum.Signer.Crypto;
using System;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Core;

namespace Microsoft.TokenService.KeyManagement
{
    /// <summary>
    /// This code is originated from Nethereum.Signer.AzureKeyVault.AzureKeyVaultExternalSigner
    /// https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Signer.AzureKeyVault/AzureKeyVaultExternalSigner.cs
    /// 
    /// Due to KeyVault SDK version incomparability issue, modified to support new version
    /// </summary>
    internal class AzureKeyVaultExternalSigner : EthExternalSignerBase
    {
        public override bool Supported1559 => true;

        public override ExternalSignerTransactionFormat ExternalSignerTransactionFormat { get; protected set; } = ExternalSignerTransactionFormat.Hash;
        public override bool CalculatesV { get; protected set; } = false;

        public KeyClient KeyVaultClient { get; private set; }
        public string KeyIdentifier { get; }
        public TokenCredential ClientSecretCredential { get; private set; }

        public AzureKeyVaultExternalSigner(KeyClient keyClient, string keyIdentifier, TokenCredential clientSecretCredential)
        {
            KeyVaultClient = keyClient;
            KeyIdentifier = keyIdentifier;
            ClientSecretCredential = clientSecretCredential;
        }


        public override Task SignAsync(LegacyTransactionChainId transaction)
        {
            return SignHashTransactionAsync(transaction);
        }

        public override Task SignAsync(LegacyTransaction transaction)
        {
            return SignHashTransactionAsync(transaction);
        }

        public override Task SignAsync(Transaction1559 transaction)
        {
            return SignHashTransactionAsync(transaction);
        }

        protected override async Task<byte[]> GetPublicKeyAsync()
        {
            var keyVaultKey = (await KeyVaultClient.GetKeyAsync(KeyIdentifier)).Value.Key;
            var xLen = keyVaultKey.X.Length;
            var yLen = keyVaultKey.Y.Length;
            var publicKey = new byte[1 + xLen + yLen];
            publicKey[0] = 0x04;
            var offset = 1;
            Buffer.BlockCopy(keyVaultKey.X, 0, publicKey, offset, xLen);
            offset = offset + xLen;
            Buffer.BlockCopy(keyVaultKey.Y, 0, publicKey, offset, yLen);
            
            return publicKey;
        }

        protected override async Task<ECDSASignature> SignExternallyAsync(byte[] bytes)
        {
            CryptographyClient ecCryptoClient = new CryptographyClient(new Uri(string.Concat(KeyVaultClient.VaultUri.ToString(), "keys/", KeyIdentifier)), ClientSecretCredential, new CryptographyClientOptions()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries =5,
                    Mode = RetryMode.Exponential
                }
            });

            var signResult = await ecCryptoClient.SignAsync(SignatureAlgorithm.ES256K, bytes);
            var signature = signResult.Signature;
            return ECDSASignatureFactory.FromComponents(signature).MakeCanonical();
        }
    }
}
