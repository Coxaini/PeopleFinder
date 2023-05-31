using Mapster;
using PeopleFinder.Contracts.Recommendation;
using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Domain.Common.Recommendation;

namespace PeopleFinder.Mappers.Mapping
{
    public class RecommendationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<IEnumerable<Profile>, RecommendationResponse>()
                .Map(dest => dest.Profiles, source => source);
            
            config.NewConfig<ProfileWithMutualFriends, ProfileWithMutualFriendsResponse>()
                .Map(dest=>dest.MainPictureUrl, 
                    src=>MapContext.Current
                        .GetService<IFileUrlService>().GetFileUrl(src.Profile.MainPictureId,"images/default.jpg"))
                .Map(dest => dest.Age, src => src.Profile.Age)
                .Map(dest=>dest, source=>source.Profile)
                .Map(dest=>dest.MutualFriends, source=>source.MutualFriends);
            
        }
    }
}
