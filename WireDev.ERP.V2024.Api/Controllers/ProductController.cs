using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly PurchaseDbContext _context2;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductDbContext context, PurchaseDbContext context2, ILogger<ProductController> logger)
        {
            _logger = logger;
            _context = context;
            _context2 = context2;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetProducts()
        {
            List<Product>? list;
            try
            {
                list = await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"List of products cannot be retrieved!";
                _logger.LogError(message, ex);
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
                _logger.LogWarning(message, ex);
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
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
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
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Product with the UUID {id} was not found!";
                _logger.LogError(message, ex);
                return NotFound(new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Edit product {id} failed!";
                _logger.LogError(message, ex);
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
                _logger.LogWarning(message, ex);
                return NotFound(new Response(false, message));
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Product with the UUID {id} could not be removed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, "Product was removed."));
        }

        //[Authorize("PURCHASE_SELL:RW")]
        [HttpPost("transaction")]
        public async Task<IActionResult> DoPurchase([FromBody][Required(ErrorMessage = "To do a transaction, you have to provide one.")] Purchase purchase)
        {
            try
            {
                _ = await _context2.Purchases.AddAsync(purchase);
                _ = await _context2.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Add purchase {purchase.Uuid} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            Product? product = null;
            foreach (KeyValuePair<(uint productId, Guid priceId, TransactionType type), uint> kvp in purchase.Items)
            {
                try
                {
                    product = await _context.Products.FindAsync(kvp.Key.productId);
                    if (product != null)
                    {

                        if (kvp.Key.type == TransactionType.Sell || kvp.Key.type == TransactionType.Withdraw || kvp.Key.type == TransactionType.Disposed)
                        {
                            product.Remove(kvp.Value);
                        }
                        else if (kvp.Key.type == TransactionType.Cancel || kvp.Key.type == TransactionType.Purchase)
                        {
                            product.Add(kvp.Value);
                        }
                        else
                        {
                            string message = "Unknown transacation type: " + kvp.Key.type.ToString();
                            _logger.LogCritical(message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                        }

                        _context.Products.Update(product);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateException ex)
                {
                    string message = $"Could not save changes to database!";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
                catch (Exception ex)
                {
                    string message = $"Modifing product {product.Uuid} failed!";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
            }

            return Ok(new Response(true, null, purchase.Uuid));
        }
    }
}

