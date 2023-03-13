using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireDev.Erp.V1.Models.Authentication;

namespace WireDev.Erp.V1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        /// <summary>Checks if the api controllers are working</summary>
        /// <response code="200">API is online</response>
        [HttpGet("ping"), AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

        /// <summary>Fills the database with sample data</summary>
        /// <response code="200">Database saved</response>
        [HttpGet("setup"), Authorize(Roles = "Admin")]
        public IActionResult Setup()
        {
            return null;
        }
    }
}
