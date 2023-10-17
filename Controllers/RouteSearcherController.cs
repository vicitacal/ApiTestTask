using Microsoft.AspNetCore.Mvc;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteSearcherController : ControllerBase
    {
        private readonly ILogger<RouteSearcherController> _logger;

        public RouteSearcherController(ILogger<RouteSearcherController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public SearchResponse Search()
        {
            return new SearchResponse() { MaxMinutesRoute = 0, MaxPrice = 100, MinMinutesRoute = 5, MinPrice = 14, Routes = new Route[2] };
        }

        [HttpGet]
        public JsonResult Get() {
            return new JsonResult("Fuck");
        }
    }
}