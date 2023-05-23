namespace PeopleFinder.Contracts.Notifications;

public record EditMessageNotification(
    Guid ChatId,
    Guid MessageId ,
    string Text,
    DateTime EditedAt,
    int? InReplyTo,
    string? AttachmentUrl,
    DateTime? UpdatedAt
);