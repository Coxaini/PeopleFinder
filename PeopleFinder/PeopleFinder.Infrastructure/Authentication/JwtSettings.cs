using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string Secret { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpiryMinutes { get; init; }   
        
        public int RefreshTokenExpiryMinutes { get; init; }
    }
}
