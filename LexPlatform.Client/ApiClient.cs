using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LexPlatform.Client
{
    public interface IApiClient
    {
        Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url);
        Task<ApiResponse<TResponse>> SendAsync<TResponse>(HttpRequestMessage request);
    }

    public class ApiClient : IApiClient
    {
        public const string HeaderApplicationJson = "application/json";

        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url)
        {
            using (var response = await _client.GetAsync(url))
            {
                return await GetContentAsync<TResponse>(response);
            }
        }

        public async Task<ApiResponse<TResponse>> SendAsync<TResponse>(HttpRequestMessage request)
        {
            using (var response = await _client.SendAsync(request))
            {
                return await GetContentAsync<TResponse>(response);
            }
        }

        private async Task<ApiResponse<TResponse>> GetContentAsync<TResponse>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<TResponse>(json);
                var apiResponse = new ApiResponse<TResponse>(obj, response.Headers)
                {
                    StatusCode = response.StatusCode
                };
                return apiResponse;
            }

            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                var apiResponse = new ApiResponse<TResponse>(response.Headers)
                {
                    StatusCode = response.StatusCode
                };
                return apiResponse;
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new ApiException(error);
        }
    }
}
