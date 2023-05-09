
using PeopleFinder.Api.Notifications;

namespace PeopleFinder.Api.Hubs;

public interface IChatHub
{
    Task MessageSent (SendMessageNotification notification);
    Task MessageDeleted (DeleteMessageNotification notification);
    Task DirectChatCreated (DirectChatCreatedNotification notification);
    Task ChatDeleted (ChatDeletedNotification notification);
    
}