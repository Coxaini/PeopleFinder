using PeopleFinder.Contracts.Notifications;

namespace PeopleFinder.Api.Hubs;

public interface IChatHub
{
    Task MessageSent (SendMessageNotification notification);
    Task MessageDeleted (DeleteMessageNotification notification);
    Task MessageEdited (EditMessageNotification notification);
    Task DirectChatCreated (ChatCreatedNotification notification);
    Task ChatDeleted (ChatDeletedNotification notification);
    
}