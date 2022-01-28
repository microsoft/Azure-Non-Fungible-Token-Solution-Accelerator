// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using Microsoft.Solutions.CosmosDB;
using Newtonsoft.Json;

namespace Microsoft.TokenService.PartyManager.Model
{
    public class Party : CosmosDBEntityBase
    {
        [JsonProperty("Id")]
        public Guid Id { get ; set ; }

        public Party()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }

        public string PartyName { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
    }
}
