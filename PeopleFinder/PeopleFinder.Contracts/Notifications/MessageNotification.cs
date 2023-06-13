namespace PeopleFinder.Contracts.Notifications;

public record MessageNotification(
    Guid ChatId, 
    string ChatType,
    Guid Id,
    int SenderId, 
    string Text,
    DateTime SentAt,
    DateTime? EditedAt,
    string? AttachmentUrl,
    string? AttachmentType,
    string? AttachmentName,
    string DisplayName,
    string AvatarUrl);
    
    