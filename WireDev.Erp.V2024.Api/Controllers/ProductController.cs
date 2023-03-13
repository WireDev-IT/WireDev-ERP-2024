using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
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

        /// <summary>Gets the ids of all existing products</summary>
        /// <response code="200">List was generated</response>
        /// <response code="404">There are no products</response>
        /// <response code="500">Oops! Getting the products from database failed</response>
        /// <returns>A List of uint</returns>
        [HttpGet("all"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetProducts()
        {
            List<uint>? list;
            try
            {
                list = await _context.Products.Select(x => x.Uuid).ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                string message = $"There are no products!";
                _logger.LogWarning(ex, message);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                string message = $"List of products cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(list);
        }

        /// <summary>Gets an existing product</summary>
        /// <response code="200">Product was found</response>
        /// <response code="404">Product was not found</response>
        /// <response code="500">Oops! Getting the product from database failed</response>
        /// <param name="id">The id of the product object you want to get</param>
        /// <returns>A Product object</returns>
        [HttpGet("{id}"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetProduct(uint id)
        {
            Product? product;
            try
            {
                product = await _context.Products.FirstAsync(x => x.Uuid == id);
                return Ok(product);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Product {id} was not found!";
                _logger.LogWarning(ex, message);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                string message = $"Product {id} cannot be retrieved!";
                _logger.LogWarning(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        /// <summary>Adds a new product</summary>
        /// <response code="200">Product was added</response>
        /// <response code="409">A product with the same id already exists</response>
        /// <response code="500">Oops! Adding the product to database failed</response>
        /// <param name="product">The product object to add</param>
        /// <returns>The added product.</returns>
        //[Authorize("PRODUCTS:RW")]
        [HttpPost("add"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddProduct([FromBody][Required(ErrorMessage = "To add a product, you have to provide one.")] Product product)
        {
            try
            {
                _ = await _context.Products.AddAsync(product);
                _ = _context.SaveChanges(User?.Identity.Name);
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            catch (Exception ex)
            {
                string message = $"Add product {product.Uuid} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status201Created, product);
        }

        /// <summary>Modifies an existing product</summary>
        /// <response code="200">Product was modified</response>
        /// <response code="404">The product does not exists</response>
        /// <response code="423">The product is locked</response>
        /// <response code="500">Oops! Modifing the product in database failed</response>
        /// <param name="id">The id of the product</param>
        /// <param name="product">The product object with modifications</param>
        /// <returns>The modified product object</returns>
        [HttpPut("{id}"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> ModifyProduct(uint id, [FromBody][Required(ErrorMessage = "To modify a product, you have to provide changes.")] Product product)
        {
            Product? temp_product;
            try
            {
                temp_product = await _context.Products.FirstAsync(x => x.Uuid == id);
                if (temp_product.Used)
                {
                    string message = $"Product {id} is in use and can not be modified!";
                    _logger.LogWarning(message);
                    return StatusCode(StatusCodes.Status423Locked, message);
                }
                else
                {
                    await temp_product.ModifyProperties(product);
                    _ = _context.Products.Update(temp_product);
                    _ = _context.SaveChanges(User?.Identity.Name);
                }
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Product with the UUID {id} was not found!";
                _logger.LogError(ex, message);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                string message = $"Edit product {id} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(temp_product);
        }

        /// <summary>Deletes a product by its id</summary>
        /// <response code="200">Product was removed</response>
        /// <response code="404">The product does not exists</response>
        /// <response code="423">The product is locked</response>
        /// <response code="500">Oops! Deleting the product from database failed</response>
        /// <param name="id">The id of the product</param>
        [HttpDelete("{id}"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteProduct(uint id)
        {
            Product? p;
            try
            {
                p = await _context.Products.FirstAsync(x => x.Uuid == id);

                if (p.Used)
                {
                    string message = $"Product {p.Uuid} is in use and can not be deleted! You can archive it.";
                    _logger.LogWarning(message);
                    return StatusCode(StatusCodes.Status423Locked, message);
                }

                _ = _context.Products.Remove(p);
                _ = _context.SaveChanges(User?.Identity.Name);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Product with the UUID {id} was not found!";
                _logger.LogWarning(ex, message);
                return NotFound(message);
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            catch (Exception ex)
            {
                string message = $"Product with the UUID {id} could not be removed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(null);
        }
    }
}