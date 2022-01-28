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
        public string BlockchainNode { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
