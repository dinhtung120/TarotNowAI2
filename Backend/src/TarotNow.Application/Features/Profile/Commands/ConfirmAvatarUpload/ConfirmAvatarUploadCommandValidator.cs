using FluentValidation;

namespace TarotNow.Application.Features.Profile.Commands.ConfirmAvatarUpload;

/// <summary>
/// Validate input confirm avatar upload.
/// </summary>
public sealed class ConfirmAvatarUploadCommandValidator : AbstractValidator<ConfirmAvatarUploadCommand>
{
    /// <summary>
    /// Khởi tạo rule validate command confirm avatar.
    /// </summary>
    public ConfirmAvatarUploadCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ObjectKey)
            .NotEmpty()
            .MaximumLength(512)
            .Must(value => value.StartsWith("avatars/", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Avatar object key phải bắt đầu bằng 'avatars/'.");

        RuleFor(x => x.PublicUrl)
            .NotEmpty()
            .MaximumLength(2048);

        RuleFor(x => x.UploadToken)
            .NotEmpty()
            .MaximumLength(512);
    }
}
