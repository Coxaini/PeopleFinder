using PeopleFinder.Domain.Common.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Domain.Repositories;

public interface IRelationshipRepository : IRepo<Relationship>
{
    Task<PagedList<Relationship>> GetByProfileIdAsync(int profileId, PagedPaginationParams pagedPaginationParams);
    /// <summary>
    /// Gets approved or pending relationship between two profiles
    /// </summary>
    Task<Relationship?> GetRelationshipByProfileIdsAsync(int firstProfileId, int secondProfileId);
    
    Task DeleteFriendshipByProfileIds(int firstProfileId, int secondProfileId);
    Task<bool> IsProfilesFriends(int firstProfileId, int secondProfileId);
    
    Task<IEnumerable<Relationship>> GetRequestsWithoutAnswer(int profileId);
    Task<CursorList<Relationship>> GetRequestsWithoutAnswer(int profileId, int limit, DateTime? after);
    Task <int> GetRequestsWithoutAnswerCount(int profileId);
    Task<PagedList<Relationship>> GetRequestsWithoutAnswer(int profileId, PagedPaginationParams pagedPaginationParams);
    Task DeclineFriendRequest(int profileId, int senderId);
    Task<Relationship?> GetRequest(int senderId, int receiverId);
    Task<Relationship?> GetSentRequest(int requesterId, int receiverId);

}