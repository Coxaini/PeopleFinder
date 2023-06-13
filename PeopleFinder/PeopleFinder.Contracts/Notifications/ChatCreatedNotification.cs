namespace PeopleFinder.Contracts.Notifications;

public record ChatCreatedNotification(
    Guid ChatId,
    string ChatType,
    string DisplayName,
    string DisplayImageAvatarUrl,
    string LastMessage,
    DateTime CreatedAt);