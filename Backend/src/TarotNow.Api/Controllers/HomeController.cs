using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Home.Queries.GetHomeSnapshot;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Home)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[AllowAnonymous]
public class HomeController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Snapshot trang chủ: reader nổi bật (1 round-trip, có presence).
    /// </summary>
    [HttpGet("snapshot")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeSnapshotDto))]
    public async Task<IActionResult> GetSnapshot(CancellationToken cancellationToken)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetHomeSnapshotQuery(), cancellationToken);
        return Ok(result);
    }
}
