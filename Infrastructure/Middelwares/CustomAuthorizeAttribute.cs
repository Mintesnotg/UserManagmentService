using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Filters;

namespace Infrastructure.Middelwares
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorization _authorization;


        public CustomAuthorizeAttribute(ApplicationDbContext dbContext, IAuthorization authorization)
        {
            _context=dbContext;
            _authorization =authorization;
        }


        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {


            bool hasAllowAnonymous = filterContext.ActionDescriptor.EndpointMetadata
           .Any(em => em is AllowAnonymousAttribute);

            if (hasAllowAnonymous) return;
            if (filterContext != null && filterContext?.ActionDescriptor is ControllerActionDescriptor descriptor) {
                string actionController = $"{descriptor.ControllerName}-{descriptor.ActionName}";

               
                var authHeader = filterContext.HttpContext.Request.Headers["Authorization"].ToString();
                var actionPrivilegies = filterContext.HttpContext.Request.Headers["ActionPrivilegies"].ToString();
                if (authHeader != null)
                {
                    var token = authHeader.Replace("Bearer ", "");
                    var claims = _authorization.GetClaim(token);
                    if (claims != null && claims.Count() > 0)
                    {

                    }
                    else
                    {
                        filterContext.Result = new UnauthorizedResult();

                    }

                    //var claims = _authorizationService.GetClaim(token);

                }
                else
                {
                    filterContext.Result = new UnauthorizedResult();

                }
            }

        }
    }
}
