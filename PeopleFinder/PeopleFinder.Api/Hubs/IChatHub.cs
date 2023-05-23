using PeopleFinder.Contracts.Notifications;

namespace PeopleFinder.Api.Hubs;

public interface IChatHub
{
    Task MessageSent (MessageNotification notification);
    Task MessageDeleted (DeleteMessageNotification notification);
    Task MessageEdited (MessageNotification notification);
    Task DirectChatCreated (ChatCreatedNotification notification);
    Task ChatDeleted (ChatDeletedNotification notification);
    
}