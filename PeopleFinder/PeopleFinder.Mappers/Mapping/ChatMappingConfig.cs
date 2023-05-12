using Mapster;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Models.Message;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Contracts.Chats;
using PeopleFinder.Contracts.Messages;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Infrastructure.FileStorage;

namespace PeopleFinder.Mappers.Mapping;

public class ChatMappingConfig : IRegister
{

    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<UserChat, ChatResponse>()
            .Map(dest => dest.DisplayLogoUrl, 
                source => 
                     MapContext.Current.GetService<IFileUrlService>().GetFileUrl(source.DisplayLogoId, null) 
                          ??( source.ChatType == ChatType.Direct ? 
                              MapContext.Current.GetService<IFileUrlService>().GetFileUrl("images/default.jpg") 
                              :MapContext.Current.GetService<IFileUrlService>().GetFileUrl("images/default-group.jpg")))
            .Map(dest => dest.ChatType, source => source.ChatType.ToString());
    }
}