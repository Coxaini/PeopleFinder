using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Interfaces.FileStorage;

namespace PeopleFinder.Infrastructure.FileStorage;

public class FileUrlService : IFileUrlService
{
    private readonly FileStorageSettings _fileSettings;

    public FileUrlService(IOptions<FileStorageSettings> fileSettings)
    {
        _fileSettings = fileSettings.Value;
    }
    
    public string? GetFileUrl(Guid? fileId)
    {
        return fileId is null ? null : $"{_fileSettings.BaseUrl}/{fileId}";
    }
}