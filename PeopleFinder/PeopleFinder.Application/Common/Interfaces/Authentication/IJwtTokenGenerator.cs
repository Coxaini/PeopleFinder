using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    { 
        string GenerateToken(User user, Profile profile);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
