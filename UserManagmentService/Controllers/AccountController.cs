using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using UserManagmentService.Models;

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenGeneretor _tokenGeneretor;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<User> userManager, ITokenGeneretor tokenGeneretor, SignInManager<User> signIn, RoleManager<UserRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signIn;
            _context = context;
            _tokenGeneretor=tokenGeneretor;
        }

       
        [HttpPost]
        [Route(nameof(Register))]
        [AllowAnonymous]
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
   
        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<IActionResult> Login( [FromBody] UserLoginDto userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userLogin.Password))
                return Unauthorized(new { message = "Invalid email or password." });
            if (user.Email != null)
                return Ok(_tokenGeneretor.GenerateJwtToken(user.Id));
            else return BadRequest("User Email is not found");
            

        }
        [HttpPost (nameof (ResetPassword))]
        public async Task<IActionResult> ResetPassword([FromBody]  ResetPasswordDto userLogin)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            // Reset the password using the token
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, userLogin.Token, userLogin.NewPassword);

            if (!resetPasswordResult.Succeeded)
            {
                return BadRequest(new { message = "Password reset failed.", errors = resetPasswordResult.Errors });
            }

            return Ok(new { message = "Password has been reset successfully." });
        }

        [HttpPost(nameof(RequestPasswordReset))]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto  requestPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(requestPassword.Email);
            if (user == null)
            {
                return BadRequest(new { message = "User not found." });
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new { token=resetToken, message = "Password reset link sent to your email." });

        }

    }
   
}
