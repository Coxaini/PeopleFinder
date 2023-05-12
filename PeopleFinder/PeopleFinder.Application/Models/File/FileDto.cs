using Microsoft.AspNetCore.Http;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Models.File;

public class FileDto
{
    public FileDto(string fileName, Stream contentStream, MediaFileType fileType)
    {
        this.FileName = fileName;
        this.ContentStream = contentStream;
        this.Type = fileType;
    }

    public string FileName { get; init; }
    public Stream ContentStream { get; init; }
    
    public MediaFileType Type { get; init; }

    public static FileDto? FromFormFile(IFormFile? file, MediaFileType fileType)
    {
        if (file is null)
            return null;
        return new FileDto(file.FileName, file.OpenReadStream(), fileType);
    }
    
}