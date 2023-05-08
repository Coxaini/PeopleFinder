using System.Drawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Infrastructure.Common.Helpers;

namespace PeopleFinder.Infrastructure.FileStorage;

public class FileStorageManager : IFileStorageManager
{
    private readonly string _fileStoragePath;
    private readonly ILogger<FileStorageManager> _logger;

    public FileStorageManager(IConfiguration fileConfiguration, ILogger<FileStorageManager> logger)
    {
        _fileStoragePath = fileConfiguration["FilePath"] ?? throw new IOException("File path not found in a configuration");
        
        _logger = logger;
    }
    
    
    public async Task<(Guid Token, string Extension)> SaveFileAsync(FileDto fileDto, DateTime uploadTime)
    {
        
        string ext = Path.GetExtension(fileDto.FileName);
        var token = Guid.NewGuid();
        string folderPath = FileFolderHelper.GetFileFolderPath(_fileStoragePath, uploadTime);
        Directory.CreateDirectory(folderPath);
        
        string filePath = Path.Combine(folderPath, $"{token}{ext}");
        await using var stream = new FileStream(filePath, FileMode.CreateNew);
        await fileDto.ContentStream.CopyToAsync(stream);

        _logger.LogInformation("{fileName} saved to {filePath}", fileDto.FileName, filePath);
        return (token, ext[1..]);
        
    }

    
    /// <exception cref="FileNotFoundException"></exception>
    public FileStream GetFileAsync(string fileName, DateTime uploadTime)
    {
        string folderPath = FileFolderHelper.GetFileFolderPath(_fileStoragePath, uploadTime);
        
        string filePath = Path.Combine(folderPath, fileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File {fileName} not found");
        }
        
        return File.OpenRead(filePath);

    }
}