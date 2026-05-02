using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.SubmitConversationReview;

/// <summary>
/// Command gửi đánh giá reader sau khi conversation completed.
/// </summary>
public sealed class SubmitConversationReviewCommand : IRequest<ConversationReviewSubmitResult>
{
    /// <summary>
    /// Id conversation được đánh giá.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// User gửi đánh giá.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Điểm số 1..5.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Nhận xét tùy chọn.
    /// </summary>
    public string? Comment { get; set; }
}
