using Mapster;
using PeopleFinder.Api.Notifications;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Contracts.Messages;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Infrastructure.FileStorage;


namespace PeopleFinder.Mappers.Mapping;

public class MessageMappingConfig : IRegister
{
 
    
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<Message, MessageResponse>()
            .Map(dest => dest.AttachmentUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AttachmentFileId, null));
        
        config.NewConfig<UserMessage, UserMessageResponse>()
            .Map(dest=>dest.AvatarUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AuthorAvatarId, "images/default.jpg"))
            .Map(dest => dest.AttachmentUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AttachmentFileId, null));

        config.NewConfig<UserMessage, MessageResponse>()
            .Map(dest => dest.AttachmentUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AttachmentFileId, null));

        
        config.NewConfig<UserMessage, SendMessageNotification>()
            .Map(dest=>dest.ChatType, src=>src.ChatType.ToString())
            .Map(dest => dest.AttachmentUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AttachmentFileId, null))
            .Map(dest => dest.DisplayImageAvatarUrl, 
                src => MapContext.Current.GetService<IFileUrlService>().GetFileUrl(src.AuthorAvatarId, "images/default.jpg"));
        
    }
}