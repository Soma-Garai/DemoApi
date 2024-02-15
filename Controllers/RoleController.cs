using DemoApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        //Create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var newRole = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }
            return BadRequest("Failed to create role.");
        }

        //See the created role
        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                return Ok(role);
            }
            return NotFound("Role not found.");
        }

        //Update role
        [HttpPut("{roleName}")]
        public async Task<IActionResult> UpdateRole(string roleName, [FromBody] string newRoleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                role.Name = newRoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Ok("Role updated successfully.");
                }
                return BadRequest("Failed to update role.");
            }
            return NotFound("Role not found.");
        }

        //Delete role
        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Ok("Role deleted successfully.");
                }
                return BadRequest("Failed to delete role.");
            }
            return NotFound("Role not found.");
        }


    }
}
