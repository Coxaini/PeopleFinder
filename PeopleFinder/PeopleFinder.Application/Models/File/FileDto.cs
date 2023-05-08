using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Models.File;

public record FileDto(string FileName, Stream ContentStream, MediaFileType Type);