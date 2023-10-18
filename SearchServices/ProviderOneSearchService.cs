namespace ApiTestTask.SearchServices {
    public class ProviderOneSearchService : ISearchService {
        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
