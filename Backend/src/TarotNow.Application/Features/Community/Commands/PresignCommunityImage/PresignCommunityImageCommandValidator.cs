using FluentValidation;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Features.Community.Commands.PresignCommunityImage;

/// <summary>
/// Validate input presign community image.
/// </summary>
public sealed class PresignCommunityImageCommandValidator : AbstractValidator<PresignCommunityImageCommand>
{
    /// <summary>
    /// Khởi tạo rule validate command presign community image.
    /// </summary>
    public PresignCommunityImageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ContextType)
            .NotEmpty()
            .MaximumLength(32)
            .Must(MediaUploadConstants.IsCommunityContextType)
            .WithMessage("ContextType chỉ nhận post/comment.");

        RuleFor(x => x.ContextDraftId)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(64)
            .Must(value => string.Equals(value, MediaUploadConstants.RequiredImageMimeType, StringComparison.OrdinalIgnoreCase))
            .WithMessage("Community image chỉ hỗ trợ image/webp.");

        RuleFor(x => x.SizeBytes)
            .GreaterThan(0)
            .LessThanOrEqualTo(MediaUploadConstants.MaxImageUploadBytes);
    }
}
