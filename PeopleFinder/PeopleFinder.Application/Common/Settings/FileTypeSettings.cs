namespace PeopleFinder.Application.Common.Settings;

public class FileTypeSettings
{
    public int MaxSize { get; init; }
    public string[] AllowedExtensions { get; init; } = null!;
}