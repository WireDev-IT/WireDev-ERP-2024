using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Enums;
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
                List<Product> list = await _context.Products.ToListAsync();
                return Ok(new Response(true, null, list));
            }
            catch (Exception ex)
            {
                string message = $"List of products cannot be retrieved!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            Product? product;
            try
            {
                product = await _context.Products.FirstAsync(x => x.Uuid == id);
                return Ok(new Response(true, null, product));
            }
            catch (Exception ex)
            {
                string message = $"Product with the UUID {id} was not found!";
                _logger.LogError(message, ex);
                return NotFound(new Response(true, message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The UUID of the added product.</returns>
        //[Authorize("PRODUCTS:RW")]
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                _ = await _context.Products.AddAsync(product);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string message = $"Add product {product.Uuid} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, product.Uuid));
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyProduct(Guid id, [FromBody] Product product)
        {
            Product? p;
            try
            {
                p = await _context.Products.FirstAsync(x => x.Uuid == id);
                p = product;
                _ = _context.Update(p);
                _ = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Edit product {id} failed!", ex);
                return NotFound();
            }

            return Ok(p);
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            Product? p;
            try
            {
                p = await _context.Products.FirstAsync(x => x.Uuid == id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Product with the UUID {id} was not found!", ex);
                return NotFound();
            }

            try
            {
                _ = _context.Products.Remove(p);
                _ = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Product UUID {id} could not be removed!", ex);
                return StatusCode(500);
            }

            return Ok();
        }
    }
}

