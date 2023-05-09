namespace PeopleFinder.Application.Services.ChatServices;

public record ChatResult(Guid ChatId, string DisplayName, string DisplayImageAvatarUrl, string LastMessageText,
    DateTime LastMessageCreatedAt, int UnreadMessagesCount);