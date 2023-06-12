
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Models.Authentication;
using PeopleFinder.Application.Services.Authentication;
using PeopleFinder.Contracts.Authentication;

namespace PeopleFinder.Api.Controllers
{
    [Route("auth")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;


        public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var authResult = await _authenticationService.Register(request);

            return authResult.Match( 
                 user =>
                 {
                     SetRefreshToken(user.RefreshToken, user.RefreshTokenExpiryTime);
                     SetAccessToken(user.Token);
                     return Ok(_mapper.Map<AuthenticationResponse>(user));
                 },
                 Problem);

          
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            var authResult = await _authenticationService.Login(request);
            
            return authResult.Match(
                user =>
                {
                    SetRefreshToken(user.RefreshToken, user.RefreshTokenExpiryTime);
                    SetAccessToken(user.Token);
                    return Ok(_mapper.Map<AuthenticationResponse>(user));
                },
                (err)=>
                {
                    
                    return Problem(err);
                });

        }
        
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            
            string? refreshToken = Request.Cookies["refreshToken"];
            if(string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token is missing");
            }

            string? accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            if(string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized("Access token is missing");
            }
            
            var request = new RefreshTokenRequest(accessToken,refreshToken);
            var tokenResult = await _authenticationService.RefreshToken(request);
           
            return tokenResult.Match(
                
                 tokens =>
                 {
                     SetAccessToken(tokens.Token);
                     return Ok("Token refreshed successfully");
                 },
                 Problem);
        }

        [HttpGet("check_username/{username:required}")]
        public async Task<IActionResult> CheckUsername([FromRoute]string username)
        {
            var result = await _authenticationService.CheckUsername(username);
            return result.Match(
                Ok,
                Problem);
        }
        
        [HttpGet("check_email/{email:required}")]
        public async Task<IActionResult> CheckEmail([FromRoute]string email)
        {
            var result = await _authenticationService.CheckEmail(email);
            return result.Match(
                Ok,
                Problem);
        }
        
        private void SetAccessToken(string accessToken)
        {
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromDays(1),
               Secure = true
            });
        }

        private void SetRefreshToken(string refreshToken, DateTime expiryTime)
        {
            CookieOptions options = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiryTime,
                SameSite = SameSiteMode.None,
                Secure = true
            };
            Response.Cookies.Append("refreshToken", refreshToken, options);
        }
    }
}
