using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PeopleFinder.Application.Common.Interfaces.Authentication;
using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Domain.Common.Constants;


namespace PeopleFinder.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtsettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
        {
            _jwtsettings = jwtOptions.Value;
        }
        public string GenerateToken(User user, Profile profile)
        {

            var signingCredentials = new SigningCredentials(new
                SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Secret)),
                SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim(ProjectJwtRegisteredClaimNames.UserId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, profile.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, profile.Username),
            };

            var securityToken = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtsettings.Issuer,
                audience: _jwtsettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtsettings.ExpiryMinutes),
                signingCredentials: signingCredentials
                );


            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_jwtsettings.Secret);
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            
            return principal;
        }
    }
}
