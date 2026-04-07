using FluentValidation;
using System.IO;

namespace TarotNow.Application.Features.Profile.Commands.UploadAvatar;

public class UploadAvatarCommandValidator : AbstractValidator<UploadAvatarCommand>
{
    public UploadAvatarCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

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
