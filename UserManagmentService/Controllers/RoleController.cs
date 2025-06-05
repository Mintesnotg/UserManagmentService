using Infrastructure.Appdbcontext;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagmentService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<UserRole> _roleManager;
        private readonly UserManager<User>  _userManager;

        private readonly ApplicationDbContext _context;

        public RoleController(RoleManager<UserRole> roleManager, UserManager<User> userManager , ApplicationDbContext context)
        {
            _roleManager=roleManager;
            _userManager=userManager;
            _context = context;
        }
        // GET: api/<RoleController>
        [HttpGet]
        public  IActionResult GetRoles()
        {
           var allroles= _roleManager.Roles.ToList();
            return Ok(allroles);
        }

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            return Ok(role);
        }

        // POST api/<RoleController>
        [HttpPost(nameof(CreateRole))]
        public async Task<IActionResult> CreateRole([FromBody] UserRoleDto role)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userrole = new UserRole
            {
                Name = role.RoleName,
                Description = role.Description,
                RegisteredDate = DateTime.Now,
                RoleName=role.RoleName
            };

            var result = await _roleManager.CreateAsync(userrole);
            if (result.Succeeded)
            {
                return Ok(role);
            }

            return BadRequest(result.Errors);
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public async Task< IActionResult> UpdateRole(string id, [FromBody] UserRoleDto role)
        {

            if (!ModelState.IsValid || id != role.Id)
                return BadRequest(ModelState);
            

            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
                return NotFound(new { message = "Role not found" });
            
            existingRole.Name = role.RoleName;
            existingRole.Description = role.Description;

            var result = await _roleManager.UpdateAsync(existingRole);
            if (result.Succeeded)
                return Ok(existingRole);
            
            return BadRequest(result.Errors);

        }        
        
        [HttpPut (nameof(UpdateUserRole))]
        public async Task< IActionResult> UpdateUserRole([FromBody] UserRoleMappingDto  userRole, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                if (!ModelState.IsValid || userRole.UserId != userRole.UserId)
                {
                    return BadRequest(ModelState);
                }
                var rolemapping = new List<UserRoleMapping>();
                var user = await _context.UserRoleMappings.Where(a => a.UserId == userRole.UserId).ToListAsync();
                if (user.Count == 0)
                    return NotFound(new { message = "User is not found" });
                else
                    rolemapping = userRole.UserRoleId.Select(role => new UserRoleMapping
                    {
                        RoleId = role,
                        UserId = userRole.UserId,
                        AssignDate = DateTime.Now
                    }).ToList();


                _context.RemoveRange(user);
                _context.AddRange(rolemapping);
                if (await _context.SaveChangesAsync(cancellationToken) > 0)
                {
                    await transaction.CommitAsync();
                    return new JsonResult("User Role is Updated");

                }
                else return StatusCode(500, new { message = "An error occurred while registering the user" });


            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
               return StatusCode(500, new { message = "An error occurred while registering the user" });

                throw;
            }








        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role deleted successfully" });
            }

            return BadRequest(result.Errors);

        }
    }
}
