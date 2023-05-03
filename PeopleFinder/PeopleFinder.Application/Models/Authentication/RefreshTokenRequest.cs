namespace PeopleFinder.Application.Models.Authentication;

public record RefreshTokenRequest(string ExpiredToken, string RefreshToken);