using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
    //[Authorize("CATEGORIES:RO,CATEGORIES:RW")]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ProductDbContext context, ILogger<CategoryController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCategories()
        {
            List<Category>? list;
            try
            {
                list = await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"List of categories cannot be retrieved!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, list));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            Category? category;
            try
            {
                category = await _context.Categories.FirstAsync(x => x.Uuid == id);
                return Ok(new Response(true, null, category));
            }
            catch (Exception ex)
            {
                string message = $"Category with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return NotFound(new Response(true, message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns>The UUID of the added category.</returns>
        //[Authorize("CATEGORIES:RW")]
        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody][Required(ErrorMessage = "To add a category, you have to provide one.")] Category category)
        {
            try
            {
                _ = await _context.Categories.AddAsync(category);
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
                string message = $"Add category {category.Uuid} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, category.Uuid));
        }

        //[Authorize("CATEGORIES:RW")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyCategory(Guid id, [FromBody][Required(ErrorMessage = "To modify a category, you have to provide changes.")] Category category)
        {
            Category? c;
            try
            {
                c = await _context.Categories.FirstAsync(x => x.Uuid == id);
                c = category;
                _ = _context.Update(category);
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
                string message = $"Category with the UUID {id} was not found!";
                _logger.LogError(message, ex);
                return NotFound(new Response(false, message));
            }
            catch (Exception ex)
            {
                string message = $"Edit category {id} failed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, category));
        }

        //[Authorize("CATEGORIES:RW")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            Category? c;
            try
            {
                c = await _context.Categories.FirstAsync(x => x.Uuid == id);
                _ = _context.Categories.Remove(c);
                _ = _context.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Category with the UUID {id} was not found!";
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
                string message = $"Category with the UUID {id} could not be removed!";
                _logger.LogError(message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, "Category was removed."));
        }
    }
}

