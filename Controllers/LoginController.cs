using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DemoApi.DTO;
using DemoApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoApi.Controllers
{
	[ApiController]
	[Route("api/v1/auth")]
	public class LoginController : Controller
	{
		private readonly ILogger<LoginController> _logger;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly TokenGenerator _tokenGenerator;
        private RoleManager<IdentityRole> _roleManager;
        public LoginController(ILogger<LoginController> logger, UserManager<IdentityUser> userManager, TokenGenerator tokenGenerator, RoleManager<IdentityRole> roleManager)
		{
			_logger = logger;
			_userManager = userManager;
			_tokenGenerator = tokenGenerator;
			_roleManager = roleManager;
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] UsersDto usersDto)
		{
			var user = await _userManager.FindByNameAsync(usersDto.username);
			
			if(user == null)
			{
				return BadRequest("User Not Found");
			}
			
			var passwordResult = await _userManager.CheckPasswordAsync(user, usersDto.password);
			if(!passwordResult)
			{
				return BadRequest("Password Not Match");
			}
			
			var token = await _tokenGenerator.GenerateToken(user);
			
			return Ok(token);
		}

        [HttpPost("assignRole")]
        //[Route("assignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
        {
            // Find the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the role exists
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            // Check if the user already has the role
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                return BadRequest($"User '{user.UserName}' already has the role '{roleName}'.");
            }

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' assigned to user '{user.UserName}' successfully.");
            }
            return BadRequest($"Failed to assign role '{roleName}' to user '{user.UserName}'.");
        }

    }
}