using Microsoft.EntityFrameworkCore;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Infrastructure.Persistence.Common;

namespace PeopleFinder.Infrastructure.Persistence.Repositories;

public class ChatRepository : BaseRepo<Chat>, IChatRepository
{
    public ChatRepository(PeopleFinderDbContext context) : base(context)
    {
    }
    
    public async Task<CursorList<UserChat>> GetChatsAsync(int profileId, int limit, DateTime? after = null)
    {
        var chatsMembersQuery = Context.ChatMembers
            .AsNoTracking()
            .Where(cm => cm.ProfileId == profileId);
            
        
        if(after != null)
            chatsMembersQuery = chatsMembersQuery
                .Where(cm =>(cm.Chat.ChatType!= ChatType.Direct && (cm.Chat.LastMessageAt ?? cm.Chat.CreatedAt) <= after)
                || ( cm.Chat.LastMessageAt <= after));
        
        var chats = await chatsMembersQuery
            .OrderByDescending(cm => cm.Chat.LastMessageAt ?? cm.Chat.CreatedAt)
            .Select(cm=>
                new UserChat()
                {
                    Id = cm.Chat.Id,
                    DisplayTitle = cm.Chat.Title,
                    DisplayLogoId = cm.Chat.Logo != null ? cm.Chat.Logo.Id : null,
                    ChatType = cm.Chat.ChatType,
                    LastMessageAt = cm.Chat.LastMessageAt,
                    LastMessage = cm.Chat.LastMessage,
                    CreatedAt = cm.Chat.CreatedAt
                }
            )
            
            .Take(limit+1)
            .ToListAsync();
        
        var directChatsId = chats
            .Where(c=>c.ChatType== ChatType.Direct)
            .Select(c=>c.Id)
            .ToList(); 
        
        var friendsMembers = await Context.ChatMembers
            .Include(cm=>cm.Profile)
            .Where(cm => directChatsId.Contains(cm.ChatId))
            .Where(cm => cm.ProfileId != profileId)
            .ToListAsync();

        foreach (var chat in chats)
        {
            if(chat.ChatType is not ChatType.Direct)
                continue;
            
            var friend = friendsMembers.FirstOrDefault(f => f.ChatId == chat.Id)?.Profile;
            if (friend == null)
                continue;
            
            chat.DisplayTitle = friend.Name;
            chat.DisplayLogoId = friend.MainPictureId;
            chat.UniqueTitle = friend.Username;
            
        }
        
        
        return new CursorList<UserChat>(chats, limit);
    }

    public Task<bool> IsProfileInChatAsync(int profileId, Guid chatId)
    {
        return Context.ChatMembers
            .AsNoTracking()
            .AnyAsync(cm => cm.ProfileId == profileId && cm.ChatId == chatId);
    }
}