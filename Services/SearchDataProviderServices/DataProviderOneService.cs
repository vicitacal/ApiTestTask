using ApiTestTask.Services.SearchServices;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ApiTestTask.Services.SearchDataProviderServices
{
    public class DataProviderOneService : ISearchDataProviderService {

        public DataProviderOneService(HttpClient httpClient, IConfiguration appConfig) {
            _httpClient = httpClient;
            BaseUrl = appConfig["ProviderOneBaseUrl"];
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken) {
            return (await _httpClient.GetAsync(BaseUrl + "api/v1/ping", cancellationToken)).StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<ProviderResponse> SearchAsync(ProviderRequest request, CancellationToken cancellationToken) {
            var content = JsonContent.Create(providerContent);
            var response = await _httpClient.PostAsync(BaseUrl + "api/v1/search", content);
            var providerResponse = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(cancellationToken: cancellationToken) ?? throw new Exception("Cannot serialize response");
            var converter = TypeDescriptor.GetConverter(typeof(ProviderOneSearchResponse));
            return new ProviderResponse();
        }

        private readonly string BaseUrl = string.Empty;
        private readonly HttpClient _httpClient;
    }

}
