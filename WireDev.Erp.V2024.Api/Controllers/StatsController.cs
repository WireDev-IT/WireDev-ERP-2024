using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Statistics;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("STATS:RO")]
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<PurchaseController> _logger;

        public StatsController(ApplicationDataDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all total statistics
        /// </summary>
        /// <response code="200">Retrieving total stats succeded</response>
        /// <response code="204">There are no statistics</response>
        /// <response code="500">Oops! List of total statistics cannot be retrieved</response>
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalStats()
        {
            List<TotalStats>? stats;
            try
            {
                stats = await _context.TotalStats.ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, new Response(true, "There are no statistics."));
            }
            catch (Exception ex)
            {
                string message = $"List of total statistics cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            if (stats.Count == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, new Response(false, "There are no statistics."));
            }

            return Ok(new Response(true, null, stats));
        }

        [HttpGet("year/{i}")]
        public async Task<IActionResult> GetYearStats(ushort i)
        {
            YearStats? stats;
            try
            {
                stats = await _context.YearStats.FindAsync(new DateTime(i, 1, 1).Ticks);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for year {i} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            if (stats == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response(false, "There are no statistics."));
            }

            return Ok(new Response(true, null, stats));
        }

        [HttpGet("year/all")]
        public async Task<IActionResult> GetAllYearStats()
        {
            List<long>? list;
            List<ushort>? years = new();
            try
            {
                list = await _context.YearStats.Select(x => x.Date).ToListAsync();
                if (list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new Response(false, "There are no statistics."));
                }

                foreach (long l in list)
                {
                    years.Add((ushort)new DateTime(l).Year);
                }
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, new Response(true, "There are no statistics."));
            }
            catch (Exception ex)
            {
                string message = $"List of year stats cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, years));
        }

        [HttpGet("month/{y}/{m}")]
        public async Task<IActionResult> GetMonthStats(ushort y, ushort m)
        {
            MonthStats? stats;
            try
            {
                stats = await _context.MonthStats.FindAsync(new DateTime(y, m, 1).Ticks);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for month {y}/{m} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            if (stats == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response(false, "There are no statistics."));
            }

            return Ok(new Response(true, null, stats));
        }

        [HttpGet("month/all")]
        public async Task<IActionResult> GetAllMonthStats([FromQuery] ushort minYear = ushort.MinValue, [FromQuery] ushort maxYear = ushort.MaxValue)
        {
            List<long>? list;
            List<DateTime>? months = new();
            try
            {
                list = await _context.MonthStats.Select(x => x.Date).Where(y => new DateTime(y).Year >= minYear && new DateTime(y).Year <= maxYear).ToListAsync();
                if (list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new Response(false, "There are no statistics."));
                }

                foreach (long l in list)
                {
                    months.Add(new DateTime(l));
                }
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, new Response(true, "There are no statistics."));
            }
            catch (Exception ex)
            {
                string message = $"List of month stats cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, months));
        }

        [HttpGet("day/{y}/{m}/{d}")]
        public async Task<IActionResult> GetDayStats(ushort y, ushort m, ushort d)
        {
            DayStats? stats;
            try
            {
                stats = await _context.DayStats.FindAsync(new DateTime(y, m, d).Ticks);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for day {y}/{m}/{d} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            if (stats == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response(false, "There are no statistics."));
            }

            return Ok(new Response(true, null, stats));
        }

        [HttpGet("day/all")]
        public async Task<IActionResult> GetAllDayStats()
        {
            List<long>? list;
            List<DateTime>? days = new();
            try
            {
                list = await _context.DayStats.Select(x => x.Date).ToListAsync();
                if (list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new Response(false, "There are no statistics."));
                }

                foreach (long l in list)
                {
                    days.Add(new DateTime(l));
                }
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, new Response(true, "There are no statistics."));
            }
            catch (Exception ex)
            {
                string message = $"List of day stats cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, days));
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductStats(uint id)
        {
            ProductStats? stats;
            try
            {
                stats = await _context.ProductStats.FindAsync(id);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for product {id} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            if (stats == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response(false, "There are no statistics."));
            }

            return Ok(new Response(true, null, stats));
        }
    }
}