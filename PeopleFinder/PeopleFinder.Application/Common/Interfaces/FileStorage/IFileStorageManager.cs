using PeopleFinder.Application.Models.File;

namespace PeopleFinder.Application.Common.Interfaces.FileStorage;

public interface IFileStorageManager
{
    Task<(Guid Token, string Extension)> SaveFileAsync(FileDto fileDto, DateTime uploadTime);
    
    FileStream GetFileAsync(string fileName , DateTime uploadTime);
}