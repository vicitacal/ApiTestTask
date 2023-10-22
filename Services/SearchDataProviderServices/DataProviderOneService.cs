using System.ComponentModel;

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
            var requestConverter = TypeDescriptor.GetConverter(typeof(ProviderOneSearchRequest));
            var content = JsonContent.Create(requestConverter.ConvertFrom(request) ?? throw new Exception("Cannot conver request type"));
            var response = await _httpClient.PostAsync(BaseUrl + "api/v1/search", content, cancellationToken);
            var providerResponse = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(cancellationToken: cancellationToken) ?? throw new Exception("Cannot serialize response");
            var responceConverter = TypeDescriptor.GetConverter(typeof(ProviderOneSearchResponse));
            return (ProviderResponse?)responceConverter.ConvertTo(providerResponse, typeof(ProviderResponse)) ?? throw new Exception("Cannot conver responce type");
        }

        private readonly string BaseUrl = string.Empty;
        private readonly HttpClient _httpClient;
    }

}
