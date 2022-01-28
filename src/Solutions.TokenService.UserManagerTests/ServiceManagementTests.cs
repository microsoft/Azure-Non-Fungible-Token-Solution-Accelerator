// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.TokenService.UserManager;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Microsoft.TokenService.PartyManager.Model;
using Microsoft.TokenService.BlockchainNetworkManager.Model;
using Microsoft.TokenService.UserManager.Model;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Microsoft.TokenService.KeyManagement;
using Microsoft.TokenService.TransactionIndexer.Model;

namespace Microsoft.TokenService.Management.Tests
{
    [TestClass()]
    public class ServiceManagementTests
    {
        IConfigurationRoot _config;
        static Party _newParty;
        static BlockchainNetwork _blockchainNetwork;
        static User _user;
        static string _cosmosConnection;
        static CosmosConnectionStrings _cosmosConnectionStrings;
        static AzureKeyVaultKeyClient _akvKeyClient;

        [TestInitialize()]
        public void InitTest()
        {

            if(_config == null) _config = ConfigReader.ReadSettings();

            if(_cosmosConnectionStrings == null) _cosmosConnectionStrings = ConnectionStringAccessor
                                                                            .Create(_config["App:SubscriptionId"],
                                                                                    _config["App:ResourceGroupName"],
                                                                                    _config["App:DatabaseAccountName"])
                                                                            .GetConnectionStringsAsync(_config["App:ManagedIdentityId"]).GetAwaiter().GetResult();

            if(_cosmosConnection == null) _cosmosConnection = _cosmosConnectionStrings.PrimaryReadWriteKey;

            if (_akvKeyClient == null) _akvKeyClient = AzureKeyVaultKeyClient.Create()
                                                                             .Build(_config["KeyVault:KeyVaultUrl"],
                                                                                     _config["App:SubscriptionId"],
                                                                                     _config["App:ResourceGroupName"],
                                                                                     _config["App:ManagedIdentityId"]);
        }


        [TestMethod()]
        public async Task Test01_CreateGroup()
        {
            Microsoft.TokenService.PartyManager.Parties party =
                new TokenService.PartyManager.Parties(_cosmosConnection, "Management");
            _newParty = await party.RegisterParty("Foo Party", "Description for Foo");

            Console.WriteLine($"Party has been created. \nPartyID : {_newParty.Id} \nPartyName : {_newParty.PartyName}");

            Assert.IsNotNull(_newParty);
        }

        [TestMethod()]
        public async Task Test02_CreateBlockchainNetwork()
        {
            Microsoft.TokenService.BlockchainNetworkManager.BlockchainNetworks blockchainNetwork =
              new BlockchainNetworkManager.BlockchainNetworks(_cosmosConnection, "Management");
            _blockchainNetwork = await blockchainNetwork.RegisterBlockchainNetwork("Foo network", "http://foo", "blabla");

            Console.WriteLine($"BlockhChainNetwork has been created. \nBlockchainNetworkID : {_blockchainNetwork.Id} \nBlockchainNetworkName : {_blockchainNetwork.Name}");

            Assert.IsNotNull(_blockchainNetwork);
        }

        [TestMethod()]
        public async Task Test03_CreateUser()
        {
            Microsoft.TokenService.UserManager.Users userManager =
                         new Users(_config,  _cosmosConnectionStrings, _akvKeyClient);

            _user = await userManager.RegisterUser("Nerde", "bla bla", _newParty.Id, _blockchainNetwork.Id);

            Console.WriteLine($"User has been created. \nUserID : {_user.Id} \nUserName : {_user.Name} \nPublicAddress : {_user.PublicAddress}");

            Assert.IsNotNull(_user);
        }

        [TestMethod()]
        public async Task Test04_GetParty()
        {
            Microsoft.TokenService.PartyManager.Parties party =
                new TokenService.PartyManager.Parties(_cosmosConnection, "Management");
            _newParty = await party.GetParty(_newParty.Id);

            Console.WriteLine($"Party has been retrieved. \nPartyID : {_newParty.Id} \nPartyName : {_newParty.PartyName}");

            Assert.IsNotNull(_newParty);
        }

        [TestMethod()]
        public async Task Test05_GetBlockchainNetwork()
        {
            Microsoft.TokenService.BlockchainNetworkManager.BlockchainNetworks blockchainNetwork =
              new BlockchainNetworkManager.BlockchainNetworks(_cosmosConnection, "Management");
            _blockchainNetwork = await blockchainNetwork.GetBlockchainNetwork(_blockchainNetwork.Id);

            Console.WriteLine($"BlockhChainNetwork has been retrieved. \nBlockchainNetworkID : {_blockchainNetwork.Id} \nBlockchainNetworkName : {_blockchainNetwork.Name}");

            Assert.IsNotNull(_blockchainNetwork);
        }

        [TestMethod()]
        public async Task Test06_GetUser()
        {
            Microsoft.TokenService.UserManager.Users userManager =
                       new Users(_config, _cosmosConnectionStrings, _akvKeyClient);

            _user = await userManager.GetUser(_user.Id);
            Console.WriteLine($"User has been retrieved. \nUserID : {_user.Id} \nUserName : {_user.Name} \nPublicAddress : {_user.PublicAddress}");

            Assert.IsNotNull(_user);
        }

        [TestMethod()]
        public async Task Test07_DeleteGroup()
        {
            Microsoft.TokenService.PartyManager.Parties party =
                new TokenService.PartyManager.Parties(_cosmosConnection, "Management");
            await party.UnRegisterParty(_newParty.Id);

        }

        [TestMethod()]
        public async Task Test08_DeleteBlockchainNetwork()
        {
            Microsoft.TokenService.BlockchainNetworkManager.BlockchainNetworks blockchainNetwork =
              new BlockchainNetworkManager.BlockchainNetworks(_cosmosConnection, "Management");
            await blockchainNetwork.UnRegisterBlockchainNetwork(_blockchainNetwork.Id);
        }

        [TestMethod()]
        public async Task Test09_DeleteUser()
        {
            Microsoft.TokenService.UserManager.Users userManager =
                       new Users(_config, _cosmosConnectionStrings, _akvKeyClient);

            await userManager.UnRegistUser(_user.Id);
        }

        [TestMethod()]
        public async Task Test10_LogTransaction()
        {
            Microsoft.TokenService.TransactionIndexer.TransactionIndexerService txIndexerService =
                    new TransactionIndexer.TransactionIndexerService(_config, _cosmosConnectionStrings);
            await txIndexerService.LogTransactionAsync(new TransactionInfo()
            {
                Sender = "Foo",
                TransactionType = TransactionType.TokenDeployed,
                TransactionReceiptInfo = new TransactionRecieptInfo()
                {
                    BlockNumber = 1,
                    ContractAddress = "0x"
                }
            });
        }


    }


    public class ConfigReader
    {
        public ConfigReader()
        {
        }


        public static IConfigurationRoot ReadSettings()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string path = null;


            if (File.Exists(Path.Combine(fileInfo.Directory.FullName, "application.settings.json")))
            {
                //Normal Application
                path = fileInfo.Directory.FullName;
            }
            else if (File.Exists(Path.Combine(fileInfo.Directory.Parent.FullName, "application.settings.json")))
            {
                //For Function App
                path = fileInfo.Directory.Parent.FullName;
            }
            else if (File.Exists(Path.Combine(fileInfo.Directory.FullName, "appsettings.json")))
            {
                //Normal Application
                path = fileInfo.Directory.FullName;
                //For ASP.net core
                return new ConfigurationBuilder()
                       .SetBasePath(path)
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .Build();
            }

            return new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("application.settings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}