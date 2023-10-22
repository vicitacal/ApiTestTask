using ApiTestTask.Services.SearchServices;
using Microsoft.AspNetCore.Mvc;

namespace ApiTestTask.Controllers {

    [ApiController]
    [Route("api/v1/[controller]")]
    public class PingController : Controller {
        public PingController() {
        }

        [HttpGet]
        public async Task<StatusCodeResult> Ping(MainSearchService service, CancellationToken token) {
            return (await service.IsAvailableAsync(token)) ? Ok() : StatusCode(503);
        }
    }
}
