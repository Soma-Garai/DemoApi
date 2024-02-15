using DemoApi.Data;
using DemoApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController (UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Result);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult CreateUser([FromBody] UsersDto usersdto)
        {
            var appUser = new IdentityUser
            {
                UserName = usersdto.username,
                Email = usersdto.email
            };
            var result= _userManager.CreateAsync(appUser, usersdto.password);
            if(result.Result.Succeeded)  //if a new user is created
            {
                //check if input role is in the database
                //var roleExists = _userManager.IsInRoleAsync(appUser, usersdto.Role);

                if (usersdto.Role !=null) //if role is entered in input
                {
                    // Assign the input role to the user 
                    var roleresult = _userManager.AddToRoleAsync(appUser, usersdto.Role);
                    if (roleresult.Result.Succeeded)
                    {
                        return Ok("New user created and role assigned succesfully");
                    }
                        
                }
                //ROLE is not assigned then, assign user a role as an USER
                var USERroleresult = _userManager.AddToRoleAsync(appUser, "User");
                if (USERroleresult.Result.Succeeded)
                {
                    return Ok("New user created and role as a USER assigned succesfully");
                }
            }
            return BadRequest("User not created");
                    
        } 

        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, [FromBody] UsersDto dto)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user.Result == null)
            {
                return BadRequest("User not updated");
            }
            var appuser = user.Result;
            appuser.UserName=dto.username; 
            appuser.Email=dto.email;
            appuser.EmailConfirmed = true;
            
            var result = _userManager.UpdateAsync(appuser);
            if(result.Result.Succeeded)
            {
                return Ok("User updated");
            }
            return BadRequest("Failed to update user");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user.Result == null)
            {
                return NotFound();
            }
            var result= _userManager.DeleteAsync(user.Result);
            if (result.Result.Succeeded)
            {
                return Ok("User Deleted");
            }
            return BadRequest("Failed to delete user");

        }




    }
}
