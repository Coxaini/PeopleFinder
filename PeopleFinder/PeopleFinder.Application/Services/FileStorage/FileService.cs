using FluentResults;
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
    
    public FileService(IFileStorageManager fileStorageManager, IUnitOfWork unitOfWork)
    {
        _fileStorageManager = fileStorageManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FileResult>> GetFileAsync(Guid token)
    {
        var media = await _unitOfWork.MediaFileRepository.GetByToken(token);

        if (media is null)
            return Result.Fail(FileErrors.FileNotFound);
        
        FileStream fileStream = _fileStorageManager.GetFileAsync(media);
        
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