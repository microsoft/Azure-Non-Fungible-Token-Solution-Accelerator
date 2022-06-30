// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.KeyManagement;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Threading.Tasks;

namespace Microsoft.TokenService.LedgerClient.Client
{
    public class LedgerClientBase
    {
        protected Web3 web3;
        private AzureKeyVaultKeyClient kvClient;

        protected LedgerClientBase(AzureKeyVaultKeyClient akvClient) => kvClient = akvClient;
        
        protected async Task<Web3> GetWeb3(string KeyIdentifier, string RPCEndpoint, int ChainId)
        {
            //prepare user's private key
            var _account = await kvClient.SetUpExternalAccountFromKeyVaultByKey(KeyIdentifier, ChainId);
            var rpcClient = new RpcClient(baseUrl: new Uri(RPCEndpoint));
            ((ExternalAccount)_account).InitialiseDefaultTransactionManager(rpcClient);
            var web3 = new Web3(account: _account, client: rpcClient);

            //just in case public ethereum, should provides some configuration
            web3.TransactionManager.DefaultGasPrice = 0;
            web3.TransactionManager.UseLegacyAsDefault = true;
            
            return web3;
        }

    }
}
