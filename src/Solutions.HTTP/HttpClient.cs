// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Flurl.Http;
using Polly;
using Polly.Retry;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.Solutions.HTTP
{
    public class HttpClient
    {
        static HttpClient()
        {
            policy = BuildRetryPolicy();
        }

        private static AsyncRetryPolicy policy;

        public static string ServiceBaseUrl = "";

        public static async Task<T> GetJsonAsync<T>(string url, string bearerToken = "", object headers = null)
        {
            return await policy.ExecuteAsync(
                () => url
                .WithHeaders(new { Accept = "application/json" })
                .WithOAuthBearerToken(bearerToken)
                .GetJsonAsync<T>()
            );
        }

        public static async Task<T> PostJsonAsync<T>(string url, object data, string bearerToken = "")
        {
            return await policy.ExecuteAsync(async () => await url
                                                   .WithHeaders(new { Accept = "application/json" })
                                                   .WithOAuthBearerToken(bearerToken)
                                                   .PostJsonAsync(data)
                                                   .ReceiveJson<T>());

        }

        public static async Task<T> PostJsonAsyncWithHeaders<T>(string url, object data, string bearerToken = "", object headers = null)
        {
            return await policy.ExecuteAsync(async () => await url
                                                   .WithHeaders(headers)
                                                   .WithOAuthBearerToken(bearerToken)
                                                   .PostJsonAsync(data)
                                                   .ReceiveJson<T>());

        }


        private static bool IsTransientError(FlurlHttpException exception)
        {
            int[] httpStatusCodesWorthRetrying =
            {
                (int)HttpStatusCode.RequestTimeout, // 408
                (int)HttpStatusCode.BadGateway, // 502
                (int)HttpStatusCode.ServiceUnavailable, // 503
                (int)HttpStatusCode.GatewayTimeout // 504
            };

            return exception.StatusCode.HasValue && httpStatusCodesWorthRetrying.Contains(exception.StatusCode.Value);
        }

        private static AsyncRetryPolicy BuildRetryPolicy()
        {
            return Policy
               .Handle<FlurlHttpException>(IsTransientError)
               .WaitAndRetryAsync(3, retryAttempt =>
               {
                   var nextAttemptIn = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                   Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptIn.TotalSeconds} seconds.");
                   return nextAttemptIn;
               });
        }
    }
}
