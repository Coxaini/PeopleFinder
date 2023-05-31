using System.Drawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Infrastructure.Common.Helpers;

namespace PeopleFinder.Infrastructure.FileStorage;

public class FileStorageManager : IFileStorageManager
{
    private readonly string _fileStoragePath;
    private readonly ILogger<FileStorageManager> _logger;

    public FileStorageManager(IOptions<FileStorageSettings> fileSettings, ILogger<FileStorageManager> logger)
    {
        _fileStoragePath = fileSettings.Value.FilePath ?? throw new IOException("File path not found in a configuration");
        
        _logger = logger;
    }
    
    
    public async Task<MediaFile> UploadFileAsync(FileDto fileDto)
    {
        
        var uploadTime = DateTime.UtcNow;
        
        string ext = Path.GetExtension(fileDto.FileName);
        var token = Guid.NewGuid();
        string folderPath = FileFolderHelper.GetFileFolderPath(_fileStoragePath, uploadTime);
        Directory.CreateDirectory(folderPath);
        
        string filePath = Path.Combine(folderPath, $"{token}{ext}");
        await using var stream = new FileStream(filePath, FileMode.CreateNew);
        await fileDto.ContentStream.CopyToAsync(stream);

        _logger.LogInformation("{fileName} saved to {filePath}", fileDto.FileName, filePath);
        
        var mediaFile = new MediaFile()
        {
            Id = token,
            OriginalName = fileDto.FileName,
            Type = fileDto.Type,
            Extension = ext[1..],
            UploadTime = uploadTime
        };

        return mediaFile;

    }

    
    
    public FileStream GetFileAsync(MediaFile mediaFile)
    {
        string folderPath = FileFolderHelper.GetFileFolderPath(_fileStoragePath, mediaFile.UploadTime);

        string fileName = mediaFile.Id.ToString() + '.' + mediaFile.Extension;
        
        string filePath = Path.Combine(folderPath, fileName);
        if (!File.Exists(filePath))
        {
            _logger.LogError("File {fileName} not found", fileName);
            throw new FileNotFoundException($"File {fileName} not found");
        }
        var stream = new FileStream(filePath, FileMode.Open);
        
        return stream;

    }

    public void DeleteFileAsync(MediaFile mediaFile)
    {
        string folderPath = FileFolderHelper.GetFileFolderPath(_fileStoragePath, mediaFile.UploadTime);

        string fileName = mediaFile.Id.ToString() + '.' + mediaFile.Extension;
        
        string filePath = Path.Combine(folderPath, fileName);
        if (!File.Exists(filePath))
        {
            _logger.LogError("File {fileName} not found", fileName);
            throw new FileNotFoundException($"File {fileName} not found");
        }
        
        File.Delete(filePath);
        _logger.LogInformation("File {fileName} deleted", fileName);
    }
}