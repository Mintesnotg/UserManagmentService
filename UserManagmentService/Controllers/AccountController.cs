using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserManagmentService.Models;

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;

        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<User> userManager, RoleManager<UserRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
  
            _context = context;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)

        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var defaultRole = await _roleManager.Roles.FirstOrDefaultAsync();
            if (defaultRole == null)
            {
                return BadRequest(new { message = "Default role does not exist." });
            }
            var user = new User
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Create the user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Create the user role mapping
                var userRoleMapping = new UserRoleMapping
                {
                    UserId = user.Id,
                    RoleId = defaultRole.Id,
                    AssignedBy = "System",
                    AssignDate = DateTime.Now
                };
                _context.UserRoleMappings.Add(userRoleMapping);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                // Rollback transaction if any error occurs
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "An error occurred while registering the user", error = ex.Message });
            }
        }

    }
}
