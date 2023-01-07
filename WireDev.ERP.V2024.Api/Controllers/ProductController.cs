using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("PRODUCTS:RO,PRODUCTS:RW")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductDbContext context, ILogger<ProductController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                return Ok(await _context.Products.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError($"List of products cannot be retrieved!", ex);
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            Product? p;
            try
            {
                p = await _context.Products.FirstOrDefaultAsync(x => x.Uuid == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Product with the UUID {id.ToString()} was not found!", ex);
                return NotFound();
            }

            return Ok(p);
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Add product {product.Uuid.ToString()} canceled!", ex);
                return StatusCode(499, ex);
            }

            return Ok();
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

