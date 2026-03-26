using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/Admin")]
[Authorize(Roles = "admin")]
public sealed class AdminWithdrawalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminWithdrawalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("withdrawals/queue")]
    public async Task<IActionResult> WithdrawalQueue([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = new TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals.ListWithdrawalsQuery
        {
            PendingOnly = true,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("withdrawals/process")]
    public async Task<IActionResult> ProcessWithdrawal([FromBody] ProcessWithdrawalBody body)
    {
        if (!User.TryGetUserId(out var adminId))
            return Unauthorized();

        var command = new TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal.ProcessWithdrawalCommand
        {
            RequestId = body.WithdrawalId,
            AdminId = adminId,
            Action = body.Action,
            AdminNote = body.AdminNote,
            MfaCode = body.MfaCode,
        };

        await _mediator.Send(command);
        return Ok(new { success = true, action = body.Action });
    }
}
