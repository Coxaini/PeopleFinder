using PeopleFinder.Application.Models.Authentication;
using PeopleFinder.Application.Services.Authentication;

namespace PeopleFinder.Application.Common.Interfaces.Authentication;

public interface IRefreshTokenGenerator
{
    public RefreshToken GenerateRefreshToken();
}