namespace PeopleFinder.Contracts.Notifications;

public record DeleteMessageNotification(
    Guid ChatId,
    Guid MessageId,
    bool IsLastMessage,
    string? NewLastMessage,
    DateTime? NewLastMessageAt,
    Guid? NewLastMessageId,
    string? NewLastMessageAuthorName
    );