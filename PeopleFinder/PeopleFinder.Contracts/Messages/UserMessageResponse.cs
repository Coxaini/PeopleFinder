namespace PeopleFinder.Contracts.Messages;

public record UserMessageResponse(
    Guid Id,
    int SenderId, 
    string Text,
    DateTime SentAt,
    DateTime? EditedAt,
    string? AttachmentUrl,
    string? AttachmentType,
    string? AttachmentName,
    string DisplayName,
    string AvatarUrl,
    bool IsMine
);