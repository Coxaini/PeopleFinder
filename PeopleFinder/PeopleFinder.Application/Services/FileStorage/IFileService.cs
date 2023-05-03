using FluentResults;

namespace PeopleFinder.Application.Services.FileStorage;

public interface IFileService
{
    Task<Result<FileResult>> GetFileAsync(Guid token);
}