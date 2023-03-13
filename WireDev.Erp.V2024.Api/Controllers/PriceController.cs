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
    public class PriceController : Controller
    {
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<PriceController> _logger;

        public PriceController(ApplicationDataDbContext context, ILogger<PriceController> logger)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>Gets the ids of all existing price</summary>
        /// <response code="200">List was generated</response>
        /// <response code="404">There are no prices</response>
        /// <response code="500">Oops! Getting the prices from database failed</response>
        /// <returns>A List of Guid</returns>
        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPrices()
        {
            List<Guid>? list;
            try
            {
                list = await _context.Prices.Select(x => x.Uuid).ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                string message = $"There are no prices!";
                _logger.LogWarning(ex, message);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                string message = $"List of prices cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(list);
        }

        /// <summary>Gets an existing price</summary>
        /// <response code="200">Price was found</response>
        /// <response code="404">Price was not found</response>
        /// <response code="500">Oops! Getting the price from database failed</response>
        /// <param name="id">The id of the price object you want to get</param>
        /// <returns>A Price object</returns>
        [HttpGet("{id}"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetPrice(Guid id)
        {
            Price? price;
            try
            {
                price = await _context.Prices.FirstAsync(x => x.Uuid == id);
                return Ok(price);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Price with the UUID {id} was not found!";
                _logger.LogWarning(ex, message);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                string message = $"Price with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        /// <summary>Adds a new price</summary>
        /// <response code="200">Price was added</response>
        /// <response code="409">A price with the same id already exists</response>
        /// <response code="500">Oops! Adding the price to database failed</response>
        /// <param name="price">The price object to add</param>
        [HttpPost("add"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddPrice([FromBody][Required(ErrorMessage = "To add a price, you have to provide one.")] Price price)
        {
            try
            {
                if (_context.Prices.Find(price.Uuid) is not null)
                {
                    string message = $"Price {price.Uuid} already exists!";
                    _logger.LogWarning(message);
                    return Conflict(message);
                }

                _ = await _context.Prices.AddAsync(price);
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
                string message = $"Add price {price.Uuid} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status201Created, price);
        }

        /// <summary>Modifies an existing price</summary>
        /// <response code="200">Price was modified</response>
        /// <response code="404">The price does not exists</response>
        /// <response code="423">The price is locked</response>
        /// <response code="500">Oops! Modifing the price in database failed</response>
        /// <param name="id">The id of the price</param>
        /// <param name="price">The price object with modifications</param>
        /// <returns>The modified Price object</returns>
        [HttpPut("{id}"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> ModifyPrice(Guid id, [FromBody][Required(ErrorMessage = "To modify a price, you have to provide changes.")] Price price)
        {
            Price? temp_price;
            try
            {
                temp_price = await _context.Prices.FirstAsync(x => x.Uuid == id);
                if (temp_price.Locked)
                {
                    string message = $"Price {id} is locked and can not be modified!";
                    _logger.LogWarning(message);
                    return StatusCode(StatusCodes.Status423Locked, message);
                }
                else
                {
                    await temp_price.ModifyProperties(price);
                    _ = _context.Prices.Update(temp_price);
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
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                string message = $"Edit price {id} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(temp_price);
        }

        /// <summary>Deletes a price by its id</summary>
        /// <response code="200">Price was removed</response>
        /// <response code="404">The price does not exists</response>
        /// <response code="423">The price is locked</response>
        /// <response code="500">Oops! Deleting the price from database failed</response>
        /// <param name="id">The id of the price</param>
        [HttpDelete("{id}"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeletePrice(Guid id)
        {
            Price? c;
            try
            {
                c = await _context.Prices.FirstAsync(x => x.Uuid == id);

                if (c.Locked)
                {
                    string message = $"Price {c.Uuid} is locked and can not be deleted! You can archive it.";
                    _logger.LogWarning(message);
                    return StatusCode(StatusCodes.Status423Locked, message);
                }

                _ = _context.Prices.Remove(c);
                _ = _context.SaveChanges(User?.Identity.Name);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Price with the UUID {id} was not found!";
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
                string message = $"Price with the UUID {id} could not be removed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(null);
        }
    }
}