// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.BlockchainNetworkManager.Model;
using Microsoft.TokenService.PartyManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Solutions.CosmosDB;
using Newtonsoft.Json;

namespace Microsoft.TokenService.UserManager.Model
{
    public class User : CosmosDBEntityBase
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        public User()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public string PublicAddress { get; set; }
        public string Description { get; set; }
        public Party Party { get; set; }
        public BlockchainNetwork BlockchainNetwork { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
