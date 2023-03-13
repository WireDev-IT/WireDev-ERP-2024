using System;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<StatsController> _logger;

        public StatsController(ApplicationDataDbContext context, ILogger<StatsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Retrieves a list of all total statistics</summary>
        /// <response code="200">Retrieving total stats succeded</response>
        /// <response code="204">There are no statistics</response>
        /// <response code="500">Oops! List of total statistics cannot be retrieved</response>
        /// <returns>A List of TotalStats</returns>
        [HttpGet("total"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetTotalStats()
        {
            List<TotalStats>? stats;
            try
            {
                stats = await _context.TotalStats.ToListAsync();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
            }
            catch (Exception ex)
            {
                string message = $"List of total statistics cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            if (stats.Count == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
            }

            return Ok(stats);
        }

        /// <summary>Get the stats of a year by its number.</summary>
        /// <param name="i">The year</param>
        /// <response code="200">Retrieving total stats succeded</response>
        /// <response code="404">Year not found</response>
        /// <response code="500">Oops! Year statistics cannot be retrieved</response>
        /// <returns>A YearStats object</returns>
        [HttpGet("year/{i}"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetYearStats(ushort i)
        {
            YearStats? stats;
            try
            {
                stats = await _context.YearStats.FirstAsync(x => x.Date == new DateTime(i, 1, 1).Ticks);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Statistics for year {i} not found!";
                _logger.LogError(ex, message);
                return NotFound(message);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for year {i} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(stats);
        }

        /// <summary>Retrieves a list of all total statistics</summary>
        /// <response code="200">Retrieving total stats succeded</response>
        /// <response code="204">There are no statistics</response>
        /// <response code="500">Oops! List of total statistics cannot be retrieved</response>
        /// <returns>A List of YearStats</returns>
        [HttpGet("year/all"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetAllYearStats()
        {
            List<long>? list;
            List<ushort>? years = new();
            try
            {
                list = await _context.YearStats.Select(x => x.Date).ToListAsync();
                if (list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
                }

                foreach (long l in list)
                {
                    years.Add((ushort)new DateTime(l).Year);
                }
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
            }
            catch (Exception ex)
            {
                string message = $"List of year stats cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(years);
        }

        /// <summary>Get the stats of a month by its number and year.</summary>
        /// <param name="y">The year</param>
        /// <param name="m">The month</param>
        /// <response code="200">Retrieving month stats succeded</response>
        /// <response code="404">Month not found</response>
        /// <response code="500">Oops! Year statistics cannot be retrieved</response>
        /// <returns>A YearStats object</returns>
        [HttpGet("month/{y}/{m}"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetMonthStats(ushort y, ushort m)
        {
            MonthStats? stats;
            try
            {
                stats = await _context.MonthStats.FirstAsync(x => x.Date == new DateTime(y, m, 1).Ticks);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"There are no statistics.";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status404NotFound, message);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for month {y}/{m} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(stats);
        }

        /// <summary>Gets a list of all months from query.</summary>
        /// <param name="minYear">Minium Year</param>
        /// <param name="maxYear">Maximum Year</param>
        /// <response code="200">Retrieving month stats succeded</response>
        /// <response code="204">There are no statistics</response>
        /// <response code="500">Oops! List of month statistics cannot be retrieved</response>
        /// <returns>A List of MonthStats</returns>
        [HttpGet("month/all"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetAllMonthStats([FromQuery] ushort minYear = ushort.MinValue, [FromQuery] ushort maxYear = ushort.MaxValue)
        {
            List<long>? list;
            List<DateTime>? months = new();
            try
            {
                list = await _context.MonthStats.Select(x => x.Date).Where(y => new DateTime(y).Year >= minYear && new DateTime(y).Year <= maxYear).ToListAsync();
                if (list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
                }

                foreach (long l in list)
                {
                    months.Add(new DateTime(l));
                }
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
            }
            catch (Exception ex)
            {
                string message = $"List of month stats cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(months);
        }

        /// <summary>Get the stats of a day by its number, month and year.</summary>
        /// <param name="y">The year</param>
        /// <param name="m">The month</param>
        /// <param name="d">The day</param>
        /// <response code="200">Retrieving day stats succeded</response>
        /// <response code="404">Day not found</response>
        /// <response code="500">Oops! Year statistics cannot be retrieved</response>
        /// <returns>A DayStats object</returns>
        [HttpGet("day/{y}/{m}/{d}"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetDayStats(ushort y, ushort m, ushort d)
        {
            DayStats? stats;
            try
            {
                stats = await _context.DayStats.FirstAsync(x => x.Date == new DateTime(y, m, d).Ticks);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"There are no statistics.";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status404NotFound, message);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for day {y}/{m}/{d} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(stats);
        }

        /// <summary>Gets a list of all days from query.</summary>
        /// <param name="minYear">Minium Year</param>
        /// <param name="maxYear">Maximum Year</param>
        /// <param name="minMonth">Minium Month</param>
        /// <param name="maxMonth">Maximum Month</param>
        /// <response code="200">Retrieving day stats succeded</response>
        /// <response code="204">There are no statistics</response>
        /// <response code="500">Oops! List of day statistics cannot be retrieved</response>
        /// <returns>A List of DayStats</returns>
        [HttpGet("day/all"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetAllDayStats([FromQuery] ushort minYear = ushort.MinValue, [FromQuery] ushort maxYear = ushort.MaxValue, [FromQuery] ushort minMonth = ushort.MinValue, [FromQuery] ushort maxMonth = ushort.MaxValue)
        {
            List<long>? list;
            List<DateTime>? days = new();
            try
            {
                list = await _context.DayStats
                    .Select(x => x.Date)
                    .Where(y => new DateTime(y).Year >= minYear && new DateTime(y).Year <= maxYear)
                    .Where(m => new DateTime(m).Month >= minMonth && new DateTime(m).Month <= maxMonth)
                    .ToListAsync();

                if (list.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
                }

                foreach (long l in list)
                {
                    days.Add(new DateTime(l));
                }
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status204NoContent, "There are no statistics.");
            }
            catch (Exception ex)
            {
                string message = $"List of day stats cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(days);
        }

        /// <summary>Get the stats of a product by its id.</summary>
        /// <param name="id">The product id</param>
        /// <response code="200">Retrieving product stats succeded</response>
        /// <response code="404">Product stats not found</response>
        /// <response code="500">Oops! Year statistics cannot be retrieved</response>
        /// <returns>A ProductStats object</returns>
        [HttpGet("product/{id}"), Authorize(Roles = "Analyst")]
        public async Task<IActionResult> GetProductStats(uint id)
        {
            ProductStats? stats;
            try
            {
                stats = await _context.ProductStats.FirstAsync(x => x.ProductId == id);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"There are no statistics.";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status404NotFound, message);
            }
            catch (Exception ex)
            {
                string message = $"Statistics for product {id} cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(stats);
        }
    }
}