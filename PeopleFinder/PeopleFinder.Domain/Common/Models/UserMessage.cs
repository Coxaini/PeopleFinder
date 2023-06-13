using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Domain.Common.Models;

public class UserMessage
{
    public Guid Id { get; init; }
    public Guid ChatId { get; set; }
    public ChatType ChatType { get; set; }
    public int SenderId { get; set; }
    public string Text { get; set; } 
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public string DisplayName { get; set; }
    public Guid? AuthorAvatarId { get; set; }
    public int? InReplyTo { get; set; }
    public Guid? AttachmentFileId { get; set; }
    public MediaFileType? AttachmentFileType { get; set; }
    public string? AttachmentName { get; set; }
    public bool IsMine { get; set; }
}