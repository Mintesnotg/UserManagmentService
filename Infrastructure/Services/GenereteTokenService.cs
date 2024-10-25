using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GenereteTokenService : ITokenGeneretor
    {

        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;
        public GenereteTokenService(IConfiguration configuration, ApplicationDbContext applicationDb)
        {
            _configuration= configuration;
            _context= applicationDb;
        }

        public SecurityToken Dycrypt(string token, string secrate)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
               
                ValidateAudience = false
            };

            try
            {
                SecurityToken securityToken = null;
                var claims = handler.ValidateToken(token, validations, out securityToken);
                return securityToken;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GenerateJwtToken(string userid)
        {

            try
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {

                   
                    Subject = new ClaimsIdentity(
                            [
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userid),
                        new Claim(JwtRegisteredClaimNames.Sid, userid),
                        new Claim(JwtRegisteredClaimNames.Jti,
                        
                            Guid.NewGuid().ToString())
                   
                ]
                    ),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Issuer = issuer,
                    Audience = audience,
                   
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
