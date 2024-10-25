using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IAuthorization
    {
        bool IsAuthorized(string username, string action);
        bool IsAuthenticated(string token);
        IEnumerable<Claim> GetClaim(string token);
    }
}
