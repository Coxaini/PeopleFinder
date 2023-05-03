using PeopleFinder.Application.Models.File;

namespace PeopleFinder.Application.Common.Interfaces.FileStorage;

public interface IFileStorageManager
{
    Task<(Guid Token, string Extension)> SaveImageAsync(ImageDto image, DateTime uploadTime);
    
    Task<byte[]> GetImageAsync(string fileName , DateTime uploadTime);
}