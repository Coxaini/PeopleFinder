using FluentResults;

namespace PeopleFinder.Application.Common.Errors;

public static class FriendRequestErrors
{
    public static readonly IError FriendRequestReceiverNotFound = new BaseError("Friend request receiver not found", ErrorType.NotFound);
    public static readonly IError FriendRequestAlreadySent = new BaseError("Friend request already sent", ErrorType.Conflict);
    public static readonly IError FriendRequestNotFound = new BaseError("Friend request not found", ErrorType.NotFound);

    public static readonly IError FriendRequestSenderNotFound =
        new BaseError("Friend request sender not found", ErrorType.NotFound);

    public static readonly IError SelfFriendshipError = new BaseError("Can`t send friend request to yourself", ErrorType.Conflict);
    public static readonly IError FriendshipNotFound = new BaseError("Friendship not found", ErrorType.NotFound);
    
}