namespace PeopleFinder.Contracts.Notifications;

public record SendMessageNotification(
    Guid ChatId, 
    string ChatType,
    int SenderId,
    Guid MessageId ,
    string DisplayName,
    string DisplayImageAvatarUrl,
    string Text,
    int? InReplyTo,
    string? AttachmentUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt);