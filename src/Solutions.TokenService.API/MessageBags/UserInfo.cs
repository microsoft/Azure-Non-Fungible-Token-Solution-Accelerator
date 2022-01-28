// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Microsoft.TokenService.API.MessageBags
{
    public class UserInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PartyID { get; set; }
        public Guid BlockchainNetworkID { get; set; }
    }
}
