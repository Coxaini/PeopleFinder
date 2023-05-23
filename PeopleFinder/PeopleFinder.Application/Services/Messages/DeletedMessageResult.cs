using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.Messages;

public class DeletedMessageResult
{
    public Guid ChatId { get; init; }
    public Guid MessageId { get; init; }
    
    public bool IsLastMessage { get;  set; }
    public string? NewLastMessage { get; private set; } = null!;
    public DateTime? NewLastMessageAt { get; private set; }
    public string? NewLastMessageAuthorName { get; private set; } = null!;
    
    public void SetNewLastMessage(Message newLastMessage)
    {
        NewLastMessage = newLastMessage.Text;
        NewLastMessageAt = newLastMessage.SentAt;
        NewLastMessageAuthorName = newLastMessage.Sender.Name;
    }
}