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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GenereteTokenService : ITokenGeneretor
    {

        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        private readonly string jwtkey;
        public GenereteTokenService(IConfiguration configuration, ApplicationDbContext applicationDb)
        {
            _configuration= configuration;
            _context= applicationDb;
            jwtkey = _configuration["Jwt:Key"] ?? "";
        }

        public SecurityToken Dycrypt(string token, string secrate)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtkey);
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                SecurityToken securityToken;
                var claims = handler.ValidateToken(token, validations, out securityToken);
                return securityToken;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public string GenerateJwtToken(string userid)
        {

            try
            {

                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(jwtkey);
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
            catch (Exception)
            {

                throw;
            }

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {


            var tokenValidationParameters = new TokenValidationParameters
            {


                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtkey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
