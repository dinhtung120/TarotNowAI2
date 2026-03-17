using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;
using TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;
using TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller quản lý rút tiền (Withdrawal).
///
/// Reader tạo yêu cầu rút (POST /create).
/// Reader xem lịch sử (GET /my).
/// Admin approve/reject (qua AdminController).
/// </summary>
[Route("api/v1/withdrawal")]
[ApiController]
[Authorize]
public class WithdrawalController : ControllerBase
{
    private readonly IMediator _mediator;

    public WithdrawalController(IMediator mediator) => _mediator = mediator;

    private Guid? GetUserId()
    {
        var str = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(str, out var id) ? id : null;
    }

    /// <summary>
    /// Reader tạo yêu cầu rút tiền.
    /// Guards: min 50D, 1/ngày, KYC approved, đủ số dư.
    /// Fee 10%, debit ngay khi tạo.
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateWithdrawalBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var command = new CreateWithdrawalCommand
        {
            UserId = userId.Value,
            AmountDiamond = body.AmountDiamond,
            BankName = body.BankName,
            BankAccountName = body.BankAccountName,
            BankAccountNumber = body.BankAccountNumber,
            MfaCode = body.MfaCode
        };

        var requestId = await _mediator.Send(command);
        return Ok(new { success = true, requestId });
    }

    /// <summary>Reader xem lịch sử rút tiền của mình.</summary>
    [HttpGet("my")]
    public async Task<IActionResult> MyWithdrawals([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

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

// --- Request Body ---

public class CreateWithdrawalBody
{
    public long AmountDiamond { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankAccountName { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
    public string MfaCode { get; set; } = string.Empty;
}
