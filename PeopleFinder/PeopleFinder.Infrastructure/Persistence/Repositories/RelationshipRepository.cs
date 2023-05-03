using Microsoft.EntityFrameworkCore;
using PeopleFinder.Domain.Common.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Domain.Repositories.Common;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories;

public class RelationshipRepository : BaseRepo<Relationship>, IRelationshipRepository
{
    public RelationshipRepository(PeopleFinderDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Relationship>> GetByProfileIdAsync(int profileId, PagedPaginationParams pagedPaginationParams)
    {
        var friendships = await PagedList<Relationship>
            .ToPagedListAsync(Context.Relationships
                .Where(f => f.InitiatorProfileId == profileId || f.ReceiverProfileId == profileId)
                .OrderByDescending(f => f.AcknowledgeAt)
                , pagedPaginationParams.PageNumber, pagedPaginationParams.PageSize);
        
        return friendships;
    }

    
    public Task<Relationship?> GetRelationshipByProfileIdsAsync(int firstProfileId, int secondProfileId)
    {
        return Context.Relationships
            .Where(f=>f.Status == RelationshipStatus.Pending || f.Status == RelationshipStatus.Approved)
            .FirstOrDefaultAsync(f => (f.InitiatorProfileId == firstProfileId && f.ReceiverProfileId == secondProfileId) ||
                                      (f.InitiatorProfileId == secondProfileId && f.ReceiverProfileId == firstProfileId));
    }

    public void DeleteFriendshipByProfileIds(int firstProfileId, int secondProfileId)
    {
        var friendship = Context.Relationships
            .FirstOrDefault(f => (f.InitiatorProfileId == firstProfileId && f.ReceiverProfileId == secondProfileId) ||
                                 (f.InitiatorProfileId == secondProfileId && f.ReceiverProfileId == firstProfileId));
        if (friendship != null)
        {
            Context.Relationships.Remove(friendship);
        }
    }
    
    private IQueryable<Relationship> GetRequestsWithoutAnswerQuery(int profileId)
    {
        return Context.Relationships
            .Where(r => r.Status == RelationshipStatus.Pending)
            .Where(r => r.ReceiverProfileId == profileId)
            .OrderByDescending(r => r.SentAt)
            .Include(r => r.InitiatorProfile)
            .ThenInclude(p => p.Tags);
    }
    public async Task<IEnumerable<Relationship>> GetRequestsWithoutAnswer(int profileId)
    {
        //await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        
        var newRequests = await GetRequestsWithoutAnswerQuery(profileId).ToListAsync();

        return newRequests;
    }

    public async Task<CursorList<Relationship>> GetRequestsWithoutAnswer(int profileId, int limit, DateTime? after)
    {
        var query = Context.Relationships
            .Where(r => r.Status == RelationshipStatus.Pending)
            .Where(r => r.ReceiverProfileId == profileId);

        int totalCount = await query.CountAsync();
        
        if (after != null)
            query = query.Where(r => r.SentAt >= after);

        var requests = await query
            .OrderByDescending(r => r.SentAt)
            .Take(limit+1)
            .Include(r => r.InitiatorProfile)
            .ThenInclude(p => p.Tags)
            .ToListAsync();
    

        
        var cursorList = new CursorList<Relationship>(requests,limit, totalCount);
        
        return cursorList;

    }

    public async Task<int> GetRequestsWithoutAnswerCount(int profileId)
    {
        return await Context.Relationships
            .Where(r => r.Status == RelationshipStatus.Pending)
            .Where(r => r.ReceiverProfileId == profileId).CountAsync();
    }

    public async Task<PagedList<Relationship>> GetRequestsWithoutAnswer(int profileId, PagedPaginationParams pagedPaginationParams)
    {
        var newRequests = await PagedList<Relationship>
            .ToPagedListAsync(GetRequestsWithoutAnswerQuery(profileId), pagedPaginationParams.PageNumber, pagedPaginationParams.PageSize);
        return newRequests;
    }

    public async void DeclineFriendRequest(int profileId, int senderId)
    {
        throw new NotImplementedException();
    }

    public async Task<Relationship?> GetRequest(int senderId, int receiverId)
    {
        return await Context.Relationships
            .Where(f=>f.Status == RelationshipStatus.Pending)
            .FirstOrDefaultAsync(r => r.InitiatorProfileId == senderId && r.ReceiverProfileId == receiverId);
    }
    
    
}