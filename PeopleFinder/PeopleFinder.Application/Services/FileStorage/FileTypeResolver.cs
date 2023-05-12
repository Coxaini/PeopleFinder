using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Settings;
using PeopleFinder.Domain.Entities.MessagingEntities;


namespace PeopleFinder.Application.Services.FileStorage;

public class FileTypeResolver : IFileTypeResolver
{
    private readonly FileSettings _fileSettings;

    public FileTypeResolver(IOptions<FileSettings> fileSettings)
    {
        _fileSettings = fileSettings.Value;
    }
    
    public MediaFileType Resolve(string? fileName, long? fileSize)
    {
        if(fileName is null)
            return MediaFileType.File;
        
        string extension = Path.GetExtension(fileName);

        if(_fileSettings.Image.AllowedExtensions.Contains(extension) && fileSize < _fileSettings.Image.MaxSize)
            return MediaFileType.Image;
        if(_fileSettings.Video.AllowedExtensions.Contains(extension) && fileSize < _fileSettings.Video.MaxSize)
            return MediaFileType.Video;
        if(_fileSettings.Audio.AllowedExtensions.Contains(extension) && fileSize < _fileSettings.Audio.MaxSize)
            return MediaFileType.Audio;
        return MediaFileType.File;

    }
}