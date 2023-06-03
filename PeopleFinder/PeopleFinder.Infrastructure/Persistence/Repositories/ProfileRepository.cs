using MediatR;
using Microsoft.EntityFrameworkCore;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Internal;
using PeopleFinder.Application.Common.Constants;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Common.Recommendation;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Infrastructure.Persistence.Common;
using PeopleFinder.Infrastructure.Persistence.Common.Extensions;

namespace PeopleFinder.Infrastructure.Persistence.Repositories
{
    public class ProfileRepository : BaseRepo<Profile>, IProfileRepository
    {
        
        public ProfileRepository(PeopleFinderDbContext context) : base(context)
        {
            
        }

        public async Task<Profile?> GetWithTagsByUserIdAsync(int userId)
        {

                return await Context.Profiles
                    .Where((x) => x.UserId == userId)
                    .AsSplitQuery()
                    .Include(x => x.Tags)
                    .FirstOrDefaultAsync();
        }

        public async Task<Profile?> GetByUsernameWithTagsAsync(string username)
        {
            return await Context.Profiles
                .Where((x) => x.Username == username)
                .AsSplitQuery()
                .Include(x => x.Tags)
                .FirstOrDefaultAsync();
            
        }

        public async Task<Profile?> GetByUserIdAsync(int userId)
        {
            return await Context.Profiles
                .Where((x) => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<Profile?> GetWithTagsByIdAsync(int id)
        {
            return await Context.Profiles
                .Where((x) => x.Id == id)
                .AsSplitQuery()
                .Include(x => x.Tags)
                .FirstOrDefaultAsync();
        }

        public async Task<Profile?> GetByIdAsync(int id)
        {
            return await Context.Profiles.FindAsync(id);
        }

        public async Task<IList<Profile>> GetRecommendedByTags(Profile profile, int limit)
        {

            var recs = await Context.Profiles
                .Where(x => x.City == profile.City)
                .Where(x => x.Id != profile.Id)
                .Where(x => !Context.Relationships
                    .Any(r => (r.InitiatorProfileId == profile.Id && x.Id == r.ReceiverProfileId)
                              || (r.InitiatorProfileId == x.Id && r.ReceiverProfileId == profile.Id)))
                .OrderByDescending(x => x.IsOnline)
                .ThenByDescending(x => x.LastActivity.Date)
                .Take(50)
                .OrderByTagsIntersection(profile.Tags)
                .Take(limit)
                .Include(x => x.Tags)
                .ToListAsync();
            

            return recs;

        }

        public async Task<PagedList<ProfileWithMutualFriends>> GetRecommendedByMutualFriends(int profileId, int page =1 , int pageSize = 10)
        {
            var mutualRecsQuery = Context.MutualFriendsRecommendations
                .FromSql(
$""" 
select m.Id, Max(m.MutualCount) as MutualCount, STRING_AGG(m.Username, ', ') Usernames from 
(
Select FFId as Id, Count(Profiles.Id) Over(Partition by FFId) as MutualCount,
	ROW_NUMBER() Over (Partition by FFId order by Profiles.LastActivity ASC) as rowN , Profiles.Username as Username
	from 
	(SELECT CASE
	WHEN f.InitiatorProfileId = {profileId} THEN f.ReceiverProfileId
	WHEN f.ReceiverProfileId =  {profileId} THEN f.InitiatorProfileId
	END AS FriendId
	FROM Relationships f
	WHERE (f.InitiatorProfileId = {profileId} OR f.ReceiverProfileId = {profileId}) AND f.Status = 1) as friends 
		cross apply (
				Select FFId from
				(SELECT CASE
				WHEN f1.InitiatorProfileId = FriendId THEN f1.ReceiverProfileId
				WHEN f1.ReceiverProfileId = FriendId THEN f1.InitiatorProfileId
				END AS FFId
				FROM Relationships f1
				WHERE (f1.InitiatorProfileId = FriendId OR f1.ReceiverProfileId = FriendId) AND f1.Status = 1 
				) as friendsOfFriend
				where FFId !=  {profileId} AND NOT EXISTS 
				(SELECT * FROM Relationships f2 
				WHERE (f2.InitiatorProfileId = {profileId} AND f2.ReceiverProfileId = FFId) 
				OR (f2.ReceiverProfileId = {profileId} AND f2.InitiatorProfileId = FFId))
				) friendsOfFriend
		inner join Profiles On FriendId = Profiles.Id
		
) as m
where m.rowN < 3
Group by m.Id
""");
            
            int count = await mutualRecsQuery.CountAsync();
            
            var mutualRecs = await mutualRecsQuery
                .OrderByDescending(x=>x.MutualCount)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

           var friendsOfFriends = await Context.Profiles
               .Where(p => mutualRecs.Select(r => r.Id).Contains(p.Id))
               .Include(p=>p.Tags)
               .ToListAsync();
            var recs = mutualRecs.Select(x =>
                new ProfileWithMutualFriends(friendsOfFriends.First(p=>p.Id == x.Id),
                    x.Usernames.Split(", "), x.MutualCount)).ToList();
            
            
            return new PagedList<ProfileWithMutualFriends>(recs, count, page, pageSize);

        }

        public async Task<CursorList<RelationshipProfile>> GetMutualFriends(int requesterProfileId, int otherProfileId, int limit, DateTime? after = null) //cache this
        {

            var profiles = Context.Relationships
                .Where(f => f.InitiatorProfileId == requesterProfileId || f.ReceiverProfileId == requesterProfileId)
                .Where(f=>f.Status == RelationshipStatus.Approved)
                .Select(f => f.InitiatorProfileId == requesterProfileId ? f.ReceiverProfileId : f.InitiatorProfileId)
                .Intersect(
                    Context.Relationships
                        .Where(f => f.InitiatorProfileId == otherProfileId || f.ReceiverProfileId == otherProfileId)
                        .Where(f=>f.Status == RelationshipStatus.Approved)
                        .Select(f =>
                            f.InitiatorProfileId == otherProfileId ? f.ReceiverProfileId : f.InitiatorProfileId)
                )
                .Join(
                    Context.Profiles,
                    mutualf => mutualf,
                    profile => profile.Id,
                    (mutualf, profile) => profile
                );

            int totalCount = await profiles.CountAsync();
            
            var query = profiles
                .Include(p=>p.Tags)
                .Join(
                    Context.Relationships,
                    profile => profile.Id,
                    relationship => relationship.ReceiverProfileId == requesterProfileId ? relationship.InitiatorProfileId : relationship.ReceiverProfileId,
                    (profile, relationship) => new { Profile = profile, Relationship = relationship }
                ) 
                .Where(joined => 
                    (joined.Relationship.InitiatorProfileId == requesterProfileId && joined.Relationship.ReceiverProfileId == joined.Profile.Id)
                    || (joined.Relationship.InitiatorProfileId == joined.Profile.Id && joined.Relationship.ReceiverProfileId == requesterProfileId)
                );

            if (after != null)
            {
                query = query.Where(joined => joined.Relationship.AcknowledgeAt <= after);
            }
                
            var result = await query.OrderByDescending(joined => joined.Relationship.AcknowledgeAt)
                .Select(joined => new RelationshipProfile(joined.Profile, joined.Relationship))
                .Take(limit+1)
                .ToListAsync();

            var cursorList = new CursorList<RelationshipProfile>(result,limit, totalCount);
            
            return cursorList;
            
        }


        public async Task<int> GetFriendsCount(int profileId)
        {
            var query = Context.Relationships
                .Where(f=>f.Status == RelationshipStatus.Approved)
                .Where(f => f.InitiatorProfileId == profileId || f.ReceiverProfileId == profileId);
            return await query.CountAsync();
        }
        public async Task<CursorList<RelationshipProfile>> GetFriends(int profileId, int limit, DateTime? after, string? searchQuery = null)
        {
            var query = Context.Relationships
                .Where(f => f.Status == RelationshipStatus.Approved)
                .Where(f => f.InitiatorProfileId == profileId || f.ReceiverProfileId == profileId);
            
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(f =>
                    (f.InitiatorProfileId == profileId && (f.ReceiverProfile.Name.Contains(searchQuery) || f.ReceiverProfile.Username.Contains(searchQuery))) ||
                    (f.ReceiverProfileId == profileId && (f.InitiatorProfile.Name.Contains(searchQuery) || f.InitiatorProfile.Username.Contains(searchQuery)))
                );
            }
            
            var totalCount = await query.CountAsync();
            
            if(after != null)
                query = query.Where(f => f.AcknowledgeAt <= after);
            
            var friends = await query.OrderByDescending(f => f.AcknowledgeAt)
               // .Include(f => f.InitiatorProfile.Tags)
               // .Include(f => f.ReceiverProfile.Tags)
               // .AsSplitQuery()
                .Select(f => f.InitiatorProfileId == profileId
                    ? new RelationshipProfile(f.ReceiverProfile, f)
                    : new RelationshipProfile(f.InitiatorProfile, f))
                .Take(limit+1)
                .ToListAsync();
            
            var cursorList = new CursorList<RelationshipProfile>(friends,limit, totalCount);
            
            return cursorList;
        }

        public async Task<PagedList<Profile>> GetFriends(int profileId, PagedPaginationParams pagedPaginationParams)
        {
            var friends = await PagedList<Profile>
                .ToPagedListAsync(Context.Relationships
                        .Where(f => f.InitiatorProfileId == profileId || f.ReceiverProfileId == profileId)
                        .OrderByDescending(f => f.AcknowledgeAt)
                        .Include(f=>f.InitiatorProfile.Tags)
                        .Include(f=>f.ReceiverProfile.Tags)
                        .Select(f=>f.InitiatorProfileId == profileId ? f.ReceiverProfile : f.InitiatorProfile)
                        
                    , pagedPaginationParams.PageNumber, pagedPaginationParams.PageSize);
            
            return friends;
        }

        public async Task<CursorList<RelationshipProfile>> GetProfilesByFilter(int profileId, int limit, DateTime? after, string searchQuery)
        {
            var query = Context.Profiles
                .Include(p=>p.Tags)
                .AsSplitQuery()
                .Where(p => p.Id != profileId)
                .Where(p => p.Name.Contains(searchQuery) || p.Username.Contains(searchQuery));

            var joinedQuery = query
                .GroupJoin(Context.Relationships,
                    profile => profile.Id,
                    r => r.ReceiverProfileId == profileId ? r.InitiatorProfileId :
                        r.InitiatorProfileId == profileId ? r.ReceiverProfileId : (int?)null,
                    (profile, relationship) => new { Profile = profile, Relationship = relationship })
                .SelectMany(x => x.Relationship.DefaultIfEmpty(),
                    (x, relationship) => new { x.Profile, Relationship = relationship });
            

            if (after != null)
            {
                joinedQuery = joinedQuery.Where(joined => joined.Profile.LastActivity <= after);
            }
                
            var result = await joinedQuery.OrderByDescending(joined => joined.Profile.LastActivity)
                .Select(joined => new RelationshipProfile(joined.Profile, joined.Relationship))
                .Take(limit+1)
                .ToListAsync();

            var cursorList = new CursorList<RelationshipProfile>(result,limit);
            
            return cursorList;
        }
    }
}
