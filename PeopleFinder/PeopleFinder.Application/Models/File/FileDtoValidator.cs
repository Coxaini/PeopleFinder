using FluentValidation;
using Microsoft.Extensions.Options;
using PeopleFinder.Application.Common.Settings;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Models.File;

public class FileDtoValidator : AbstractValidator<FileDto>
{
    public FileDtoValidator(IOptions<FileSettings> options)
    {
        var settings = options.Value;

        RuleFor(x=>x.ContentStream.Length).LessThan(settings.FileMaxSize)
            .WithMessage($"File size is too big. Max file size is {settings.FileMaxSize/1024.0} KB");
        
        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(x=>settings.Image.AllowedExtensions.Contains(Path.GetExtension(x)))
            .When(f=>f.Type == MediaFileType.Image)
            .WithMessage($"Image extension is not allowed. Only the following extensions are allowed:" +
                         $" {string.Join(", ", settings.Image.AllowedExtensions)}");
        
    }
}