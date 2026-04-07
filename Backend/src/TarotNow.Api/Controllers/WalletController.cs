

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using TarotNow.Application.Features.Wallet.Queries.GetLedgerList;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] 
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        
        var query = new GetWalletBalanceQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

        [HttpGet("ledger")]
    public async Task<IActionResult> GetLedger([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        
        var query = new GetLedgerListQuery(userId, page, limit);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
