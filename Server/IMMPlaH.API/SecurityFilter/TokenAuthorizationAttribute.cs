using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using IMMPlaH.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMMPlaH.API.SecurityFilter
{
    public class TokenAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public RoleEnum Role { get; set; }

        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (token == "null" || token == "")
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
            else
            {
                var tokenObject = new JwtSecurityToken(token);
                var userId = tokenObject.Payload[ClaimTypes.Sid].ToString();
                if (userId == null)
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                }
                else if (Role == RoleEnum.Doctor && userId != "1")
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                }
                else if (Role == RoleEnum.Caregiver && userId != "2")
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                }
                else if (Role == RoleEnum.Patient && userId != "3")
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                }
            }
        }
    }
}
