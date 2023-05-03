namespace PeopleFinder.Application.Models.File;

public class ImageSettings
{
    public int MaxSize { get; init; }
    public string[] AllowedExtensions { get; init; } = null!;
    public string Path { get; init; } = null!;
}