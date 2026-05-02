using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.SubmitConversationReview;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// User gửi đánh giá reader sau khi conversation completed.
    /// Luồng xử lý: xác thực user, gửi command submit review.
    /// </summary>
    /// <param name="id">Id hội thoại.</param>
    /// <param name="body">Payload đánh giá.</param>
    /// <returns>Kết quả submit review.</returns>
    [HttpPost("{id}/review")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> SubmitConversationReview(string id, [FromBody] ConversationReviewBody body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new SubmitConversationReviewCommand
        {
            ConversationId = id,
            UserId = userId,
            Rating = body.Rating,
            Comment = body.Comment
        });

        return Ok(result);
    }
}
