namespace PeopleFinder.Api.Notifications;

public record DirectChatCreatedNotification(Guid ChatId, Guid FistMessageId ,string Text, DateTime CreatedAt, string? AttachmentUrl );