namespace PeopleFinder.Application.Services.Authentication;

public record TokenResult(string Token, string RefreshToken, DateTime? RefreshTokenExpiryTime);