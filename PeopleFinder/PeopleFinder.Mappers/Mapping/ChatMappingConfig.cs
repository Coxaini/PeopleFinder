using Mapster;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.Chat;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Contracts.Chats;
using PeopleFinder.Domain.Common.Models;

namespace PeopleFinder.Mappers.Mapping;

public class ChatMappingConfig : IRegister
{
    private readonly IFileUrlService _fileUrlService;

    public ChatMappingConfig(IFileUrlService fileUrlService)
    {
        _fileUrlService = fileUrlService;
    }
    
    public void Register(TypeAdapterConfig config)
    {
        
        config.NewConfig<(SendMessage Message, int ProfileId), SendMessageRequest>()
            .Map(dest=>dest, source=>source.Message)
            .Map(dest=>dest.SenderId, source=>source.ProfileId)
            .Map(dest => dest.Attachment, 
                source => FileDto.FromFormFile(source.Message.Attachment));
        
        
        
        config.NewConfig<UserChat, ChatResponse>()
            .Map(dest => dest.DisplayLogoUrl, source => _fileUrlService.GetFileUrl(source.DisplayLogoId))
            .Map(dest => dest.ChatType, source => source.ChatType.ToString());
    }
}