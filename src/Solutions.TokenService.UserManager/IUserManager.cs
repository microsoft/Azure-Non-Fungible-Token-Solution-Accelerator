// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.TokenService.UserManager.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.TokenService.UserManager
{
    public interface IUserManager
    {
        Task<User> GetUser(Guid Id);
        Task<User> GetUserByPublicAddress(string PublicAddress);
        Task<User> RegisterUser(string Name, string Description, Guid PartyID, Guid BlockchainNetworkID);
        Task UnRegistUser(Guid Id);
        Task<IEnumerable<User>> GetAllUsers();
    }
}