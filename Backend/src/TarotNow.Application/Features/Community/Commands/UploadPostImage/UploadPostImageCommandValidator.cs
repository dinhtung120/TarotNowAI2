using FluentValidation;
using System.IO;

namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

public class UploadPostImageCommandValidator : AbstractValidator<UploadPostImageCommand>
{
    public UploadPostImageCommandValidator()
    {
        RuleFor(x => x.ImageStream)
            .NotNull()
            .Must(stream => stream != Stream.Null)
            .WithMessage("ImageStream không hợp lệ.");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(100);
    }
}
