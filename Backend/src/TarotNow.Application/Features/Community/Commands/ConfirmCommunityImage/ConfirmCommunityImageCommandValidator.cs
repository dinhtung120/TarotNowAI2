using FluentValidation;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Features.Community.Commands.ConfirmCommunityImage;

/// <summary>
/// Validate input confirm community image.
/// </summary>
public sealed class ConfirmCommunityImageCommandValidator : AbstractValidator<ConfirmCommunityImageCommand>
{
    /// <summary>
    /// Khởi tạo rule validate command confirm community image.
    /// </summary>
    public ConfirmCommunityImageCommandValidator()
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

        RuleFor(x => x.ObjectKey)
            .NotEmpty()
            .MaximumLength(512)
            .Must(value => value.StartsWith("community/", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Community object key phải bắt đầu bằng 'community/'.");

        RuleFor(x => x.PublicUrl)
            .NotEmpty()
            .MaximumLength(2048);

        RuleFor(x => x.UploadToken)
            .NotEmpty()
            .MaximumLength(512);
    }
}
