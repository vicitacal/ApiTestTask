using ApiTestTask.Services.SearchServices;
using Microsoft.AspNetCore.Mvc;

namespace ApiTestTask.Controllers {

    [ApiController]
    [Route("api/v1/[controller]")]
    public class PingController : Controller {
        private readonly ISearchService _service;

        public PingController(ISearchService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<StatusCodeResult> Ping() {
            return (await _service.IsAvailableAsync(HttpContext.RequestAborted)) ? Ok() : StatusCode(503);
        }
    }
}
