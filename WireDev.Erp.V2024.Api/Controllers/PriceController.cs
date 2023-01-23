using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<PriceController> _logger;

        public PriceController(ApplicationDataDbContext context, ILogger<PriceController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //[Authorize(Roles = "Administrator")]
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
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, list));
        }

        //[Authorize("PRICES:RO")]
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

        //[Authorize("PRICES:RW")]
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
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Add price {price.Uuid} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, price.Uuid));
        }

        //[Authorize("PRICES:RW")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyPrice(Guid id, [FromBody][Required(ErrorMessage = "To modify a price, you have to provide changes.")] Price price)
        {
            Price? temp_price;
            try
            {
                temp_price = await _context.Prices.FirstAsync(x => x.Uuid == id);
                if (temp_price == null)
                {
                    throw new ArgumentNullException($"Price {id} was not found!");
                }
                else if (temp_price.Locked)
                {
                    throw new UnauthorizedAccessException($"Price {id} is locked and can not be modified!");
                }
                else
                {
                    await temp_price.ModifyProperties(price);
                    _ = _context.Prices.Update(temp_price);
                    _ = _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new Response(false, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status403Forbidden, new Response(false, ex.Message));
            }
            catch (Exception ex)
            {
                string message = $"Edit price {id} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, temp_price));
        }

        //[Authorize("PRICES:RW")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(Guid id)
        {
            Price? c;
            try
            {
                c = await _context.Prices.FirstAsync(x => x.Uuid == id);

                if (c.Locked) throw new BadHttpRequestException($"Price {c.Uuid} is locked and can not be deleted! Try to archive it.");

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
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new Response(false, ex.Message));
            }
            catch (Exception ex)
            {
                string message = $"Price with the UUID {id} could not be removed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, "Price was removed."));
        }
    }
}

