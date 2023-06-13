using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Domain.Repositories;

public interface IMessageRepository : IRepo<Message>
{
    Task<Message?> GetWithDetailsById(Guid id);
    
    Task<CursorList<UserMessage>> GetUserMessages(Guid chatId, int limit, DateTime? after, DateTime? before);
    Task<Message?> GetLastMessage(Guid chatId, Guid? exceptMessageId = null);
}