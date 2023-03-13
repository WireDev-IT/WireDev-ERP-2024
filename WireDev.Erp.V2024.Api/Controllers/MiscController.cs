using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<PurchaseController> _logger;

        public MiscController(ApplicationDataDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Checks if the api controllers are working</summary>
        /// <response code="200">API is online</response>
        [HttpGet("ping"), AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

        /// <summary>Fills the database with sample data</summary>
        /// <response code="200">Database saved</response>
        [HttpPost("setup"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Setup(string[] csv)
        {
            ProductController pc = new(_context, null);
            PriceController pr = new(_context, null);
            for (int i = 0; i < csv.Length; i++)
            {
                string[] line = csv[i].Split(';');
                Price price = new() { RetailValue = Convert.ToDecimal(line[1]), SellValue = Convert.ToDecimal(line[2]) };
                ObjectResult? o = await pr.AddPrice(price) as ObjectResult;

                Product p = new() { Name = line[0], Prices = new() { (o.Value as Price).Uuid } };
                await pc.AddProduct(p);
            }
            return Ok();
        }
    }
}