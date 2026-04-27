using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.UserContext)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[EnableRateLimiting("auth-session")]
// API cung cấp metadata khởi tạo cho user context.
// Luồng chính: xác thực user rồi trả metadata tổng hợp dùng cho bootstrap client.
public class UserContextController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller user context.
    /// </summary>
    /// <param name="mediator">MediatR điều phối query metadata.</param>
    public UserContextController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy metadata khởi tạo cho người dùng hiện tại.
    /// </summary>
    /// <returns>Metadata bootstrap của user hoặc unauthorized khi thiếu user id.</returns>
    [HttpGet("metadata")]
    [Authorize]
    public async Task<IActionResult> GetInitialMetadata(CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn metadata khi không có danh tính hợp lệ.
            return this.UnauthorizedProblem();
        }

        var query = new GetInitialMetadataQuery(userId);
        var result = await _mediator.SendWithRequestCancellation(HttpContext, query, cancellationToken);

        return Ok(result);
    }
}
