using Microsoft.AspNetCore.Mvc;

namespace ApiTestTask.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class PingController : Controller {
        public PingController() {
        }

        [HttpGet]
        public void Ping() {
            Ok();
        }
    }
}
