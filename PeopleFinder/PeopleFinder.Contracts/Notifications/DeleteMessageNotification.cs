namespace PeopleFinder.Api.Notifications;

public record DeleteMessageNotification(Guid ChatId, Guid MessageId);