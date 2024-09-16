namespace FrankfurterApi.SDK
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class HttpResponseHelper
    {
        public static async Task<Result<TResult>> GetHttpResponseContentAsync<TResult>(HttpClient httpClient,
                                                                                       HttpMethod method,
                                                                                       string endpointUrl,
                                                                                       CancellationToken cancellationToken)
        {
            using var httpRequest = GetHttpRequest(httpClient, method, endpointUrl);
            cancellationToken.ThrowIfCancellationRequested();
            using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            if (httpResponse.IsSuccessStatusCode)
            {
                var responsePayload = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                return new Result<TResult>
                {
                    IsError = false,
                    StatusCode = (int) httpResponse.StatusCode,
                    Data = JsonSerializer.Deserialize<TResult>(responsePayload, options)
                };
            }

            return await httpResponse.GetErrorsAsync<TResult>(cancellationToken);
        }

        private static HttpRequestMessage GetHttpRequest(HttpClient httpClient,
                                                         HttpMethod method,
                                                         string endpointUrl)
        {
            var baseUrl = httpClient.BaseAddress?.AbsoluteUri;
            var httpRequest = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(baseUrl + (baseUrl != null && baseUrl.EndsWith("/") ? "" : "/") + endpointUrl)
            };

            return httpRequest;
        }

        private static async Task<Result<TResult>> GetErrorsAsync<TResult>(this HttpResponseMessage httpResponse,
                                                                           CancellationToken cancellationToken)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException(nameof(httpResponse));
            }

            var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            ErrorModel? responseContent = null;

            if (!string.IsNullOrEmpty(content))
            {
                responseContent = JsonSerializer.Deserialize<ErrorModel>(content);
            }

            return new Result<TResult>
            {
                IsError = true,
                StatusCode = (int) httpResponse.StatusCode,
                Errors = responseContent?.ErrorCodes ?? new[] {"error"}
            };
        }
    }
}