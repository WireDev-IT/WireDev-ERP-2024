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

        private Task<(TotalStats, YearStats, MonthStats, DayStats)> PrepareTimeStats()
        {
            TotalStats? totalStats;
            YearStats? yearStats;
            MonthStats? monthStats;
            DayStats? dayStats;
            DateTime time = DateTime.UtcNow;

            totalStats = _context.TotalStats.FirstOrDefault();
            if (totalStats == null)
            {
                totalStats = new(time);
                _context.TotalStats.AddAsync(totalStats);
            }

            yearStats = _context.YearStats.Find(new DateTime((int)time.Year, 1, 1).Ticks);
            if (yearStats == null)
            {
                yearStats = new(time);
                _context.YearStats.AddAsync(yearStats);
            }

            monthStats = _context.MonthStats.Find(new DateTime((int)time.Year, (int)time.Month, 1).Ticks);
            if (monthStats == null)
            {
                monthStats = new(time);
                _context.MonthStats.AddAsync(monthStats);
            }

            dayStats = _context.DayStats.Find(new DateTime((int)time.Year, (int)time.Month, (int)time.Day).Ticks);
            if (dayStats == null)
            {
                dayStats = new(time);
                _context.DayStats.AddAsync(dayStats);
            }

            return Task.FromResult((totalStats, yearStats, monthStats, dayStats));
        }

        private async Task ProcessTimeStats(uint count, Price price, TransactionType type, TotalStats totalStats, YearStats yearStats, MonthStats monthStats, DayStats dayStats)
        {
            await totalStats.AddTransaction(count, price, type);
            await yearStats.AddTransaction(count, price, type);
            await monthStats.AddTransaction(count, price, type);
            await dayStats.AddTransaction(count, price, type);
        }

        private Task ProcessProductWithStats(TransactionItem item, TransactionType type)
        {
            Product? product;
            product = _context.Products.Find(item.ProductId);
            if (product != null)
            {
                if (type == TransactionType.Sell || type == TransactionType.Disposed)
                    product.Remove(item.Count);
                else if (type == TransactionType.Cancel || type == TransactionType.Purchase)
                    product.Add(item.Count);
                _context.Products.Update(product);
            }
            else
            {
                throw new ArgumentNullException($"Product {item.ProductId} was not found. Rolling back changes.");
            }

            ProductStats? productStats = _context.ProductStats.Find(item.ProductId);
            if (productStats == null)
            {
                productStats = new(item.ProductId);
                productStats.AddTransaction(item, type);
                _context.ProductStats.Add(productStats);
            }
            else
            {
                productStats.AddTransaction(item, type);
                _context.ProductStats.Update(productStats);
            }

            return Task.CompletedTask;
        }

        private async Task<ObjectResult> ProcessTransaction(Purchase purchase)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                try
                {
                    Product? p = await _context.Products.FirstOrDefaultAsync();
                    Price? price = await _context.Prices.FirstOrDefaultAsync();
                    purchase.TryAddItem(p.Uuid, price, 3);
                    purchase.Post();
                    _ = await _context.Purchases.AddAsync(purchase);
                }
                catch (Exception ex)
                {
                    string message = $"Add purchase {purchase.Uuid} failed!";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
                _ = await _context.SaveChangesAsync();
                transaction.CreateSavepoint("BeforeStaticsPreparation");

                TotalStats? totalStats;
                YearStats? yearStats;
                MonthStats? monthStats;
                DayStats? dayStats;
                try
                {
                    (totalStats, yearStats, monthStats, dayStats) = await PrepareTimeStats();
                }
                catch (Exception ex)
                {
                    string message = $"Preparing time statistics failed!";
                    _logger.LogError(message, ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
                }
                _ = await _context.SaveChangesAsync();
                transaction.CreateSavepoint("BeforeProductModification");

                Product? product = null;
                try
                {
                    foreach (TransactionItem item in purchase.Items)
                    {
                        await ProcessProductWithStats(item, purchase.Type);

                        Price? price = await _context.Prices.FindAsync(item.PriceId);
                        if (price != null)
                        {
                            await ProcessTimeStats(item.Count, price, purchase.Type, totalStats, yearStats, monthStats, dayStats);
                        }
                        else
                        {
                            throw new ArgumentNullException($"Price {item.ProductId} was not found. Rolling back changes.");
                        }
                    }

                    _context.TotalStats.Update(totalStats);
                    _context.YearStats.Update(yearStats);
                    _context.MonthStats.Update(monthStats);
                    _context.DayStats.Update(dayStats);
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
                await transaction.RollbackAsync();
                string message = $"Processing purchase {purchase.Uuid} failed! Rolling back changes.";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }
            await transaction.CommitAsync();

            return Ok(new Response(true, null, purchase.Uuid));
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

            return await ProcessTransaction(purchase);
        }

        //[Authorize("PURCHASE_BUY:RW")]
        [HttpPost("buy")]
        public async Task<IActionResult> BuyPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Purchase)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, new Response(false, message));
            }

            return await ProcessTransaction(purchase);
        }

        //[Authorize("PURCHASE_CANCEL:RW")]
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Cancel)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, new Response(false, message));
            }

            return await ProcessTransaction(purchase);
        }

        //[Authorize("PURCHASE_REFUND:RW")]
        [HttpPost("refund")]
        public async Task<IActionResult> RefundPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Refund)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, new Response(false, message));
            }

            return await ProcessTransaction(purchase);
        }

        //[Authorize("PURCHASE_DISPOSE:RW")]
        [HttpPost("dispose")]
        public async Task<IActionResult> DisposalPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Disposed)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, new Response(false, message));
            }

            return await ProcessTransaction(purchase);
        }
    }
}

