using System.ComponentModel.DataAnnotations;

namespace PeopleFinder.Domain.Entities.MessagingEntities;


public enum MemberRole
{
    Member,
    Admin
}
public class ChatMember
{
    public long ChatId { get; set; }
    public int ProfileId { get; set; }
    public Chat? Chat { get; set; } = null!;
    public Profile? Profile { get; set; } = null!;
    
    [DataType(DataType.DateTime)]
    public  DateTime JoinedAt { get; set; }
    public MemberRole Role { get; set; }
    
}