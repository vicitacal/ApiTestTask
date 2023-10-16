
namespace TestTask;

internal class SearchService : ISearchService, IDisposable
{

    private HttpClient _httpClient;

    public SearchService()
    {
        _httpClient = new();
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        return (await _httpClient.GetAsync("HTTP GET http://provider-one/api/v1/ping", cancellationToken)).StatusCode == System.Net.HttpStatusCode.OK;
    }

    public Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
