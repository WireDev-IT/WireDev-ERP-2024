using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDataDbContext context, ILogger<ProductController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProducts()
        {
            List<uint>? list;
            try
            {
                list = await _context.Products.Select(x => x.Uuid).ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"List of products cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, list));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(uint id)
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
                _logger.LogWarning(ex, message);
                return NotFound(new Response(false, message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The UUID of the added product.</returns>
        //[Authorize("PRODUCTS:RW")]
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody][Required(ErrorMessage = "To add a product, you have to provide one.")] Product product)
        {
            try
            {
                _ = await _context.Products.AddAsync(product);
                _ = await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Add product {product.Uuid} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return StatusCode(StatusCodes.Status201Created, new Response(true, null, product.Uuid));
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyProduct(uint id, [FromBody][Required(ErrorMessage = "To modify a product, you have to provide changes.")] Product product)
        {
            Product? p;
            try
            {
                p = await _context.Products.FirstAsync(x => x.Uuid == id);
                p = product;
                _ = _context.Update(p);
                _ = _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Product with the UUID {id} was not found!";
                _logger.LogError(ex, message);
                return NotFound(new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Edit product {id} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, p));
        }

        //[Authorize("PRODUCTS:RW")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(uint id)
        {
            Product? p;
            try
            {
                p = await _context.Products.FirstAsync(x => x.Uuid == id);
                _ = _context.Products.Remove(p);
                _ = _context.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Product with the UUID {id} was not found!";
                _logger.LogWarning(ex, message);
                return NotFound(new Response(false, message));
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Product with the UUID {id} could not be removed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, "Product was removed."));
        }
    }
}

