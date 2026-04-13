using FluentValidation;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Features.Profile.Commands.PresignAvatarUpload;

/// <summary>
/// Validate input command presign avatar.
/// </summary>
public sealed class PresignAvatarUploadCommandValidator : AbstractValidator<PresignAvatarUploadCommand>
{
    /// <summary>
    /// Khởi tạo rule validate command presign avatar.
    /// </summary>
    public PresignAvatarUploadCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(64)
            .Must(value => string.Equals(value, MediaUploadConstants.RequiredImageMimeType, StringComparison.OrdinalIgnoreCase))
            .WithMessage("Avatar chỉ hỗ trợ image/webp.");

        RuleFor(x => x.SizeBytes)
            .GreaterThan(0)
            .LessThanOrEqualTo(MediaUploadConstants.MaxImageUploadBytes);
    }
}
