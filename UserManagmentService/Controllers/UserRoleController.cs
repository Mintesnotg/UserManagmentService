using Infrastructure.Appdbcontext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {

        private readonly    ApplicationDbContext _context;  

        public UserRoleController(ApplicationDbContext context)
        {
            _context=context;
        }




    }
}
