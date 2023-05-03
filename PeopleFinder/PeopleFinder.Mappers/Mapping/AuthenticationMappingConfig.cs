using Mapster;
using PeopleFinder.Application.Services.Authentication;
using PeopleFinder.Contracts.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Mappers.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(dest => dest.Token, source => source.Token)
                .Map(dest => dest, source => source.User)
                .Map(dest => dest.Username, source => source.User.Profile!.Username);
        }
    }
}
