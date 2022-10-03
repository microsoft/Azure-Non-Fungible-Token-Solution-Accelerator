// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Solutions.NFT.ConsoleApp
{
    internal class NFTSampleApp : IHostedService
    {
        private IConfiguration _config;
        private HttpClient _httpClient;

        private ServiceClient _serviceClient;
        private NFTSampleConfig _appconfig;

        public NFTSampleApp(IConfiguration Config, HttpClient HttpClient)
        {
            (_config, _httpClient) = (Config, HttpClient);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ProcessStart();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task ProcessStart()
        {
            _serviceClient = new ServiceClient(_config["App:NFTServiceURL"], _httpClient);
            await SetupEnvironment();
            await ShowTokenTransactions();
        }

        /// <summary>
        /// Set up Environmental Configuration
        ///     1. Register Blockchain Node Information
        ///     2. Register Consorium Party Information
        ///     3. Register Users
        ///         3.1 Token Admin User
        ///         3.2 Token Users
        /// </summary>
        /// <returns></returns>
        private async Task SetupEnvironment()
        {
            Console.WriteLine("** Set up Environment for NFT Service Example ****\n\n");
            Console.WriteLine("Make sure the appsetting.json file before start the Appliation\n");
            Console.WriteLine("********************************************************");
            Console.WriteLine("**  Step 1. Register Blockchain Network Information   **");
            Console.WriteLine("********************************************************");

            _appconfig = new NFTSampleConfig();

            //Register Blockchain Network
            var bc_result = await _serviceClient.RegisterBlockchainNetworkAsync(new BlockchainNetworkInfo()
            {
                Name = "Test Blockchain Network",
                Description = "This is Sample Application",
                ChainId = ChainId.Private,
                NodeURL = _config["App:BlockchainTxEndpointURL"]
            });

            _appconfig.BlockchainNetwork = bc_result;

            Console.WriteLine($"Blockchain Network has been registered => Registered Id is {bc_result.Id} with NodeURL for {bc_result.BlockchainNode}\n\n");


            Console.WriteLine("********************************************************");
            Console.WriteLine("***  Step 2. Register Consortium Party Information   ***");
            Console.WriteLine("********************************************************");

            //Register Party for Users
            var party_result = await _serviceClient.RegisterPartyAsync(new PartyInfo()
            {
                PartyName = "Test Party",
                Description = "This is Sample Party"
            });

            _appconfig.Party = party_result;

            Console.WriteLine($"Party has been registered => Registered Id is {party_result.Id} and Party Name is {party_result.PartyName}\n\n");


            Console.WriteLine("********************************************************");
            Console.WriteLine("***       Step 3. Register Users Information         ***");
            Console.WriteLine("********************************************************");


            //Register New Users - TokenAdmin / User A / User B with Party and Blockchain information
            var tokenAdmin_result = await _serviceClient.RegisterUserAsync(new UserInfo()
            {
                Name = "Token Admin",
                BlockchainNetworkID = bc_result.Id,
                PartyID = party_result.Id,
                Description = "This is Token Admin User"
            });

            _appconfig.TokenAdmin = tokenAdmin_result;

            Console.WriteLine($"Token Admin User has been registered => Registered Id is {tokenAdmin_result.Id} and User Name is {tokenAdmin_result.Name}");

            var tokenUserA_result = await _serviceClient.RegisterUserAsync(new UserInfo()
            {
                Name = "Token User A",
                BlockchainNetworkID = bc_result.Id,
                PartyID = party_result.Id,
                Description = "This is Token User A"
            });

            _appconfig.UserA = tokenUserA_result;

            Console.WriteLine($"Test User A has been registered => Registered Id is {tokenUserA_result.Id} and User Name is {tokenUserA_result.Name}");

            var tokenUserB_result = await _serviceClient.RegisterUserAsync(new UserInfo()
            {
                Name = "Token User B",
                BlockchainNetworkID = bc_result.Id,
                PartyID = party_result.Id,
                Description = "This is Token User B"
            });

            _appconfig.UserB = tokenUserB_result;
            Console.WriteLine($"Test User B has been registered => Registered Id is {tokenUserB_result.Id} and User Name is {tokenUserB_result.Name}\n\n");

            Console.WriteLine("Environmental Configuraiton is all Set\n");
        }

        /// <summary>
        /// Shows Token Transaction Use Case
        /// 1. Deploy NFT Token Smart Contract by Token Admin
        /// 2. Mint Token to User A by Token Admin
        /// 3. Validate(Check) Token Ownership - User A
        /// 4. Transfer Token from User A to User B by User A
        /// 5. Validate(Check) Token Ownership - User B
        /// 6. Read Token URI for Token 
        /// </summary>
        /// <returns></returns>
        private async Task ShowTokenTransactions()
        {
            Console.WriteLine("** Now, It shows NFT Token Transactions\n");

            Console.WriteLine("********************************************************");
            Console.WriteLine("**  Step 1. Deploy NFT Token Smart Contract           **");
            Console.WriteLine("********************************************************");

            //Deploy Token Smart Contract by TokenAdmin
            var tokenDeployment_result = await _serviceClient.DeployNewTokenAsync(_appconfig.TokenAdmin.Id.ToString(), "NFT Sample Token", "NFT");
            Console.WriteLine($"Token SmartContract Deployment has been completed by Token Admin User.....\n  Contract Address :{tokenDeployment_result.ContractAddress}\n\n");


            Console.WriteLine("********************************************************");
            Console.WriteLine("**  Step 2. Mint Token to User A                      **");
            Console.WriteLine("********************************************************");

            //Mint Token from Token Admin to User A. Token Number will be 1 (can be vary)
            //TokenURI can be used for any information holder
            var mintTxResult = await _serviceClient.MintTokenAsync(tokenDeployment_result.ContractAddress,
                                                                    _appconfig.TokenAdmin.Id.ToString(),
                                                                    _appconfig.UserA.Id.ToString(),
                                                                    1,
                                                                    JsonConvert.SerializeObject(
                                                                        new
                                                                        {
                                                                            Name = "NFT #1",
                                                                            ImageURL = "https://www.larvalabs.com/public/images/cryptopunks/punk7804.png",
                                                                            ImageHash = "F957D10ED1A02FD0D21454696FAE0878"
                                                                        }
                                                                        ));

            Console.WriteLine($"NFT Token (Token Number #1) has been minted to User A with Digital Image Information\n\n");


            Console.WriteLine("********************************************************");
            Console.WriteLine("**  Step 3. Check Who is this Token Owner            ***");
            Console.WriteLine("********************************************************");
            var tokenOwnerA = await _serviceClient.OwnerOfAsync(tokenDeployment_result.ContractAddress,
                                                _appconfig.UserA.Id.ToString(),
                                                1);

            Console.WriteLine($"Token Owner Public Address is {tokenOwnerA}");
            Console.WriteLine($"Token Owner's Name can be retrived from NFT API Service : Token Owner Name is {(await _serviceClient.GetUserByPublicAddressAsync(tokenOwnerA)).Name}\n\n");

            Console.WriteLine("********************************************************");
            Console.WriteLine("**  Step 4. Transfer token from User A to User B     ***");
            Console.WriteLine("********************************************************");

            //Transfer Token from User A to User B. Token Number is 1
            var transferResult = await _serviceClient.TransferAsync(tokenDeployment_result.ContractAddress,
                                                                    _appconfig.UserA.Id.ToString(),
                                                                    _appconfig.UserB.Id.ToString(),
                                                                    1);

            Console.WriteLine($"NFT Token (Token number #1) has been transferred from User A to User B\n");


            Console.WriteLine("********************************************************");
            Console.WriteLine("**  Step 5. Check Who is this Token Owner            ***");
            Console.WriteLine("********************************************************");

            var tokenOwnerB = await _serviceClient.OwnerOfAsync(tokenDeployment_result.ContractAddress,
                                                _appconfig.UserA.Id.ToString(),
                                                1);
            Console.WriteLine($"Token Owner Public Address is {tokenOwnerB}");
            Console.WriteLine($"Token Owner's Name can be retrived from NFT API Service : Token Owner Name is {(await _serviceClient.GetUserByPublicAddressAsync(tokenOwnerB)).Name}\n\n");


            Console.WriteLine("********************************************************");
            Console.WriteLine("**  Step 6. Check the Content in the Token           ***");
            Console.WriteLine("********************************************************");

            var tokenURI = await _serviceClient.TokenURIAsync(tokenDeployment_result.ContractAddress, _appconfig.TokenAdmin.Id.ToString(), 1);

            Console.WriteLine($"The Content in the Token number 1 is {tokenURI}");
        }

    }

    public class NFTSampleConfig
    {
        public BlockchainNetwork BlockchainNetwork { get; set; }
        public Party Party { get; set; }
        public User TokenAdmin { get; set; }
        public User UserA { get; set; }
        public User UserB { get; set; }
    }
}
