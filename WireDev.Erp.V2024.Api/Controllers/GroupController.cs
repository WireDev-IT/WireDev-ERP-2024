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
    //[Authorize("GROUPS:RO")]
    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        private readonly ApplicationDataDbContext _context;
        private readonly ILogger<GroupController> _logger;

        public GroupController(ApplicationDataDbContext context, ILogger<GroupController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //[Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        public async Task<IActionResult> GetGroups()
        {
            List<int>? list;
            try
            {
                list = await _context.Groups.Select(x => x.Uuid).ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"List of groups cannot be retrieved!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, list));
        }

        //[Authorize("GROUPS:RO")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(uint id)
        {
            Group? group;
            try
            {
                group = await _context.Groups.FirstAsync(x => x.Uuid == id);
                return Ok(new Response(true, null, group));
            }
            catch (Exception ex)
            {
                string message = $"Group with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return NotFound(new Response(true, message));
            }
        }

        //[Authorize("GROUPS:RW")]
        [HttpPost("add")]
        public async Task<IActionResult> AddGroup([FromBody][Required(ErrorMessage = "To add a group, you have to provide one.")] Group group)
        {
            try
            {
                _ = await _context.Groups.AddAsync(group);
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
                string message = $"Add group {group.Uuid} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, group.Uuid));
        }

        //[Authorize("GROUPS:RW")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyGroup(uint id, [FromBody][Required(ErrorMessage = "To modify a group, you have to provide changes.")] Group group)
        {
            Group? temp_group;
            try
            {
                temp_group = await _context.Groups.FirstAsync(x => x.Uuid == id);
                if (temp_group == null)
                {
                    throw new ArgumentNullException($"Group {id} was not found!");
                }
                else
                {
                    await temp_group.ModifyProperties(group);
                    _ = _context.Groups.Update(temp_group);
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
            catch (Exception ex)
            {
                string message = $"Edit group {id} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, null, temp_group));
        }

        //[Authorize("GROUPS:RW")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(uint id)
        {
            Group? c;
            try
            {
                c = await _context.Groups.FirstAsync(x => x.Uuid == id);

                if (c.Used) throw new BadHttpRequestException($"Group {c.Uuid} is locked and can not be deleted! Try to archive it.");

                _ = _context.Groups.Remove(c);
                _ = _context.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Group with the UUID {id} was not found!";
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
                string message = $"Group with the UUID {id} could not be removed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(false, message));
            }

            return Ok(new Response(true, "Group was removed."));
        }
    }
}