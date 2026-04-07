using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

namespace TarotNow.Api.Controllers;

public partial class DepositController
{
    [HttpPost("orders")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateDepositOrderRequest request)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return this.UnauthorizedProblem();
        }

        var command = new CreateDepositOrderCommand
        {
            UserId = userId,
            AmountVnd = request.AmountVnd,
            IdempotencyKey = request.IdempotencyKey
        };

        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
