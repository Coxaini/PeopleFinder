namespace PeopleFinder.Application.Common.Interfaces.FileStorage;

public interface IFileUrlService
{
    string? GetFileUrl(Guid? fileId);
}