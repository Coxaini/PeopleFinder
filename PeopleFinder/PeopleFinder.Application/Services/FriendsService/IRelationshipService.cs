using FluentResults;
using PeopleFinder.Application.Models.Friend;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Application.Services.FriendsService;

public interface IRelationshipService
{
  //  Task<Result> RateProfile(int userId, RateProfileRequest rateProfileRequest);
    
    Task<Result<Relationship>> SendFriendRequest(int profileId, FriendshipRequest friendshipRequest);
    
    Task<Result> ApproveFriendRequest(int profileId, int friendSenderId);
    
    Task<Result> RejectFriendRequest(int profileId, int friendSenderId);
    
    Task<Result> RemoveFriend(int profileId, int friendId);
    
    Task<Result<CursorList<FriendProfile>>> GetFriends(int profileId, CursorPaginationParams<DateTime> cursorPaginationParams);
    
    Task<Result<CursorList<Relationship>>> GetFriendshipRequestUpdates(int profileId, CursorPaginationParams<DateTime> pagedPaginationParams);
    
    Task<Result<CursorList<FriendProfile>>> GetMutualFriends(int requesterProfileId, int otherProfileId, CursorPaginationParams<DateTime> cursorPaginationParams);
}