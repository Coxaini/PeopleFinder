

using FluentResults;
using FluentValidation;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Common.Extensions;
using PeopleFinder.Application.Common.Helpers;
using PeopleFinder.Application.Common.Interfaces.Authentication;
using PeopleFinder.Application.Models.Authentication;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PeopleFinder.Domain.Common.Constants;

namespace PeopleFinder.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, 
            IValidator<LoginRequest> validator, IValidator<RegisterRequest> registerValidator,
            IUnitOfWork unitOfWork, ILogger<AuthenticationService> logger,
            IRefreshTokenGenerator refreshTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _loginValidator = validator;
            _registerValidator = registerValidator;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<Result<AuthenticationResult>> Login(LoginRequest request)
        {

            var result = await _loginValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return Result.Fail(result.ToErrorList());
            }
            
            var user = await _unitOfWork.UserRepository.GetByEmailOrLoginAsync(request.EmailOrUsername);
            if(user is null) {
                return AuthenticationErrors.UserNotFound;
            }
            
            var profile = await _unitOfWork.ProfileRepository.GetByUserIdAsync(user.Id);
            
            if(profile is null)
            {
                _logger.LogError("Profile with given user id doesn't exist in database");
                return AuthenticationErrors.UserNotFound;
            }
            
            if(user.Password != request.Password)
            {
                return AuthenticationErrors.IncorrectPassword;
            }
           
            var token = _jwtTokenGenerator.GenerateToken(user, profile);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpiryTime = refreshToken.ExpiryTime;
            
            await _unitOfWork.SaveAsync();
            
            return new AuthenticationResult(user, token, refreshToken.Token, refreshToken.ExpiryTime);

        }

        public async Task<Result<AuthenticationResult>> Register(RegisterRequest request)
        {

            var result = await _registerValidator.ValidateAsync(request);
            if(!result.IsValid)
            {
                return Result.Fail(result.ToErrorList());
            }

            bool emailExists = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email) is not null;
            bool loginExists = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username) is not null;
            
            if(emailExists && loginExists)
            {
                return Result.Fail(AuthenticationErrors.UserEmailAndLoginAlreadyExists);
            }
            if(emailExists)
                return AuthenticationErrors.UserEmailAlreadyExists;
            if(loginExists)
                return AuthenticationErrors.UserLoginAlreadyExists;
            

            Profile profile = new() {Name = String.Empty, Username = request.Username, Bio = String.Empty, City = String.Empty
                , CreatedAt = DateTime.Now, LastActivity = DateTime.Now};
            User user = new() { Email = request.Email, Password = request.Password, Profile = profile };
            

            await _unitOfWork.ProfileRepository.AddAsync(profile);
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();
            
            _logger.LogInformation("User {Login} was created", profile.Username);
            var token = _jwtTokenGenerator.GenerateToken(user, profile);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpiryTime = refreshToken.ExpiryTime;
            
            await _unitOfWork.SaveAsync();

            return new AuthenticationResult(user, token, refreshToken.Token, refreshToken.ExpiryTime);
        }
        
        public async Task<Result<TokenResult>> RefreshToken(RefreshTokenRequest request)
        {
            ClaimsPrincipal principal;
            
            try
            {
                principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.ExpiredToken);
            }
            catch (SecurityTokenException e)
            {
                return Result.Fail(AuthenticationErrors.InvalidToken);
            }

            int profileId = Convert.ToInt32(principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            int userId = Convert.ToInt32(principal.Claims.First(x => x.Type == ProjectJwtRegisteredClaimNames.UserId)?.Value);
            
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return AuthenticationErrors.UserNotFound;
            }
            if (user.RefreshToken != request.RefreshToken)
            {
                return Result.Fail(AuthenticationErrors.InvalidRefreshToken);
            }
            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                return Result.Fail(AuthenticationErrors.RefreshTokenExpired);
            }
            
            string newToken = _jwtTokenGenerator.GenerateToken(user, user.Profile!);

            return new TokenResult(newToken, user.RefreshToken, user.RefreshTokenExpiryTime);


        }

        public async Task<Result> CheckUsername(string username)
        {
            return await _unitOfWork.UserRepository.GetByUsernameAsync(username) is not null 
                ? Result.Fail(AuthenticationErrors.UserAlreadyExists) : Result.Ok();
        }

        public async Task<Result> CheckEmail(string email)
        {
            return await _unitOfWork.UserRepository.GetByEmailAsync(email) is not null 
                ? Result.Fail(AuthenticationErrors.UserAlreadyExists) : Result.Ok();
        }
    }
}
