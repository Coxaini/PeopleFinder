using FluentResults;
using PeopleFinder.Domain.Entities.MessagingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;

namespace PeopleFinder.Application.Services.ChatServices
{
    public interface IChatService
    {
        
        Task<Result<CursorList<UserChat>>> GetChats(int profileId, CursorPaginationParams<DateTime> paginationParams);
        /// <summary>
        /// Creates a direct chat between two profiles and sends a first message
        /// </summary>
        Task<Result<UserChat>> CreateDirectChat(CreateDirectChatRequest request);
        /// <summary>
        /// Gets a direct chat between two profiles
        /// </summary>
        Task<Result<UserChat>> GetDirectChat(int profileId, int friendId);
        
        Task<Result<UserChat>> GetChat(int profileId,Guid chatId);

        Task<Result<IList<Chat>>> GetAllChats(int profileId);

        /*Task<Result<Chat>> UpdateChat();
        Task<Result<Chat>> DeleteChat();#1#*/
    }
}
