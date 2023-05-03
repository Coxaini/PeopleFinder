using FluentResults;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
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
        
        var file = await _fileStorageManager.GetImageAsync(media.Token.ToString() + media.Extension, media.UploadTime);
        
        return Result.Ok(new FileResult(media.OriginalName, file));
        
    }
}