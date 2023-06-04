using PeopleFinder.Application.Models.File;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Common.Interfaces.FileStorage;

public interface IFileStorageManager
{
    Task<MediaFile> UploadFileAsync(FileDto fileDto);
    /// <exception cref="FileNotFoundException"></exception>
    FileStream GetFileAsync(MediaFile mediaFile);
    
    
    void DeleteFileAsync(MediaFile mediaFile);
}