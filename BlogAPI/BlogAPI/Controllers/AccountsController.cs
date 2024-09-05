using BlogDAL.DTO;
using BlogDAL.Models;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public AccountsController(UserManager<User> _userManager, RoleManager<IdentityRole<int>> _roleManager, IMapper _mapper)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO == null)
            {
                return BadRequest();
            }
            var user = userRegisterDTO.Adapt<User>();
            user.UserName = userRegisterDTO.Email;
            var result = await userManager.CreateAsync(user, userRegisterDTO.PasswordHash);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            if (userRegisterDTO.Role == Role.Admin)
            {
                var role = await roleManager.FindByNameAsync("Admin");
                if (role == null)
                {
                    role = new IdentityRole<int>("Admin");
                    await roleManager.CreateAsync(role);
                }
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else if (userRegisterDTO.Role == Role.User)
            {
                var role = await roleManager.FindByNameAsync("User");
                if (role == null)
                {
                    role = new IdentityRole<int>("User");
                    await roleManager.CreateAsync(role);
                }
                await userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                var role = await roleManager.FindByNameAsync("Author");
                if (role == null)
                {
                    role = new IdentityRole<int>("Author");
                    await roleManager.CreateAsync(role);
                }
                await userManager.AddToRoleAsync(user, "Author");
            }
            return StatusCode(201);

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var roles = await userManager.GetRolesAsync(user);

            var result = new
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = roles[0]
            };
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userManager.Users
                       .Select(x => new
                       {
                           x.Id,
                           x.Email,
                           x.FullName
                       })
                       .ToListAsync();
            return Ok(users);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }


    }
}
