using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;

namespace PeopleFinder.Domain.Repositories
{
    public interface IChatRepository : IRepo<Chat>
    {
        Task<CursorList<UserChat>> GetChatsAsync(int profileId, int limit, DateTime? after = null);

        Task<bool> IsProfileInChatAsync(int profileId, Guid chatId);

    }
}
