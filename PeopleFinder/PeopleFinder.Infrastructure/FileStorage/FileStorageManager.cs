using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Infrastructure.Common.Helpers;

namespace PeopleFinder.Infrastructure.FileStorage;

public class FileStorageManager : IFileStorageManager
{
    private readonly IOptions<ImageSettings> _imageSettings;
    private readonly ILogger<FileStorageManager> _logger;

    public FileStorageManager(IOptions<ImageSettings> imageSettings, ILogger<FileStorageManager> logger)
    {
        _imageSettings = imageSettings;
        _logger = logger;
    }
    
    
    public async Task<(Guid Token, string Extension)> SaveImageAsync(ImageDto image, DateTime uploadTime)
    {
        string ext = Path.GetExtension(image.FileName);
        var token = Guid.NewGuid();
        string folderPath = FileFolderHelper.GetFileFolderPath(_imageSettings.Value.Path, uploadTime);
        Directory.CreateDirectory(folderPath);
        
        string filePath = Path.Combine(folderPath, $"{token}{ext}");
        await using var stream = new FileStream(filePath, FileMode.CreateNew);
        
        await stream.WriteAsync(image.Content);
        _logger.LogInformation($"Image saved to {filePath}");
        return (token, ext);
        
    }

    public async  Task<byte[]> GetImageAsync(string fileName, DateTime uploadTime)
    {
        string folderPath = FileFolderHelper.GetFileFolderPath(_imageSettings.Value.Path, uploadTime);
        
        string filePath = Path.Combine(folderPath, fileName);
        
        return await File.ReadAllBytesAsync(filePath);

    }
}