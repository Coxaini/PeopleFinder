using Mapster;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Application.Services.ProfileServices;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Infrastructure.FileStorage;
using PeopleFinder.Mappers.Extensions;

namespace PeopleFinder.Mappers.Mapping
{
    public class ProfileMappingsConfig : IRegister
    {

        public void Register(TypeAdapterConfig config)
        {

            
            config.NewConfig<FriendProfile, ShortProfileResponse>()
                .Map(dest=>dest.MainPictureUrl, 
                    src=>MapContext.Current
                        .GetService<IFileUrlService>().GetFileUrl(src.Profile.MainPictureId,"images/default.jpg"))
                .Map(dest => dest, src => src.Profile);


            config.NewConfig<Profile, ShortProfileResponse>()
                .Map(dest => dest.Tags, src => src.Tags)
                .Map(dest => dest.Age, src => src.Age)
                .Map(dest=>dest.MainPictureUrl, 
                    src=>MapContext.Current
                        .GetService<IFileUrlService>().GetFileUrl(src.MainPictureId,"images/default.jpg"));


            config
                .NewConfig<(Profile Profile, Relationship Relationship, CursorList<FriendProfile> MutualFriends),
                    ProfileResult>()
                .Map(dest => dest, src => src.Profile)
                .Map(dest => dest.Tags, src => src.Profile.Tags)
                .Map(dest => dest.MutualFriends, src => src.MutualFriends)
                .Map(dest => dest.Age, src => src.Profile.Age)
                .Map(dest => dest.Relationship, src => src.Relationship);

            config
                .NewConfig<(Profile Profile, CursorList<FriendProfile> MutualFriends ), ProfileResult>()
                .Map(dest => dest, src => src.Profile)
                .Map(dest => dest.Tags, src => src.Profile.Tags)
                .Map(dest => dest.MutualFriends, src => src.MutualFriends);

            config.NewConfig<ProfileResult, ProfileResponse>()
                .Map(dest=>dest.MutualFriends, 
                    src=>src.MutualFriends != null ? src.MutualFriends.Items.Select(p=>p.Profile.Username) : new List<string>())
                .Map(src=>src.Tags, dest=>dest.Tags)
                .Map(dest=>dest.MainPictureUrl, 
                    src=>MapContext.Current
                        .GetService<IFileUrlService>().GetFileUrl(src.MainPictureId,"images/default.jpg"))
                .Map(dest=>dest.Status, src=>(src.Relationship != null ? src.Relationship!.Status.
                    ToRelationshipStatusResponse( src.Id, src.Relationship) : RelationshipStatusResponse.None).ToString().ToLower());
                    
                    //src =>(DateTime.Today - src.BirthDate).TotalDays / 365.25);
                    //src => src.BirthDate != null ? (int)((DateTime.Today - src.BirthDate).Value.TotalDays / 365.25) : (int?)null);
                            

            config.NewConfig<Tag, TagResponse>()
                .Map(dest => dest.Title, src => src.Name);

            //config.NewConfig<ProfilePicture, string>();
            // .Map(dest => dest, src => src.);

        }
    }
}
