

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;
using TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;
using TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Withdrawal)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] 
public class WithdrawalController : ControllerBase
{
    private readonly IMediator _mediator;

    
    public WithdrawalController(IMediator mediator) => _mediator = mediator;

        private Guid? GetUserId()
    {
        return User.GetUserIdOrNull();
    }

        [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateWithdrawalBody body)
    {
        var userId = GetUserId();
        if (userId == null) return this.UnauthorizedProblem();

        var command = new CreateWithdrawalCommand
        {
            UserId = userId.Value,                       
            AmountDiamond = body.AmountDiamond,           
            IdempotencyKey = body.IdempotencyKey,
            BankName = body.BankName,                     
            BankAccountName = body.BankAccountName,       
            BankAccountNumber = body.BankAccountNumber,   
            MfaCode = body.MfaCode                        
        };

        
        var requestId = await _mediator.Send(command);
        return Ok(new { success = true, requestId });
    }

        [HttpGet("my")]
    public async Task<IActionResult> MyWithdrawals([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = GetUserId();
        if (userId == null) return this.UnauthorizedProblem();

        var query = new ListWithdrawalsQuery
        {
            UserId = userId.Value,   
            PendingOnly = false,     
            Page = page,
            PageSize = pageSize,
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
