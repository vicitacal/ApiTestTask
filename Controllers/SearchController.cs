using ApiTestTask.Services.SearchServices;
using Microsoft.AspNetCore.Mvc;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SearchController : ControllerBase
    {

        public SearchController()
        {
        }

        [HttpPost]
        public async Task<SearchResponse> SearchRoute(SearchRequest request, CancellationToken cancellationToken, ISearchService searchService)
        {
            return await searchService.SearchAsync(request, cancellationToken);
        }
    }
}