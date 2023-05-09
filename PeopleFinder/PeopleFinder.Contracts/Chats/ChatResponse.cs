using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Contracts.Chats;

public record ChatResponse(Guid Id, string DisplayTitle,
    string? UniqueTitle, string DisplayLogoUrl, string ChatType,
    DateTime? LastMessageAt, string? LastMessage, DateTime CreatedAt);