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
            return Result.Fail(FileServiceErrors.FileNotFound);
        
        FileStream fileStream = _fileStorageManager.GetFileAsync(media.Id.ToString()+'.' + media.Extension, media.UploadTime);
        
        return Result.Ok(new FileResult(media.OriginalName,media.Type, media.Extension, fileStream));
        
    }

    public async Task<Result<MediaFile>> UploadFileAsync(FileDto fileDto)
    {
        
        var now = DateTime.Now;
        var file = await _fileStorageManager.UploadFileAsync(fileDto, now);
        
        var MediaFile = new MediaFile()
        {
            Id = file.Token,
            OriginalName = fileDto.FileName,
            Type = fileDto.Type,
            Extension = file.Extension,
            UploadTime = now
        };
        
        await _unitOfWork.MediaFileRepository.AddAsync(MediaFile);
        await _unitOfWork.SaveAsync();
        return Result.Ok(MediaFile);
    }
}