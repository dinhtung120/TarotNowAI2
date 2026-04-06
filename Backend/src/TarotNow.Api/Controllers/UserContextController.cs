using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller cung cấp các endpoint về ngữ cảnh người dùng, gộp các yêu cầu metadata để tối ưu hóa hiệu suất.
/// </summary>
[Route(ApiRoutes.UserContext)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
public class UserContextController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserContextController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/user-context/metadata
    /// MỤC ĐÍCH: Lấy toàn bộ metadata cần thiết cho Dashboard (Wallet, Streak, Notifications, Chat) trong 1 request.
    /// Giúp tránh tình trạng "bão request" khi load trang.
    /// </summary>
    [HttpGet("metadata")]
    [Authorize]
    public async Task<IActionResult> GetInitialMetadata()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var query = new GetInitialMetadataQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
