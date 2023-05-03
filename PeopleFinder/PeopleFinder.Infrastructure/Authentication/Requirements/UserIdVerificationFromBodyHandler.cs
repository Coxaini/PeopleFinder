using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PeopleFinder.Infrastructure.Authentication.Requirements
{
    public class UserIdVerificationFromBodyHandler : AuthorizationHandler<UserIdVerificationFromBodyRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            UserIdVerificationFromBodyRequirement requirement)
        {

            if (context.Resource is HttpContext mvcContext)
            {
                if (await HasAccess(mvcContext))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }

        private async Task<bool> HasAccess(HttpContext context)
        {
            var userId = Convert.ToInt32(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var request = context.Request;
            request.EnableBuffering();
            request.Body.Position = 0;
            string result;
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                result = await reader.ReadToEndAsync();
            }
            request.Body.Position = 0;
            try
            {
                var body = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result);

                if (body == null)
                    return false;

                JsonElement requestUserId;
                if (!body.TryGetValue("userId", out requestUserId))
                    return false;

                int jId = requestUserId.GetInt32();

                if (jId == userId)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;

        }
    }
}
