using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Infrastructure.Authentication.Requirements
{
    public class UserIdFromRouteHandler : AuthorizationHandler<UserIdFromRouteRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIdFromRouteRequirement requirement)
        {
            if (context.Resource is HttpContext mvcContext)
            {
                var userIdInClaims = Convert.ToInt32(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var userIdInRoute = Convert.ToInt32(mvcContext.Request.RouteValues["userId"]);
                if (userIdInClaims == userIdInRoute)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }
    }

}
