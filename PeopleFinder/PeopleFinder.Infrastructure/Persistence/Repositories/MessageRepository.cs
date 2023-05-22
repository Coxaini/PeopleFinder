using Microsoft.EntityFrameworkCore;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories;

public class MessageRepository : BaseRepo<Message>, IMessageRepository
{
    public MessageRepository(PeopleFinderDbContext context) : base(context)
    {
    }

    public async Task<Message?> GetWithDetailsById(Guid id)
    {
        return await Context.Messages
            .Include(m=>m.Chat)
            .Include(m=>m.AttachmentFile)
            .FirstOrDefaultAsync(m => m.Id == id);
        
    }
    
    public async Task<CursorList<UserMessage>> GetUserMessages(Guid chatId, int limit, DateTime? after, DateTime? before)
    {
        var query = Context.Messages
            .Where(m => m.ChatId == chatId);

        if (after is not null)
        {
            query = query.Where(m => m.SentAt <= after);
        }
        else if (before is not null)
        {
            query = query.Where(m => m.SentAt >= before);
        }

        var messages = await query
            .OrderByDescending(m => m.SentAt)
            .Take(limit+1)
            .Select(m=>
                new UserMessage()
                {
                    Id = m.Id,
                    ChatId = m.ChatId,
                    SenderId = m.SenderId,
                    SentAt = m.SentAt,
                    Text = m.Text,
                    AttachmentFileId = m.AttachmentFileId,
                    EditedAt = m.EditedAt,
                    DisplayName = m.Sender.Name,
                    AuthorAvatarId = m.Sender.MainPictureId,
                    AttachmentFileType = m.AttachmentFile!= null ? m.AttachmentFile.Type: null,

                })
            .ToListAsync();

        return new CursorList<UserMessage>(messages, limit);
    }

    public async Task<Message?> GetLastMessage(Guid chatId, Guid? exceptMessageId = null)
    {
        return await Context.Messages
            .Include(m=>m.Sender)
            .Where(m => m.ChatId == chatId)
            .Where(m => m.Id != exceptMessageId)
            .OrderByDescending(m => m.SentAt)
            .FirstOrDefaultAsync();
    }
}