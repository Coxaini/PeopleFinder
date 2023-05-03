using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Application.Models.Relationship;

public record RelationshipResult(long Id, int InitiatorProfileId, int ReceiverProfileId, RelationshipStatus Status, DateTime SentAt, DateTime? AcknowledgeAt);