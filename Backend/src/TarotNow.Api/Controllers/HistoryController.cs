using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.History.Queries.GetReadingDetail;
using TarotNow.Application.Features.History.Queries.GetReadingHistory;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize] // Yêu cầu đăng nhập (Header: "Authorization: Bearer <token>")
public class HistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public HistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách Lịch sử bốc bài Tarot có phân trang
    /// </summary>
    [HttpGet("sessions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingHistoryResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSessions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // Trích xuất UserId từ Access Token (Claims)
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var query = new GetReadingHistoryQuery
        {
            UserId = userId,
            Page = page > 0 ? page : 1,
            PageSize = pageSize is > 0 and <= 50 ? pageSize : 10
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Xem chi tiết một phiên bốc bài cụ thể (Kèm theo lịch sử Chat Follow-up với AI)
    /// </summary>
    [HttpGet("sessions/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingDetailResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionDetails([FromRoute] Guid id)
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var query = new GetReadingDetailQuery
        {
            UserId = userId,
            SessionId = id
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
