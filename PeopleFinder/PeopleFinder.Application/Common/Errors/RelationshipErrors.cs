using FluentResults;

namespace PeopleFinder.Application.Common.Errors;

public static class RelationshipErrors
{
    public static readonly IError ProfileIsNotYourFriend = new BaseError("Profile is not your friend", ErrorType.AccessDenied);
    
    public static readonly IError FriendNotFound = new BaseError("Friend not found", ErrorType.NotFound);
    
}