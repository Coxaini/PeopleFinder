using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Contracts.Chats;

public record UserChatResponse(
    Guid Id,
    string DisplayTitle,
    string? UniqueTitle,
    string DisplayLogoUrl,
    string ChatType,
    Guid? LastMessageId,
    DateTime? LastMessageAt,
    string? LastMessage,
    string? LastMessageAuthorName,
    DateTime CreatedAt);