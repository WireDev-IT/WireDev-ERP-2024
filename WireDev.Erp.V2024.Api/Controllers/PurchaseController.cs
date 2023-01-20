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
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;
using WireDev.Erp.V1.Models.Statistics;
using System.Data;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("PURCHASE:RO")]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly PurchaseDbContext _context;
        private readonly ProductDbContext _context2;
        private readonly StatsDbContext _context3;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(PurchaseDbContext context, ProductDbContext context2, StatsDbContext context3, ILogger<PurchaseController> logger)
        {
            _context = context;
            _context2 = context2;
            _context3 = context3;
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

        //[Authorize("PURCHASE_SELL:RW")]
        [HttpPost("sell")]
        public async Task<IActionResult> SellPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Sell)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, new Response(false, message));
            }

            using IDbContextTransaction transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                Product? p = await _context2.Products.FirstOrDefaultAsync();
                purchase.TryAddItem(p.Uuid, p.Prices.First(), 3);
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
            transaction.CreateSavepoint("BeforeProductModification");

            Product? product = null;
            foreach (TransactionItem item in purchase.Items)
            {
                try
                {
                    product = await _context2.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Remove(item.Count);

                        _context2.Products.Update(product);
                        _ = await _context2.SaveChangesAsync();
                    }
                    else
                    {
                        throw new ArgumentNullException($"Product {item.ProductId} was not found. Rolling back changes.");
                    }
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    string message = $"Could not save changes to database! Rolling back changes.";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
                catch (ArgumentNullException ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex.Message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, ex.Message));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    string message = $"Modifing product {product.Uuid} failed! Rolling back changes.";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
            }
            transaction.CreateSavepoint("BeforeProductStatsModification");

            foreach (TransactionItem item in purchase.Items)
            {
                try
                {
                    ProductStats? productStats = await _context3.ProductStats.FindAsync(item.ProductId);
                    if (productStats == null)
                    {
                        productStats = new(item.ProductId);
                        productStats.AddTransaction(item);
                        await _context3.ProductStats.AddAsync(productStats);
                    }
                    else
                    {
                        productStats.AddTransaction(item);
                        _context3.ProductStats.Update(productStats);
                    }
                    _=await _context3.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    string message = $"Could not save changes to database! Rolling back changes.";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    string message = $"Modifing stats of product {product.Uuid} failed! Rolling back changes.";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
            }
            await transaction.CommitAsync();

            return Ok(new Response(true, null, purchase.Uuid));
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyPurchase([FromBody] Purchase purchase)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}

