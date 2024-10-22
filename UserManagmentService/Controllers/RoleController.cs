using Infrastructure.Dtos;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<UserRole> _roleManager;

        public RoleController(RoleManager<UserRole> roleManager)
        {
            _roleManager=roleManager;
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
            {
                return BadRequest(ModelState);
            }

            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound(new { message = "Role not found" });
            }
            existingRole.Name = role.RoleName;
            existingRole.Description = role.Description;

            var result = await _roleManager.UpdateAsync(existingRole);
            if (result.Succeeded)
            {
                return Ok(existingRole);
            }
            return BadRequest(result.Errors);

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
