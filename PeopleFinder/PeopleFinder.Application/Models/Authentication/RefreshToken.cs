namespace PeopleFinder.Application.Models.Authentication;

public record RefreshToken(string Token = "", DateTime ExpiryTime = default);