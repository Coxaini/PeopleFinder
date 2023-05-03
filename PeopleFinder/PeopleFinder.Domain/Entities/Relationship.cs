
namespace PeopleFinder.Domain.Entities;


public enum RelationshipStatus
{
    Pending,
    Approved,
    Rejected,
    Ignored,
    BlockedByFirstProfile,
    BlockedBySecondProfile,
    BlockedByBothProfiles
}
public class Relationship
{
    public long Id { get; set; }
    public int InitiatorProfileId { get; set; }
    public Profile InitiatorProfile { get; set; } = null!;
    public int ReceiverProfileId { get; set; }
    public Profile ReceiverProfile { get; set; } = null!;
    public RelationshipStatus Status { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime? AcknowledgeAt { get; set; }

}