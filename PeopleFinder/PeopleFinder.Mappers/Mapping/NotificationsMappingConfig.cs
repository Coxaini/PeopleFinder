using Mapster;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Contracts.Notifications;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Mappers.Mapping;

public class NotificationsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserMessage, SendMessageNotification>()
            .Map(dest=>dest.ChatType, src=>src.ChatType.ToString())
            .Map(dest => dest.AttachmentUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AttachmentFileId, null))
            .Map(dest => dest.DisplayImageAvatarUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AuthorAvatarId, "images/default.jpg"));

        config.NewConfig<UserChat, ChatCreatedNotification>()
            .Map(dest=>dest.ChatId, src=>src.Id)
            .Map(dest=>dest.DisplayName, src=>src.DisplayTitle)
            .Map(dest=>dest.ChatType, src=>src.ChatType.ToString())
            .Map(dest => dest.DisplayImageAvatarUrl, 
                source => 
                    MapContext.Current.GetService<IFileUrlService>().GetFileUrl(source.DisplayLogoId, null) 
                    ??( source.ChatType == ChatType.Direct ? 
                        MapContext.Current.GetService<IFileUrlService>().GetFileUrl("images/default.jpg") 
                        :MapContext.Current.GetService<IFileUrlService>().GetFileUrl("images/default-group.jpg")))
            .Map(dest=>dest.LastMessage, src=>src.LastMessage ?? string.Empty);
        
        config.NewConfig<UserMessage, EditMessageNotification>()
            .Map(dest => dest.AttachmentUrl,
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AttachmentFileId, null));

    }
}