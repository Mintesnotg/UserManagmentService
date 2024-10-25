using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface ITokenGeneretor
    {
        public string GenerateJwtToken(string email);
        public SecurityToken Dycrypt(string token, string secrate);

    }
}
