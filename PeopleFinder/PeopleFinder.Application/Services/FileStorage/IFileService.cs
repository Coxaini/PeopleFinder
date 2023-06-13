using FluentResults;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.FileStorage;

public interface IFileService
{
    Task<Result<FileResult>> GetFileAsync(Guid token);
    
    Task<Result<MediaFile>> UploadFileAsync(FileDto fileDto);
}