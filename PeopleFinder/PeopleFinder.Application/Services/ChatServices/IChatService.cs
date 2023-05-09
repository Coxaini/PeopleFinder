using FluentResults;
using PeopleFinder.Domain.Entities.MessagingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;

namespace PeopleFinder.Application.Services.ChatServices
{
    public interface IChatService
    {
        
        Task<Result<CursorList<UserChat>>> GetChats(int profileId, CursorPaginationParams<DateTime> paginationParams);
        /// <summary>
        /// Creates a direct chat between two profiles and send a first message
        /// </summary>
        Task<Result<Chat>> CreateDirectChat(CreateDirectChatRequest request);
        Task<Result<Message>> SendMessage(SendMessageRequest request);
        
        /*Task<Result<Chat>> UpdateChat();
        Task<Result<Chat>> DeleteChat();#1#*/
    }
}
