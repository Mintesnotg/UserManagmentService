using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagmentService.Models;

namespace Infrastructure.Services
{
    public class AuthorizationService : IAuthorization
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenGeneretor _tokenGeneretor;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public AuthorizationService(ApplicationDbContext  context , IHttpContextAccessor httpContext, IConfiguration configuration, ITokenGeneretor tokenGeneretor)
        {
            _context = context;
            _httpContext= httpContext;
            _configuration = configuration;
            _tokenGeneretor = tokenGeneretor;
        }

        public IEnumerable<Claim> GetClaim(string token)
        {

            var securityToken = _tokenGeneretor.Dycrypt(token,_configuration["Jwt:Key"]);
            if (securityToken != null)
            {
                var identity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claims = identity.Claims;

                return claims;
            }

            return null;
        }

        public bool IsAuthenticated(string token)
        {
            var securityToken = _tokenGeneretor.Dycrypt(token, _configuration["Jwt:Key"]);
            if (securityToken != null)
            {

                if (securityToken != null && securityToken.ValidTo != DateTime.MinValue && securityToken.ValidTo > DateTime.UtcNow) return true;
                //var userTokenInfo = _userTokenRepository.FirstOrDefault(t => t.AccessToken == token, [nameof(User)]);
                //if (userTokenInfo != null)
                //    return true;

            }
            return false;
        }

        public bool IsAuthorized(string username, string action)
        {

            //var test = _context.UserRoleMappings.Where(a => a.UserId == username).ToList();
            var roles = _context.UserRoleMappings.Where(a => a.UserId == username).Select(r=>r.RoleId).ToList();
            if (roles.Count >0 )
            {
                var userprevilage = _context.RolePrivileges.Include(i => i.Privilege).Where(a => roles.Contains(a.RoleId)).ToList();

                foreach (var privilege in userprevilage)
                {
                    if (string.Equals(action, privilege.Privilege.Action, StringComparison.OrdinalIgnoreCase))
                        return true;

                    
                }
                return false;
            } else
            {
                return false;
            }


        }
    }
}
