using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Filters;

namespace Infrastructure.Middelwares
{
    public class AuthorizationMiddlWare : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {


            throw new NotImplementedException();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {


            throw new NotImplementedException();
        }
    }
}
