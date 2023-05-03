using PeopleFinder.Application.Models.Relationship;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Mappers.Extensions;

public static class RelationshipStatusExtensions
{
    public static RelationshipStatusResponse ToRelationshipStatusResponse(this RelationshipStatus status, int profileId , RelationshipResult relationship)
    {
        switch (status)
        {
            case RelationshipStatus.Rejected:
                return RelationshipStatusResponse.None;
            case RelationshipStatus.Pending when relationship.InitiatorProfileId == profileId:
                return RelationshipStatusResponse.RequestReceived;
            case RelationshipStatus.Pending when relationship.ReceiverProfileId == profileId:
                return RelationshipStatusResponse.RequestSent;
            case RelationshipStatus.Approved:
                return RelationshipStatusResponse.Friend;
            case RelationshipStatus.BlockedByFirstProfile when relationship.InitiatorProfileId == profileId:
                return RelationshipStatusResponse.BlockedByPerson;
            case RelationshipStatus.BlockedBySecondProfile when relationship.ReceiverProfileId == profileId:
                return RelationshipStatusResponse.BlockedByYou;
            case RelationshipStatus.BlockedByBothProfiles:
                return RelationshipStatusResponse.BlockedBoth;
            default:
                return RelationshipStatusResponse.None;
        }

       
    }
}