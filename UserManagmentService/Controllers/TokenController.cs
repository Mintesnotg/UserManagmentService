using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagmentService.Models;

namespace UserManagmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {


        private readonly ITokenGeneretor _tokenService;
        private readonly UserManager<User> _userManager;

        private readonly ApplicationDbContext _context;
        public TokenController(ITokenGeneretor tokenGeneretor , UserManager<User> userManager , ApplicationDbContext applicationDb)
        {
            _tokenService = tokenGeneretor;
            _userManager = userManager;
            _context = applicationDb;
        }



        [HttpPost]
        [Route(nameof(RefreshToken))]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
           var userid = principal.Claims.ToList()[2].Value;
            //var username = principal?.Identity?.Name; //this is mapped to the Name claim by default
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userid);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GenerateJwtToken(user?.Email);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

    }
}
