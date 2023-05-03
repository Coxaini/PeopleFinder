using FluentValidation;
using Microsoft.Extensions.Options;

namespace PeopleFinder.Application.Models.File;

public class ImageDtoValidator : AbstractValidator<ImageDto>
{
    private readonly ImageSettings settings;


    public ImageDtoValidator(IOptions<ImageSettings> options)
    {
        settings = options.Value;
        RuleFor(x => x.Content).Must(x => x.Length < settings.MaxSize);
        RuleFor(x => x.ContentType).NotEmpty();
        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(x=>options.Value.AllowedExtensions.Contains(Path.GetExtension(x)))
            .WithMessage($"File extension is not allowed. Only the following extensions are allowed:" +
                         $" {string.Join(", ", settings.AllowedExtensions)}");
    }
}