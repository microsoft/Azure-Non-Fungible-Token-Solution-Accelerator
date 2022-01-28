// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Solutions.Security.Cosmos.Tests
{
    [TestClass()]
    public class ConnectionStringAccessorTests
    {
        [TestMethod()]
        public async Task ConnectionStringAccessorTest()
        {
            var result = (await ConnectionStringAccessor.Create("{Your Subscription ID}", "{ResourceGroupName}", "{DatabaseName}")
                .GetConnectionStringsAsync("Your Managed Identity Client ID")).PrimaryReadWriteKey;

            Console.WriteLine(result);

            Assert.IsTrue(result.Length > 0);
        }
    }
}