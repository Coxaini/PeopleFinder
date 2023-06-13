namespace PeopleFinder.Application.Common.Settings;

public class FileSettings
{
    public FileTypeSettings Image { get; init; } = null!;
    public FileTypeSettings Video { get; init; } = null!;
    public FileTypeSettings Audio { get; init; } = null!;
    
    public int FileMaxSize { get; init; }
    
}