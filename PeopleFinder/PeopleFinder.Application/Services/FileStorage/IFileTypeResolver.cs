using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.FileStorage;

public interface IFileTypeResolver
{
    MediaFileType Resolve(string? fileName, long? fileSize);
}