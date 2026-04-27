using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Application.Features.Admin.Queries.GetOutboxDashboard;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API vận hành outbox dành cho quản trị hệ thống.
public sealed class AdminOutboxController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller outbox operations.
    /// </summary>
    public AdminOutboxController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy dashboard trạng thái outbox (pending/failed/dead-letter/retry age).
    /// </summary>
    /// <param name="top">Số lượng bản ghi top failed/dead-letter trả về cho mỗi nhóm.</param>
    /// <returns>Snapshot vận hành outbox hiện tại.</returns>
    [HttpGet("outbox/dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] int top = 20)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetOutboxDashboardQuery
        {
            Top = top
        });

        return Ok(result);
    }
}
