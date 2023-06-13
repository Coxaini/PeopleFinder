using FluentResults;
using Microsoft.Extensions.Logging;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Application.Services.FileStorage;

public class FileService : IFileService
{
    
    private readonly IFileStorageManager _fileStorageManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FileService> _logger;

    public FileService(IFileStorageManager fileStorageManager, IUnitOfWork unitOfWork, ILogger<FileService> logger)
    {
        _fileStorageManager = fileStorageManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FileResult>> GetFileAsync(Guid token)
    {
        var media = await _unitOfWork.MediaFileRepository.GetByToken(token);

        if (media is null)
        {
            _logger.LogInformation("File with token {token} not found in a database", token);
            return Result.Fail(FileErrors.FileNotFound);
        }

        Stream fileStream;
        try
        {
            fileStream = await _fileStorageManager.GetFileAsync(media);
        }
        catch (FileNotFoundException e)
        {
            _logger.LogError(e, "File with token {token} not found in a file storage", token);
            return Result.Fail(FileErrors.FileNotFound);
        }

        return Result.Ok(new FileResult(media.OriginalName,media.Type, media.Extension, fileStream));
        
    }

    public async Task<Result<MediaFile>> UploadFileAsync(FileDto fileDto)
    {
        var uploadTime = DateTime.UtcNow;
        
        var mediaFile = await _fileStorageManager.UploadFileAsync(fileDto);

        await _unitOfWork.MediaFileRepository.AddAsync(mediaFile);
        await _unitOfWork.SaveAsync();
        return Result.Ok(mediaFile);
    }
}