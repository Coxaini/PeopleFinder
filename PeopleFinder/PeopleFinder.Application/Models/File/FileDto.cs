using Microsoft.AspNetCore.Http;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Models.File;

public class FileDto
{
    public FileDto(string fileName, Stream contentStream, MediaFileType type)
    {
        this.FileName = fileName;
        this.ContentStream = contentStream;
        this.Type = type;
    }

    public string FileName { get; init; }
    public Stream ContentStream { get; init; }
    public MediaFileType Type { get; init; }

    public static FileDto? FromFormFile(IFormFile? file)
    {
        if (file is null)
            return null;
        return new FileDto(file.FileName, file.OpenReadStream(), MediaFileType.Image);
    }
    
    public void Deconstruct(out string fileName, out Stream contentStream, out MediaFileType type)
    {
        fileName = this.FileName;
        contentStream = this.ContentStream;
        type = this.Type;
    }
}