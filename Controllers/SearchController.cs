using ApiTestTask.SearchServices;
using Microsoft.AspNetCore.Mvc;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        public SearchController()
        {
        }

        [HttpPost]
        public SearchResponse SearchRoute(SearchRequest request)
        {
            return new SearchResponse() { MaxMinutesRoute = 0, MaxPrice = 100, MinMinutesRoute = 5, MinPrice = 14, Routes = new ApiTestTask.SearchServices.Route[2] };
        }
    }
}