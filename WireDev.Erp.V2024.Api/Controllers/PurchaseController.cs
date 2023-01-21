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
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(ApplicationDataDbContext context, ILogger<PurchaseController> logger)
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

            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                Product? p = await _context.Products.FirstOrDefaultAsync();
                Price? price = await _context.Prices.FirstOrDefaultAsync();
                purchase.TryAddItem(p.Uuid, price, 3);
                purchase.Post();
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
            try
            {
                foreach (TransactionItem item in purchase.Items)
                {
                    product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Remove(item.Count);

                        _context.Products.Update(product);
                    }
                    else
                    {
                        throw new ArgumentNullException($"Product {item.ProductId} was not found. Rolling back changes.");
                    }
                }

                _ = await _context.SaveChangesAsync();
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
            transaction.CreateSavepoint("BeforeProductStatsModification");

            try
            {
                foreach (TransactionItem item in purchase.Items)
                {
                    ProductStats? productStats = await _context.ProductStats.FindAsync(item.ProductId);
                    if (productStats == null)
                    {
                        productStats = new(item.ProductId);
                        productStats.AddTransaction(item);
                        await _context.ProductStats.AddAsync(productStats);
                    }
                    else
                    {
                        productStats.AddTransaction(item);
                        _context.ProductStats.Update(productStats);
                    }
                }

                _ = await _context.SaveChangesAsync();
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
            transaction.CreateSavepoint("BeforeTimeStatsModification");

            try
            {
                foreach (TransactionItem item in purchase.Items)
                {
                    Price? price = await _context.Prices.FindAsync(item.PriceId);

                    TotalStats? totalStats = await _context.TotalStats.FirstOrDefaultAsync();
                    if (totalStats == null)
                    {
                        totalStats = new(DateTime.UtcNow);
                        await _context.TotalStats.AddAsync(totalStats);
                    }
                    totalStats.AddSells(item.Count);
                    totalStats.AddRevenue(item.Count * price.SellValue);
                    _context.TotalStats.Update(totalStats);

                    YearStats? yearStats = await _context.YearStats.FindAsync(new DateTime((int)DateTime.UtcNow.Year, 1, 1).Ticks);
                    if (yearStats == null)
                    {
                        yearStats = new(DateTime.UtcNow);
                        await _context.YearStats.AddAsync(yearStats);
                    }
                    yearStats.AddSells(item.Count);
                    yearStats.AddRevenue(item.Count * price.SellValue);
                    _context.YearStats.Update(yearStats);

                    MonthStats? monthStats = await _context.MonthStats.FindAsync(new DateTime((int)DateTime.UtcNow.Year, (int)DateTime.UtcNow.Month, 1).Ticks);
                    if (monthStats == null)
                    {
                        monthStats = new(DateTime.UtcNow);
                        await _context.MonthStats.AddAsync(monthStats);
                    }
                    monthStats.AddSells(item.Count);
                    monthStats.AddRevenue(item.Count * price.SellValue);
                    _context.MonthStats.Update(monthStats);

                    DayStats? dayStats = await _context.DayStats.FindAsync(new DateTime((int)DateTime.UtcNow.Year, (int)DateTime.UtcNow.Month, (int)DateTime.UtcNow.Day).Ticks);
                    if (dayStats == null)
                    {
                        dayStats = new(DateTime.UtcNow);
                        await _context.DayStats.AddAsync(dayStats);
                    }
                    dayStats.AddSells(item.Count);
                    dayStats.AddRevenue(item.Count * price.SellValue);
                    _context.DayStats.Update(dayStats);
                }

                _ = await _context.SaveChangesAsync();
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

