using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;
using TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;
using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.CheckIn)]
[Authorize]
public class CheckInController : ControllerBase
{
    private readonly IMediator _mediator;

    public CheckInController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpPost]
    public async Task<IActionResult> DailyCheckIn()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var command = new DailyCheckInCommand { UserId = userId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

        [HttpGet("streak")]
    public async Task<IActionResult> GetStreakStatus()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        var query = new GetStreakStatusQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

        [HttpPost("freeze")]
    public async Task<IActionResult> PurchaseFreeze([FromBody] PurchaseStreakFreezeCommand command)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        command.UserId = userId; 

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
