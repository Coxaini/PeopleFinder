using FluentResults;
using PeopleFinder.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticationResult>> Login(LoginRequest request);
        Task<Result<AuthenticationResult>> Register(RegisterRequest request);
        Task<Result<TokenResult>> RefreshToken(RefreshTokenRequest request);
        
        Task<Result> CheckUsername(string username);
        Task<Result> CheckEmail(string email);
       
    }
}
