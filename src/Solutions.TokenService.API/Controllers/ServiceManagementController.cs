// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TokenService.API.MessageBags;
using Microsoft.TokenService.BlockchainNetworkManager;
using Microsoft.TokenService.BlockchainNetworkManager.Model;
using Microsoft.TokenService.PartyManager;
using Microsoft.TokenService.PartyManager.Model;
using Microsoft.TokenService.UserManager;
using Microsoft.TokenService.UserManager.Model;

namespace Microsoft.TokenService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceManagementController : ControllerBase
    {
        private IBlockchainNetworkManager blockchainNetworkManager;
        private IPartyManager partyManager;
        private IUserManager userManager;

        public ServiceManagementController(IBlockchainNetworkManager blockchains, IPartyManager parties, IUserManager users)
            => (blockchainNetworkManager, partyManager, userManager) = (blockchains, parties, users );


        [HttpGet]
        [ActionName("GetBlockchainNetworkById")]
        [Route("BlockchainNetworks/BlockchainNetwork/{Id}")]
        public async Task<BlockchainNetwork> GetBlockchainNetwork(Guid Id)
        {
            return await blockchainNetworkManager.GetBlockchainNetwork(Id);
        }

        [HttpGet]
        [ActionName("GetAllBlockchainNetworks")]
        [Route("BlockchainNetworks")]
        public async Task<IEnumerable<BlockchainNetwork>> GetAllBlockchainNetwork()
        {
            return await blockchainNetworkManager.GetAllBlockchainNetworks();
        }


        [HttpPost]
        [ActionName("RegisterBlockchainNetwork")]
        [Route("BlockchainNetworks/BlockchainNetwork")]
        public async Task<BlockchainNetwork> RegisterBlockchainNetwork([FromBody] MessageBags.BlockchainNetworkInfo BlockchainNetworkInfo)
        {


            return await blockchainNetworkManager.RegisterBlockchainNetwork(BlockchainNetworkInfo.Name,
                                                                            BlockchainNetworkInfo.NodeURL,
                                                                            BlockchainNetworkInfo.ChainId,
                                                                            BlockchainNetworkInfo.Description);
        }


        [HttpDelete]
        [ActionName("UnRegisterBlockchainNetwork")]
        [Route("BlockchainNetworks/BlockchainNetwork/{Id}")]
        public async Task<bool> UnregisterBlockchainNetwork(Guid Id)
        {
            try
            {
                await blockchainNetworkManager.UnRegisterBlockchainNetwork(Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        [ActionName("GetPartyById")]
        [Route("Parties/Party/{Id}")]
        public async Task<Party> GetParty(Guid Id)
        {
            return await partyManager.GetParty(Id);
        }

        [HttpGet]
        [ActionName("GetAllParties")]
        [Route("Parties")]
        public async Task<IEnumerable<Party>> GetParty()
        {
            return await partyManager.GetAllParty();
        }

        [HttpPost]
        [ActionName("RegisterParty")]
        [Route("Parties/Party")]
        public async Task<Party> RegisterParty([FromBody] PartyInfo PartyInfo)
        {
            return await partyManager.RegisterParty(PartyInfo.PartyName, PartyInfo.Description);
        }

        [HttpDelete]
        [ActionName("UnRegisterParty")]
        [Route("Parties/Party/{Id}")]
        public async Task<bool> UnregisterParty(Guid Id)
        {
            try
            {
                await partyManager.UnRegisterParty(Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        [ActionName("GetUserById")]
        [Route("Users/User/{Id}")]
        public async Task<User> GetUser(Guid Id)
        {
            return await userManager.GetUser(Id);
        }

        [HttpGet]
        [ActionName("GetUserByPublicAddress")]
        [Route("Users/User")]
        
        public async Task<User> GetUserByPublicAddress([FromQuery]string PublicAddress)
        {
            return await userManager.GetUserByPublicAddress(PublicAddress);
        }



        [HttpPost]
        [ActionName("RegisterUser")]
        [Route("Users/User")]
        public async Task<User> RegisterUser([FromBody] UserInfo UserInfo)
        {
            return await userManager.RegisterUser(UserInfo.Name,
                                                    UserInfo.Description,
                                                    UserInfo.PartyID,
                                                    UserInfo.BlockchainNetworkID);
        }

        [HttpGet]
        [ActionName("GetAllUsers")]
        [Route("Users")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await userManager.GetAllUsers();
        }


        [HttpDelete]
        [ActionName("UnRegisterUser")]
        [Route("Users/User/{Id}")]
        public async Task<bool> UnRegisterUser(Guid Id)
        {
            try
            {
                await userManager.UnRegistUser(Id);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
