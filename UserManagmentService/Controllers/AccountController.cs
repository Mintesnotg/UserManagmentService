using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Infrastructure.Dtos;
using Infrastructure.Models;
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

        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signIn, RoleManager<UserRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signIn;
            _context = context;
        }


        [HttpPost]
        [Route(nameof(Register))]
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
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, userLogin.Password))
                return Unauthorized(new { message = "Invalid email or password." });


            var roles = await _context.UserRoles.Where(a => a.Id == user.Id).ToListAsync();
            var prev = new List<Privilege>();

            foreach (var item in roles)
            {
                var privilege = await _context.RolePrivileges.Where(a => a.RoleId == item.Id).FirstOrDefaultAsync();
                prev.Add(privilege.Privilege);
            }

            var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email)
         };

            //foreach (var rol in roles)
            //{
            //    claims.Add(new Claim("Role", rol));
            //}

            //// Add privilege claims (assuming privileges are just strings like "Read", "Write", etc.)
            //foreach (var pri in prev)
            //{
            //    claims.Add(new Claim("Privilege", pri));
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UserserviceApi"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            
            var token = new JwtSecurityToken(
              issuer: "yourissuer",
              audience: "youraudience",
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds
          );

            return new JsonResult("");
            //_userManager.s
        }
      
    }
   
}
