using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

namespace TarotNow.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(Roles = "admin")] // Chỉ dành cho Admin
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("reconciliation/wallet")]
    public async Task<IActionResult> GetWalletMismatches()
    {
        var query = new GetLedgerMismatchQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> ListUsers([FromQuery] TarotNow.Application.Features.Admin.Queries.ListUsers.ListUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("users/lock")]
    public async Task<IActionResult> ToggleUserLock([FromBody] TarotNow.Application.Features.Admin.Commands.ToggleUserLock.ToggleUserLockCommand command)
    {
        var success = await _mediator.Send(command);
        return success ? Ok() : BadRequest();
    }

    [HttpGet("deposits")]
    public async Task<IActionResult> ListDeposits([FromQuery] TarotNow.Application.Features.Admin.Queries.ListDeposits.ListDepositsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("users/add-balance")]
    public async Task<IActionResult> AddUserBalance([FromBody] TarotNow.Application.Features.Admin.Commands.AddUserBalance.AddUserBalanceCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể cộng tiền cho người dùng này." });
    }
}
