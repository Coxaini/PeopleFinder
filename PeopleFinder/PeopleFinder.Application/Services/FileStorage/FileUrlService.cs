using Microsoft.Extensions.Options;
using PeopleFinder.Application.Services.FileStorage;

namespace PeopleFinder.Application.Services.FileStorage;

public class FileUrlService : IFileUrlService
{
    private readonly FileStorageSettings _fileSettings;

    public FileUrlService(IOptions<FileStorageSettings> fileSettings)
    {
        _fileSettings = fileSettings.Value;
    }
    
    public string? GetFileUrl(Guid? fileId, string? defaultUrl = null)
    {
        if (fileId != null)
        {
            return $"{_fileSettings.BaseUrl}/{fileId}";
        }

        return defaultUrl != null ? $"{_fileSettings.BaseUrl}/{defaultUrl}" : null;
    }

    public string GetFileUrl(string fileName)
    {
        return $"{_fileSettings.BaseUrl}/{fileName}";
    }
}