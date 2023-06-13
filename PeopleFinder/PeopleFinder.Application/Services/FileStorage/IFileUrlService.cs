namespace PeopleFinder.Application.Services.FileStorage;

public interface IFileUrlService
{
    string? GetFileUrl(Guid? fileId, string? defaultUrl = null);

    string GetFileUrl(string fileName);

}