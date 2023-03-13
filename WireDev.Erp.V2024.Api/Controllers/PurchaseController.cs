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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
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

        /// <summary>
        /// Creates new TimeStats objects
        /// </summary>
        /// <returns>(TotalStats, YearStats, MonthStats, DayStats)</returns>
        private Task<(TotalStats, YearStats, MonthStats, DayStats)> PrepareTimeStats()
        {
            TotalStats? totalStats;
            YearStats? yearStats;
            MonthStats? monthStats;
            DayStats? dayStats;
            DateTime time = DateTime.UtcNow;

            totalStats = _context.TotalStats?.FirstOrDefault();
            if (totalStats == null || !_context.TotalStats.Any())
            {
                totalStats = new(time);
                _context.TotalStats.Add(totalStats);
            }

            yearStats = _context.YearStats.Find(new DateTime((int)time.Year, 1, 1).Ticks);
            if (yearStats == null)
            {
                yearStats = new(time);
                _context.YearStats.Add(yearStats);
            }

            monthStats = _context.MonthStats.Find(new DateTime((int)time.Year, (int)time.Month, 1).Ticks);
            if (monthStats == null)
            {
                monthStats = new(time);
                _context.MonthStats.Add(monthStats);
            }

            dayStats = _context.DayStats.Find(new DateTime((int)time.Year, (int)time.Month, (int)time.Day).Ticks);
            if (dayStats == null)
            {
                dayStats = new(time);
                _context.DayStats.Add(dayStats);
            }

            return Task.FromResult((totalStats, yearStats, monthStats, dayStats));
        }

        /// <summary>
        /// Modifies product and generates stats.
        /// </summary>
        /// <param name="item">The TransactionItem to process.</param>
        /// <param name="type">The Type od the transaction.</param>
        /// <exception cref="ArgumentNullException">Product not found</exception>
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

                product.Use();
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

        /// <summary>
        /// Saves the purchase and modifies time stats.
        /// </summary>
        /// <param name="purchase">The Purchase object to process</param>
        private async Task<ObjectResult> ProcessTransaction(Purchase purchase)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                try
                {
                    purchase.Post();
                    _ = await _context.Purchases.AddAsync(purchase);
                }
                catch (Exception ex)
                {
                    string message = $"Add purchase {purchase.Uuid} failed!";
                    _logger.LogError(ex, message);
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
                _ = _context.SaveChanges(User?.Identity.Name);
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
                    _logger.LogError(ex, message);
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
                _ = _context.SaveChanges(User?.Identity.Name);
                transaction.CreateSavepoint("BeforeProductModification");

                Product? product = null;
                Price? price = null;
                try
                {
                    foreach (TransactionItem item in purchase.Items)
                    {
                        await ProcessProductWithStats(item, purchase.Type);

                        price = await _context.Prices.FindAsync(item.PriceId);
                        if (price != null)
                        {
                            await totalStats.AddTransaction(item.Count, price, purchase.Type);
                            await yearStats.AddTransaction(item.Count, price, purchase.Type);
                            await monthStats.AddTransaction(item.Count, price, purchase.Type);
                            await dayStats.AddTransaction(item.Count, price, purchase.Type);
                            price.Lock();
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
                    _context.Prices.Update(price);
                }
                catch (ArgumentNullException ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    string message = $"Modifing product {product.Uuid} failed! Rolling back changes.";
                    _logger.LogError(ex, message);
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
                _ = _context.SaveChanges(User?.Identity.Name);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                string message = $"Could not save changes to database! Rolling back changes.";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                string message = $"Processing purchase {purchase.Uuid} failed! Rolling back changes.";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            await transaction.CommitAsync();

            return Ok(purchase.Uuid);
        }

        /// <summary>Gets a list of all purchases</summary>
        /// <response code="200">List was generated</response>
        /// <response code="500">Database error</response>
        /// <returns>A List of Guid</returns>
        [HttpGet("all"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetPurchases()
        {
            List<Guid>? list;
            try
            {
                list = await _context.Purchases.Select(x => x.Uuid).ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"List of purchases cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(list);
        }

        /// <summary>Get a purchase by its id</summary>
        /// <param name="id">The id of the purchase</param>
        /// <response code="200">Purchase was found</response>
        /// <response code="404">Purchase was not found</response>
        /// <returns>A Purchase object</returns>
        [HttpGet("{id}"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetPurchase(Guid id)
        {
            Purchase? purchase;
            try
            {
                purchase = await _context.Purchases.FirstAsync(x => x.Uuid == id);
                return Ok(purchase);
            }
            catch (Exception ex)
            {
                string message = $"Purchase with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return NotFound(message);
            }
        }

        /// <summary>Processes a sell purchase</summary>
        /// <param name="purchase">The purchase object to process</param>
        /// <response code="200">Saved</response>
        /// <response code="400">Purchase has wrong transaction type</response>
        /// <response code="500">Database error</response>
        /// <returns>GUID of the purchase object</returns>
        [HttpPost("sell"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> SellPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Sell)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, message);
            }

            return await ProcessTransaction(purchase);
        }

        /// <summary>Processes a buy purchase</summary>
        /// <param name="purchase">The purchase object to process</param>
        /// <response code="200">Saved</response>
        /// <response code="400">Purchase has wrong transaction type</response>
        /// <response code="500">Database error</response>
        /// <returns>GUID of the purchase object</returns>
        [HttpPost("buy"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> BuyPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Purchase)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, message);
            }

            return await ProcessTransaction(purchase);
        }

        /// <summary>Processes a cancel purchase</summary>
        /// <param name="purchase">The purchase object to process</param>
        /// <response code="200">Saved</response>
        /// <response code="400">Purchase has wrong transaction type</response>
        /// <response code="500">Database error</response>
        /// <returns>GUID of the purchase object</returns>
        [HttpPost("cancel"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> CancelPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Cancel)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, message);
            }

            return await ProcessTransaction(purchase);
        }

        /// <summary>Processes a refund purchase</summary>
        /// <param name="purchase">The purchase object to process</param>
        /// <response code="200">Saved</response>
        /// <response code="400">Purchase has wrong transaction type</response>
        /// <response code="500">Database error</response>
        /// <returns>GUID of the purchase object</returns>
        [HttpPost("refund"), Authorize(Roles = "Seller")]
        public async Task<IActionResult> RefundPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Refund)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, message);
            }

            return await ProcessTransaction(purchase);
        }

        /// <summary>Processes a dispose purchase</summary>
        /// <param name="purchase">The purchase object to process</param>
        /// <response code="200">Saved</response>
        /// <response code="400">Purchase has wrong transaction type</response>
        /// <response code="500">Database error</response>
        /// <returns>GUID of the purchase object</returns>
        [HttpPost("dispose"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> DisposalPurchase([FromBody] Purchase purchase)
        {
            if (purchase.Type != TransactionType.Disposed)
            {
                string message = $"Purchase has not the correct transaction type!";
                _logger.LogWarning(message);
                return StatusCode(StatusCodes.Status400BadRequest, message);
            }

            return await ProcessTransaction(purchase);
        }
    }
}

