// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.BlockchainNetworkManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.BlockchainNetworkManager
{
    public interface IBlockchainNetworkManager
    {
        Task<BlockchainNetwork> GetBlockchainNetwork(Guid Id);
        Task<BlockchainNetwork> GetBlockchainNetwork(string blockchainName);
        Task<IEnumerable<BlockchainNetwork>> GetAllBlockchainNetworks();
        Task<BlockchainNetwork> RegisterBlockchainNetwork(string BlockchainNetworkName, string TransactionNodeURL, ChainId ChainId, string Description);
        Task UnRegisterBlockchainNetwork(Guid Id);
    }
}