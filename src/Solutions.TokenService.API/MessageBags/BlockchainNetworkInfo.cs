// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

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
        public string Description { get; set; }
    }
}
