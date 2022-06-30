// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.BlockchainNetworkManager.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.TokenService.API.MessageBags
{
    public class BlockchainNetworkInfo
    {
        public string Name { get; set; }
        public string NodeURL { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ChainId ChainId { get; set; }
        public string Description { get; set; }
    }
}
