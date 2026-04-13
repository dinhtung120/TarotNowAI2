using FluentValidation;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Features.Chat.Commands.PresignConversationMedia;

/// <summary>
/// Validate input presign conversation media.
/// </summary>
public sealed class PresignConversationMediaCommandValidator : AbstractValidator<PresignConversationMediaCommand>
{
    /// <summary>
    /// Khởi tạo rule validate command presign conversation media.
    /// </summary>
    public PresignConversationMediaCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.RequesterId)
            .NotEmpty();

        RuleFor(x => x.MediaKind)
            .NotEmpty()
            .MaximumLength(16)
            .Must(MediaUploadConstants.IsChatMediaKind)
            .WithMessage("MediaKind chỉ nhận image|voice.");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.SizeBytes)
            .GreaterThan(0)
            .LessThanOrEqualTo(MediaUploadConstants.MaxImageUploadBytes);

        RuleFor(x => x.DurationMs)
            .GreaterThan(0)
            .LessThanOrEqualTo(600_000)
            .When(x => string.Equals(x.MediaKind, "voice", StringComparison.OrdinalIgnoreCase));
    }
}
