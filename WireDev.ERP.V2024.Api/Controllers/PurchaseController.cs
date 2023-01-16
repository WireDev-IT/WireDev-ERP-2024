using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WireDev.Erp.V1.Models.Storage;
using WireDev.Erp.V1.Api.Context;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("PURCHASE:RO")]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly PurchaseDbContext _context;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(PurchaseDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetPurchases()
        {
            List<Purchase>? list;
            try
            {
                list = await _context.Purchases.ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"List of purchases cannot be retrieved!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, list));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchase(Guid id)
        {
            Purchase? purchase;
            try
            {
                purchase = await _context.Purchases.FirstAsync(x => x.Uuid == id);
                return Ok(new Response(true, null, purchase));
            }
            catch (Exception ex)
            {
                string message = $"Purchase with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return NotFound(new Response(true, message));
            }
        }

        //[Authorize("PURCHASE_BUY:RW")]
        [HttpPost("buy")]
        public async Task<IActionResult> BuyPurchase([FromBody] Purchase purchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_SELL:RW")]
        [HttpPost("sell")]
        public async Task<IActionResult> SellPurchase([FromBody] Purchase purchase)
        {
            foreach(KeyValuePair<(uint productId, Guid priceId, TransactionType type), uint> kvp in purchase.Items)
            {
                if (kvp.Key.type != TransactionType.Sell)
                {
                    string message = $"Prodcut {kvp.Key.productId} has no correct transaction type!";
                    _logger.LogWarning(message);
                    return StatusCode(StatusCodes.Status400BadRequest, new Response(false, message));
                }
            }

            try
            {
                _ = await _context.Purchases.AddAsync(purchase);
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
                string message = $"Add purchase {purchase.Uuid} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            try
            {
                _ = await _context.Purchases.AddAsync(purchase);
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
                string message = $"Add purchase {purchase.Uuid} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, purchase.Uuid));
        }

        //[Authorize("PURCHASE_CANCEL:RW")]
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPurchase([FromBody] Purchase purchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        //[Authorize("PURCHASE_WITHDRAW:RW")]
        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawPurchase([FromBody] Purchase purchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}

