using ApiTestTask.Services.SearchServices;
using Microsoft.AspNetCore.Mvc;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<SearchResponse> SearchRoute(SearchRequest request, CancellationToken cancellationToken)
        {
            return await _searchService.SearchAsync(request, cancellationToken);
        }
    }
}