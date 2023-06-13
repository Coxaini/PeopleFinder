namespace PeopleFinder.Contracts.Messages;

public record MessageResponse(Guid Id, int SenderId , string Text ,DateTime SentAt, DateTime? EditedAt , string? AttachmentUrl);