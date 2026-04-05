using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Call.Queries.GetCallHistory;

namespace TarotNow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/conversations/{conversationId}/calls")]
public class CallController : ControllerBase
{
    private readonly IMediator _mediator;

    public CallController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// GET /api/v1/conversations/{conversationId}/calls?page=1&amp;pageSize=20
    /// Lịch sử cuộc gọi trong conversation (paginated).
    /// Cho phép xem cả khi conversation đã completed.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCallHistory(
        [FromRoute] string conversationId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        var participantIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(participantIdStr, out var participantId))
            return Unauthorized();

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

    // Ghi chú: getActive (như plan) đã không còn cần thiết bằng API rời rạc vì SignalR hub khi join group 
    // đã có sẵn mechanism load trạng thái hoặc ta có thể quản lý lifecycle bằng Client Cache (Zustand).
    // Có thể bổ sung sau nếu FE cần re-sync.
}
