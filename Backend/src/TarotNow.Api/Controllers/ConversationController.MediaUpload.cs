using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Chat.Commands.PresignConversationMedia;

namespace TarotNow.Api.Controllers;

public partial class ConversationController
{
    /// <summary>
    /// Sinh presigned URL upload media chat (image/voice) trực tiếp lên R2.
    /// </summary>
    [HttpPost("{conversationId}/media/presign")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> PresignMedia(string conversationId, [FromBody] ConversationMediaPresignRequest body)
    {
        if (TryGetUserId(out var userId) == false)
        {
            return this.UnauthorizedProblem();
        }

        var result = await Mediator.Send(new PresignConversationMediaCommand
        {
            ConversationId = conversationId,
            RequesterId = userId,
            MediaKind = body.MediaKind,
            ContentType = body.ContentType,
            SizeBytes = body.SizeBytes,
            DurationMs = body.DurationMs,
        });

        return Ok(new PresignedUploadResponse(
            result.UploadUrl,
            result.ObjectKey,
            result.PublicUrl,
            result.UploadToken,
            result.ExpiresAtUtc));
    }
}
