namespace PeopleFinder.Api.Notifications;

public record SendMessageNotification(
    Guid ChatId, 
    string ChatType,
    int ProfileId,
    Guid MessageId ,
    string DisplayName,
    string DisplayImageAvatarUrl,
    string Text,
    int? InReplyTo,
    string? AttachmentUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt);