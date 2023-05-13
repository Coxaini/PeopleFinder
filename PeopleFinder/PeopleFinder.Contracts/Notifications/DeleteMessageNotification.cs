namespace PeopleFinder.Contracts.Notifications;

public record DeleteMessageNotification(Guid ChatId, Guid MessageId);