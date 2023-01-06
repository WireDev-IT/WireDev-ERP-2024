using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("PRODUCTS:RO,PRODUCTS:RW")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProducts()
        {
            return StatusCode(501);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            return StatusCode(501);
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            return StatusCode(501);
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyProduct(Guid id, [FromBody] Product product)
        {
            return StatusCode(501);
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            return StatusCode(501);
        }
    }
}

