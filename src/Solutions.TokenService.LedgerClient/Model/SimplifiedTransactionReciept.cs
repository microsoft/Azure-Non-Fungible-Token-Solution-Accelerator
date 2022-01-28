// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.TokenService.LedgerClient.Model
{

    public class TransactionReciept
    {
        public string TransactionHash { get; set; }

        public long TransactionIndex { get; set; }

        public string BlockHash { get; set; }
        
        public long BlockNumber { get; set; }
        
        public long CumulativeGasUsed { get; set; }

        public long GasUsed { get; set; }

        public string ContractAddress { get; set; }

        public long Status { get; set; }

    }
}
