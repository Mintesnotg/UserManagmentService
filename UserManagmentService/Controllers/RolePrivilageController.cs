using Infrastructure.Appdbcontext;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePrivilageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RolePrivilageController(ApplicationDbContext context)
        {
            _context=context;
        }


        [HttpPost]
        [Route(nameof(AssignPrivilegesToRole))]
        public async Task<IActionResult> AssignPrivilegesToRole([FromBody] AssignPrivilegesToRoleDto model)
        {
            // Validate the input
            if (model == null || string.IsNullOrEmpty(model.RoleId) || model.PrivilegeIds == null || !model.PrivilegeIds.Any())
            {
                return BadRequest("Invalid input data.");
            }

            // Get the role from the database
            var role = await _context.UserRoles.Include(r => r.RolePrivileges)
                                               .FirstOrDefaultAsync(r => r.Id == model.RoleId);
          
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            // Fetch all privileges from the PrivilegeIds list
            var privileges = await _context.Privileges
                                           .Where(p => model.PrivilegeIds.Contains(p.Id))
                                           .ToListAsync();

            if (privileges.Count != model.PrivilegeIds.Count)
            {
                return BadRequest("Some of the privileges are invalid.");
            }

            // Clear any existing privileges for the role (if needed)
            _context.RolePrivileges.RemoveRange(role.RolePrivileges);

            // Assign new privileges to the role
            foreach (var privilege in privileges)
            {
                var rolePrivilege = new RolePrivilege
                {
                    RoleId = role.Id,
                    PrivilegeId = privilege.Id
                };
                _context.RolePrivileges.Add(rolePrivilege);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "Privileges assigned successfully to the role." });
        }


    }
}
