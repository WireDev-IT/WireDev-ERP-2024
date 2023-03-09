using Microsoft.AspNetCore.Mvc;
using WireDev.Erp.V1.Models.Authentication;

namespace WireDev.Erp.V1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}
