namespace ApiTestTask.Services.SearchDataProviderServices
{
    public interface ISearchDataProviderService
    {
        Task<ProviderResponse> SearchAsync(ProviderRequest request, CancellationToken cancellationToken);
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
