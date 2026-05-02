using FluentValidation;

namespace TarotNow.Application.Features.Chat.Commands.SubmitConversationReview;

/// <summary>
/// Validator đầu vào cho command gửi đánh giá conversation.
/// </summary>
public sealed class SubmitConversationReviewCommandValidator : AbstractValidator<SubmitConversationReviewCommand>
{
    /// <summary>
    /// Khởi tạo rule validate cơ bản cho submit review.
    /// </summary>
    public SubmitConversationReviewCommandValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5);

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .When(x => string.IsNullOrWhiteSpace(x.Comment) == false);
    }
}
