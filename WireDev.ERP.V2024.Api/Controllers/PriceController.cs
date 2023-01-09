using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("PRICES:RO,PRICES:RW")]
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        private readonly PriceDbContext _context;
        private readonly ILogger<PriceController> _logger;

        public PriceController(PriceDbContext context, ILogger<PriceController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        public async Task<IActionResult> GetPrices()
        {
            List<Price>? list;
            try
            {
                list = await _context.Prices.ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"List of prices cannot be retrieved!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, list));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrice(Guid id)
        {
            Price? price;
            try
            {
                price = await _context.Prices.FirstAsync(x => x.Uuid == id);
                return Ok(new Response(true, null, price));
            }
            catch (Exception ex)
            {
                string message = $"Price with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return NotFound(new Response(true, message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        /// <returns>The UUID of the added price.</returns>
        //[Authorize("CATEGORIES:RW")]
        [HttpPost("add")]
        public async Task<IActionResult> AddPrice([FromBody][Required(ErrorMessage = "To add a price, you have to provide one.")] Price price)
        {
            try
            {
                _ = await _context.Prices.AddAsync(price);
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
                string message = $"Add price {price.Uuid} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, price.Uuid));
        }

        //[Authorize("CATEGORIES:RW")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyPrice(Guid id, [FromBody][Required(ErrorMessage = "To modify a price, you have to provide changes.")] Price price)
        {
            Price? c;
            try
            {
                c = await _context.Prices.FirstAsync(x => x.Uuid == id);
                c = price;
                _ = _context.Update(price);
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
                string message = $"Price with the UUID {id} was not found!";
                _logger.LogError(message, ex);
                return NotFound(new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Edit price {id} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, price));
        }

        //[Authorize("CATEGORIES:RW")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(Guid id)
        {
            Price? c;
            try
            {
                c = await _context.Prices.FirstAsync(x => x.Uuid == id);
                _ = _context.Prices.Remove(c);
                _ = _context.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Price with the UUID {id} was not found!";
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
                string message = $"Price with the UUID {id} could not be removed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, "Price was removed."));
        }
    }
}

