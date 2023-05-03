using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;
using PeopleFinder.Domain.Common.Recommendation;

namespace PeopleFinder.Domain.Repositories
{
    public interface IProfileRepository : IRepo<Profile>
    {
        public Task<Profile?> GetWithTagsByUserIdAsync(int userId);
        public Task<Profile?> GetByUsernameWithTagsAsync(string username);
        public Task<Profile?> GetByUserIdAsync(int userId);
        public Task<Profile?> GetWithTagsByIdAsync(int id);
        public Task<Profile?> GetByIdAsync(int id);
        public Task<IEnumerable<ProfileWithMutualFriends>> GetRecommendedByMutualFriends(int profileId);
        
        public Task<CursorList<FriendProfile>> GetMutualFriends(int requesterProfileId, int otherProfileId,int limit, DateTime? after= null);
        public Task<CursorList<FriendProfile>> GetFriends(int profileId,int limit, DateTime? after);
        public Task<int> GetFriendsCount(int profileId);
        public Task<PagedList<Profile>> GetFriends(int profileId, PagedPaginationParams pagedPaginationParams);
        
    }
}
