using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using LexPlatform.Client.Models;
using LexPlatform.Client.Configuration;

namespace LexPlatform.Client
{
    public interface IPlatformApi
    {
        Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request);
    }

    public class PlatformApi : IPlatformApi
    {
        private readonly IApiClient _apiClient;
        private readonly PlatformApiConfig _config;

        public PlatformApi(IOptions<PlatformApiConfig> config)
        {
            _config = config.Value;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_config.ApiUrl)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiClient.HeaderApplicationJson));

            _apiClient = new ApiClient(httpClient);
        }

        public async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request)
        {
            return await PostRequest<PlaceOrderResponse>(request);
        }

        private async Task<T> PostRequest<T>(RequestBase req)
        {
            var properties = req.BuildProperties() ?? new Dictionary<string, string>();
            properties["access_key"] = _config.ApiKey;
            properties["tonce"] = req.Timestamp.ToString();
            properties["signature"] = Sign(req.BuildPayload(properties), _config.ApiSecret);

            var content = new StringContent(req.BuildQueryStringFromParams(properties), Encoding.UTF8, "application/x-www-form-urlencoded");
            var requestMessage = new HttpRequestMessage(req.Method, req.Url())
            {
                Content = content
            };
            var response = await _apiClient.SendAsync<T>(requestMessage);
            return Resolve(response);
        }

        private T Resolve<T>(ApiResponse<T> apiResponse)
        {
            return apiResponse.IsSuccessStatusCode
                ? apiResponse.Result
                : default(T);
        }

        private string Sign(string payload, string apiSecret)
        {
            var keyByte = new ASCIIEncoding().GetBytes(apiSecret);
            var messageBytes = new ASCIIEncoding().GetBytes(payload);
            var hashmessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);

            // to lowercase hexits
            return string.Concat(Array.ConvertAll(hashmessage, x => x.ToString("x2")));
        }
    }
}
