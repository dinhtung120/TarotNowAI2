using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.UserContext.Queries.GetNavbarSnapshot;
using TarotNow.Application.Features.UserContext.Queries.GetRuntimePolicies;
using TarotNow.Application.Features.UserContext.Queries.GetReadingSetupSnapshot;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Me)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
[EnableRateLimiting("auth-session")]
public class MeController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Một round-trip: unread thông báo, unread chat, streak, preview dropdown thông báo.
    /// </summary>
    [HttpGet("navbar-snapshot")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NavbarSnapshotDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNavbarSnapshot()
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new GetNavbarSnapshotQuery(userId));
        return Ok(result);
    }

    /// <summary>
    /// Gộp số dư ví + catalog bài cho màn setup reading (một round-trip).
    /// </summary>
    [HttpGet("reading-setup-snapshot")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadingSetupSnapshotDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetReadingSetupSnapshot(CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new GetReadingSetupSnapshotQuery(userId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lấy snapshot policy runtime từ system_configs cho FE.
    /// </summary>
    [HttpGet("runtime-policies")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RuntimePoliciesDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRuntimePolicies(CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out _))
        {
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new GetRuntimePoliciesQuery(), cancellationToken);
        return Ok(result);
    }
}
