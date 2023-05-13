namespace PeopleFinder.Contracts.Notifications;

public record EditMessageNotification(
    Guid ChatId,
    Guid MessageId ,
    string Text,
    int? InReplyTo,
    string? AttachmentUrl,
    DateTime? UpdatedAt
);