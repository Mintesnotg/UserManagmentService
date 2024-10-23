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


        public GenereteTokenService(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        public string GenerateJwtToken(string email)
        {

            try
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                            {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                        new Claim(JwtRegisteredClaimNames.Email, email),
                        new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                }),
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
