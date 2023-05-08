using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.FileStorage;

public record FileResult(string OriginalFileName,MediaFileType FileType, string Extension , FileStream ContentStream);