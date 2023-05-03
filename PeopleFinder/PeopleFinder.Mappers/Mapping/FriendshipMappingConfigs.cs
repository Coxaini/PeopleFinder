using Mapster;
using PeopleFinder.Application.Models.Relationship;
using PeopleFinder.Contracts.Rating;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Mappers.Mapping;

public class FriendshipMappingConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        /*config.NewConfig<IEnumerable<FriendRequest>, IEnumerable<RatingResponse>>()
            .Map(dest => dest, source => source);*/

        config.NewConfig<Relationship, RelationshipResult>();
        
        config.NewConfig<Relationship, FriendRequestResponse>()
            .Map(dest=>dest.RequestId, source => source.Id)
            .Map(dest => dest.Profile, source => source.InitiatorProfile)
            .Map(dest => dest.SentAt, source => source.SentAt);
        
    }
}