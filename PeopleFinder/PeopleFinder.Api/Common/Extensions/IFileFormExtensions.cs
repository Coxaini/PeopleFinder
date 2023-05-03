namespace PeopleFinder.Api.Common.Extensions;

public static class FileFormExtensions
{
    public static byte[] GetBytes(this IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
    
}