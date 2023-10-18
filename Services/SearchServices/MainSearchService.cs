namespace ApiTestTask.Services.SearchServices
{
    public class MainSearchService : ISearchService
    {
        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
