using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Application.Features.Community.Commands.ConfirmCommunityImage;
using TarotNow.Application.Features.Community.Commands.PresignCommunityImage;

namespace TarotNow.Api.Controllers;

public partial class CommunityController
{
    /// <summary>
    /// Sinh presigned URL upload ảnh community trực tiếp lên R2.
    /// </summary>
    [HttpPost("image/presign")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> PresignCommunityImage([FromBody] CommunityImagePresignRequest body)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new PresignCommunityImageCommand
        {
            UserId = GetRequiredUserId(),
            ContextType = body.ContextType,
            ContextDraftId = body.ContextDraftId,
            ContentType = body.ContentType,
            SizeBytes = body.SizeBytes,
        });

        return Ok(new PresignedUploadResponse(
            result.UploadUrl,
            result.ObjectKey,
            result.PublicUrl,
            result.UploadToken,
            result.ExpiresAtUtc));
    }

    /// <summary>
    /// Xác nhận ảnh community đã upload thành công và lưu trạng thái uploaded.
    /// </summary>
    [HttpPost("image/confirm")]
    [EnableRateLimiting("community-write")]
    public async Task<IActionResult> ConfirmCommunityImage([FromBody] CommunityImageConfirmRequest body)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new ConfirmCommunityImageCommand
        {
            UserId = GetRequiredUserId(),
            ContextType = body.ContextType,
            ContextDraftId = body.ContextDraftId,
            ObjectKey = body.ObjectKey,
            PublicUrl = body.PublicUrl,
            UploadToken = body.UploadToken,
        });

        return Ok(new
        {
            success = true,
            objectKey = result.ObjectKey,
            publicUrl = result.PublicUrl,
            contextType = result.ContextType,
            contextDraftId = result.ContextDraftId,
        });
    }
}
