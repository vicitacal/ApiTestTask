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
            var content = JsonContent.Create(requestConverter.ConvertFrom(request) ?? throw new Exception("Cannot convert request type"));
            var response = await _httpClient.PostAsync(BaseUrl + "api/v1/search", content, cancellationToken);
            var providerResponse = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(cancellationToken: cancellationToken) ?? throw new Exception("Cannot serialize response");
            var responseConverter = TypeDescriptor.GetConverter(typeof(ProviderOneSearchResponse));
            return (ProviderResponse?)responseConverter.ConvertTo(providerResponse, typeof(ProviderResponse)) ?? throw new Exception("Cannot convert response type");
        }

        private readonly string BaseUrl = string.Empty;
        private readonly HttpClient _httpClient;
    }

}
