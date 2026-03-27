using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/Admin")]
[Authorize(Roles = "admin")]
public sealed class AdminEscrowController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminEscrowController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("escrow/resolve-dispute")]
    public async Task<IActionResult> ResolveDispute([FromBody] ResolveDisputeBody body)
    {
        if (!User.TryGetUserId(out var adminId))
            return Unauthorized();

        var command = new TarotNow.Application.Features.Admin.Commands.ResolveDispute.ResolveDisputeCommand
        {
            ItemId = body.ItemId,
            Action = body.Action,
            AdminNote = body.AdminNote,
            AdminId = adminId
        };

        await _mediator.Send(command);
        return Ok(new { success = true, action = body.Action?.Trim().ToLowerInvariant() });
    }
}
