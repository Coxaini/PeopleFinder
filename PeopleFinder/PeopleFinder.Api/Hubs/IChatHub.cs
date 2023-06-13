using PeopleFinder.Contracts.Notifications;

namespace PeopleFinder.Api.Hubs;

public interface IChatHub
{
    Task MessageSent (MessageNotification notification);
    Task MessageDeleted (DeleteMessageNotification notification);
    Task MessageEdited (MessageNotification notification);
    Task DirectChatCreated (Guid chatId);
    Task UserOnline (string username);
    Task UserOffline (string username, DateTime lastSeenAt);
    Task ChatDeleted (Guid chatId);
    
}