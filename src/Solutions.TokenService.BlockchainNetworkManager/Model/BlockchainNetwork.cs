// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using Microsoft.Solutions.CosmosDB;
using Newtonsoft.Json;

namespace Microsoft.TokenService.BlockchainNetworkManager.Model
{
    public class BlockchainNetwork : CosmosDBEntityBase
    {
        [JsonProperty("Id")]
        public Guid Id { get ; set ; }
        public BlockchainNetwork()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }
        public string Name { get; set; }
        public string BlockchainPlatformName { get; set; }
        public string  BlockchainPlatformType { get; set; }
        public ChainId ChainId { get; set; }
        public string BlockchainNode { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public enum ChainId
    {
        MainNet = 1,
        Morden = 2,
        Ropsten = 3,
        Rinkeby = 4,
        QBSorABS = 10,
        RootstockMainNet = 30,
        RootstockTestNet = 0x1F,
        Kovan = 42,
        ClassicMainNet = 61,
        ClassicTestNet = 62,
        Private = 1337
    }
}
