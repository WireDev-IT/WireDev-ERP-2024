using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WireDev.Erp.V1.Models.Authentication;
using WireDev.Erp.V1.Models.Statistics;

namespace WireDev.Erp.V1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>Logs the user in</summary>
        /// <response code="200">Token was gernerated</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Database error</response>
        /// <returns>JWT</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> authClaims = new()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("scope", "weather.read"),
                    new Claim("scope", "weather.readwrite")
                };

                foreach (string? userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                JwtSecurityToken token = new(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return Unauthorized();
        }

        /// <summary>Creates a new user</summary>
        /// <response code="200">Created</response>
        /// <response code="500">Database error</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            IdentityUser userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User already exists!");
            }

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            return !result.Succeeded
                ? StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.")
                : Ok("User created successfully!");
        }

        /// <summary>Creates a new admin-user</summary>
        /// <response code="200">Created</response>
        /// <response code="500">Database error or weak password</response>
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            IdentityUser userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User already exists!");
            }

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                _ = await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                _ = await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                _ = await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok("User created successfully!");
        }
    }
}
