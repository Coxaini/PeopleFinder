using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Services.Authorization
{
    public class AccessVerificationService : IAccessVerificationService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AccessVerificationService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool IsUserHasAccess(int userId)
        {
            int nameId = Convert.ToInt32(_contextAccessor.HttpContext
             ?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId != nameId)
                return false;

            return true;
        }

    }
}
