using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Wallet.Queries.GetLedgerList;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller xử lý các tác vụ liên quan đến Ví người dùng.
/// Bắt buộc đăng nhập.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize] // Bảo mật route này, chỉ User đã xác thực mới đc truy cập
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy số dư hiện tại của người dùng.
    /// </summary>
    /// <returns>Đối tượng chứa Gold, Diamond.</returns>
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var query = new GetWalletBalanceQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Lấy lịch sử giao dịch ví (Ledger) có phân trang.
    /// </summary>
    [HttpGet("ledger")]
    public async Task<IActionResult> GetLedger([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var query = new GetLedgerListQuery(userId, page, limit);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
