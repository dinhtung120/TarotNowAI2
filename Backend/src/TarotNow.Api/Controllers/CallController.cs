using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Call.Queries.GetCallHistory;

namespace TarotNow.Api.Controllers;

[ApiController]
[Authorize(Policy = ApiAuthorizationPolicies.AuthenticatedUser)]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.ConversationCalls)]
// API truy xuất lịch sử cuộc gọi theo hội thoại.
// Luồng chính: xác định participant từ claim, dựng query phân trang, trả danh sách call history.
public class CallController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller lịch sử cuộc gọi.
    /// </summary>
    /// <param name="mediator">MediatR dùng để gọi query lịch sử cuộc gọi.</param>
    public CallController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy lịch sử cuộc gọi của một hội thoại.
    /// Luồng xử lý: đọc participant id từ claim, validate định dạng Guid, rồi gửi query phân trang.
    /// </summary>
    /// <param name="conversationId">Id hội thoại cần lấy lịch sử cuộc gọi.</param>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số mục mỗi trang.</param>
    /// <returns>Dữ liệu lịch sử cuộc gọi đã phân trang.</returns>
    [HttpGet]
    [EnableRateLimiting("call-history")]
    public async Task<IActionResult> GetCallHistory(
        [FromRoute] string conversationId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        var participantIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(participantIdStr, out var participantId))
        {
            // Claim định danh không hợp lệ thì chặn ngay để bảo vệ dữ liệu lịch sử cuộc gọi.
            throw new UnauthorizedAccessException();
        }

        // Dựng query đầy đủ để handler xác minh quyền participant trong hội thoại tương ứng.
        var query = new GetCallHistoryQuery
        {
            ConversationId = conversationId,
            ParticipantId = participantId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return Ok(new
        {
            TotalCount = result.TotalCount,
            Items = result.Items
        });
    }
}
