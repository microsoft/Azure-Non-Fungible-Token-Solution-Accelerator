// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Solutions.CosmosDB;
using Newtonsoft.Json;

namespace Microsoft.TokenService.TransactionIndexer.Model
{
    public class TransactionInfo : CosmosDBEntityBase
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public TransactionType TransactionType { get; set; }
        public string Caller { get; set; }
        public string Sender { get; set; }
        public string Receipient { get; set; }
        public TokenMeta MetaInfo { get; set; }
        public TransactionRecieptInfo TransactionReceiptInfo { get; set; }
    }

    public enum TransactionType
    {
        TokenDeployed,
        TokenMinted,
        TokenTransferred,
        TokenBurnt,
        SetApprovalForAll,
        Approve
    }

    public class TokenMeta
    {
        public long TokenId { get; set; }
        public string TokenUrl { get; set; }

    }

    public class TransactionRecieptInfo
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
