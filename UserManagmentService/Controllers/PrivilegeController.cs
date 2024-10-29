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
    public class PrivilegeController : ControllerBase
    {


        readonly ApplicationDbContext _context;
        public PrivilegeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut ]
        [Route(nameof(AssignRoletoPrivilages))]
        public async Task<IActionResult> AssignRoletoPrivilages(RolePrivlilegeDto rolePrivlilegeDto) {

            var userrole = await _context.Roles.Include(r => r.RolePrivileges).FirstOrDefaultAsync(a => a.Id == rolePrivlilegeDto.RoleId);
            if (userrole == null) return BadRequest(new JsonResult("Role Does not exist !"));
            else
            {

                var existingprivileges = userrole.RolePrivileges.Select(a => a.PrivilegeId).ToList();
                rolePrivlilegeDto.PrivilegeIds = rolePrivlilegeDto.PrivilegeIds.Where(s => !existingprivileges.Contains(s)).ToList();
                var newRolePrivileges = rolePrivlilegeDto.PrivilegeIds.Select(privilegeId => new RolePrivilege
                {
                    PrivilegeId = privilegeId,
                    RoleId = userrole.Id
                }).ToList();
                if (newRolePrivileges.Count >0)
                    await _context.RolePrivileges.AddRangeAsync(newRolePrivileges);
                    if (await _context.SaveChangesAsync() > 0) return Ok(new JsonResult("Privilege is Assigned to Role"));
                else return BadRequest("User role is alredy assigned");
            }
           
        }




    }
}
