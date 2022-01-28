// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.PartyManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.PartyManager
{
    public interface IPartyManager
    {
        Task<IEnumerable<Party>> GetAllParty();
        Task<Party> GetParty(Guid Id);
        Task<Party> RegisterParty(string PartyName, string Description);
        Task UnRegisterParty(Guid Id);
    }
}