using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WireDev.Erp.V1.Api.Context;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Controllers
{
    [ApiController]
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

        /// <summary>Gets the ids of all existing groups</summary>
        /// <response code="200">List was generated</response>
        /// <response code="404">There are no groups</response>
        /// <response code="500">Oops! Getting the groups from database failed</response>
        /// <returns>A List of int</returns>
        [HttpGet("all"), Authorize(Roles = "Manager")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(list);
        }

        /// <summary>Gets an existing group</summary>
        /// <response code="200">Group was found</response>
        /// <response code="404">Group was not found</response>
        /// <response code="500">Oops! Getting the group from database failed</response>
        /// <param name="id">The id of the group object you want to get</param>
        /// <returns>A Group object</returns>
        [HttpGet("{id}"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetGroup(int id)
        {
            Group? group;
            try
            {
                group = await _context.Groups.FirstAsync(x => x.Uuid == id);
                return Ok(group);
            }
            catch (Exception ex)
            {
                string message = $"Group with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return NotFound(message);
            }
        }

        /// <summary>Adds a new group</summary>
        /// <response code="200">Group was added</response>
        /// <response code="409">A group with the same id already exists</response>
        /// <response code="500">Oops! Adding the group to database failed</response>
        /// <param name="group">The group object to add</param>
        /// <returns>The added group.</returns>
        [HttpPost("add"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddGroup([FromBody][Required(ErrorMessage = "To add a group, you have to provide one.")] Group group)
        {
            try
            {
                _ = await _context.Groups.AddAsync(group);
                _ = _context.SaveChanges(User?.Identity.Name);
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            catch (Exception ex)
            {
                string message = $"Add group {group.Uuid} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status201Created, group);
        }

        /// <summary>Modifies an existing group</summary>
        /// <response code="200">Group was modified</response>
        /// <response code="404">The group does not exists</response>
        /// <response code="423">The group is locked</response>
        /// <response code="500">Oops! Modifing the group in database failed</response>
        /// <param name="id">The id of the group</param>
        /// <param name="group">The group object with modifications</param>
        /// <returns>The modified group object</returns>
        //[Authorize("GROUPS:RW")]
        [HttpPut("{id}"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> ModifyGroup(int id, [FromBody][Required(ErrorMessage = "To modify a group, you have to provide changes.")] Group group)
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
                    _ = _context.SaveChanges(User?.Identity.Name);
                }
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                string message = $"Edit group {id} failed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(temp_group);
        }

        /// <summary>Deletes a group by its id</summary>
        /// <response code="200">Group was removed</response>
        /// <response code="404">The group does not exists</response>
        /// <response code="423">The group is locked</response>
        /// <response code="500">Oops! Deleting the group from database failed</response>
        /// <param name="id">The id of the group</param>
        [HttpDelete("{id}"), Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            Group? c;
            try
            {
                c = await _context.Groups.FirstAsync(x => x.Uuid == id);

                if (c.Used)
                {
                    throw new BadHttpRequestException($"Group {c.Uuid} is locked and can not be deleted! Try to archive it.");
                }

                _ = _context.Groups.Remove(c);
                _ = _context.SaveChanges(User?.Identity.Name);
            }
            catch (ArgumentNullException ex)
            {
                string message = $"Group with the UUID {id} was not found!";
                _logger.LogWarning(message, ex);
                return NotFound(message);
            }
            catch (DbUpdateException ex)
            {
                string message = $"Could not save changes to database!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            catch (BadHttpRequestException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                string message = $"Group with the UUID {id} could not be removed!";
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return Ok(null);
        }
    }
}