namespace PeopleFinder.Application.Models.File;

public record ImageDto(string FileName, string ContentType, byte[] Content);