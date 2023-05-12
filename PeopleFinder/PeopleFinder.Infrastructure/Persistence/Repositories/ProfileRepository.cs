﻿using MediatR;
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
using PeopleFinder.Application.Common.Constants;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Common.Recommendation;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Infrastructure.Persistence.Common;

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
        
        public async Task<IEnumerable<ProfileWithMutualFriends>> GetRecommendedByMutualFriends(int profileId) //cache this
        {


            var mutualRecs = await Context.MutualFriendsRecommendations
                .FromSql(
$""" 
Select FFId as Id, Count (Profiles.Username) as MutualCount, STRING_AGG(Profiles.Username, ', ') Usernames
	from 
	(SELECT CASE
	WHEN f.InitiatorProfileId = {profileId} THEN f.ReceiverProfileId
	WHEN f.ReceiverProfileId = {profileId} THEN f.InitiatorProfileId
	END AS FriendId
	FROM Relationships f
	WHERE f.InitiatorProfileId = {profileId} OR f.ReceiverProfileId = {profileId}) as friends 
		cross apply (
				Select FFId from
				(SELECT CASE
				WHEN f1.InitiatorProfileId = FriendId THEN f1.ReceiverProfileId
				WHEN f1.ReceiverProfileId = FriendId THEN f1.InitiatorProfileId
				END AS FFId
				FROM Relationships f1
				WHERE f1.InitiatorProfileId = FriendId OR f1.ReceiverProfileId = FriendId) as friendsOfFriend
				where FFId != {profileId}) friendsOfFriend
		inner join Profiles On FriendId = Profiles.Id
	Group by FFId
	Order by MutualCount Desc
""").AsNoTracking().ToListAsync();

           var friendsOfFriends = await Context.Profiles
               .Where(p => mutualRecs.Select(r => r.Id).Contains(p.Id))
               .Include(p=>p.Tags)
               .ToListAsync();
            var recs = mutualRecs.Select(x =>
                new ProfileWithMutualFriends(friendsOfFriends.First(p=>p.Id == x.Id),
                    x.Usernames.Split(", ")));
            return recs;

        }

        public async Task<CursorList<FriendProfile>> GetMutualFriends(int requesterProfileId, int otherProfileId, int limit, DateTime? after = null) //cache this
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
                .Select(joined => new FriendProfile(joined.Profile, joined.Relationship))
                .Take(limit+1)
                .ToListAsync();

            var cursorList = new CursorList<FriendProfile>(result,limit, totalCount);
            
            return cursorList;
            
        }


        public async Task<int> GetFriendsCount(int profileId)
        {
            var query = Context.Relationships
                .Where(f=>f.Status == RelationshipStatus.Approved)
                .Where(f => f.InitiatorProfileId == profileId || f.ReceiverProfileId == profileId);
            return await query.CountAsync();
        }
        public async Task<CursorList<FriendProfile>> GetFriends(int profileId, int limit, DateTime? after)
        {
            var query = Context.Relationships
                .Where(f=>f.Status == RelationshipStatus.Approved)
                .Where(f => f.InitiatorProfileId == profileId || f.ReceiverProfileId == profileId);

            var totalCount = await query.CountAsync();
            
            
            if(after != null)
                query = query.Where(f => f.AcknowledgeAt <= after);

            var friends = await query.OrderByDescending(f => f.AcknowledgeAt)
                .Include(f => f.InitiatorProfile.Tags)
                .Include(f => f.ReceiverProfile.Tags)
                .AsSplitQuery()
                .Select(f => f.InitiatorProfileId == profileId
                    ? new FriendProfile(f.ReceiverProfile, f)
                    : new FriendProfile(f.InitiatorProfile, f))
                .Take(limit+1)
                .ToListAsync();
            
            
            

            var cursorList = new CursorList<FriendProfile>(friends,limit, totalCount);
            
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
    }
}
