using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test API is working");
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test API is working");
        }
    }
}
