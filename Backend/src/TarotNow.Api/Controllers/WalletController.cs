

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;


using TarotNow.Application.Features.Wallet.Queries.GetLedgerList;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] 
[EnableRateLimiting("auth-session")]
// API ví người dùng.
// Luồng chính: lấy số dư ví và danh sách ledger giao dịch theo phân trang.
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller ví.
    /// </summary>
    /// <param name="mediator">MediatR điều phối query ví/ledger.</param>
    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy số dư ví hiện tại của người dùng.
    /// </summary>
    /// <returns>Thông tin số dư ví.</returns>
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn ví khi request không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Query số dư theo user hiện tại để tránh lộ dữ liệu ví chéo tài khoản.
        var query = new GetWalletBalanceQuery(userId);
        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);

        return Ok(result);
    }

    /// <summary>
    /// Lấy lịch sử giao dịch ví (ledger) theo phân trang.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="limit">Số giao dịch mỗi trang.</param>
    /// <returns>Dữ liệu ledger của người dùng hiện tại.</returns>
    [HttpGet("ledger")]
    public async Task<IActionResult> GetLedger([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        if (!User.TryGetUserId(out var userId))
        {
            // Chặn truy vấn ledger khi thiếu danh tính user.
            return this.UnauthorizedProblem();
        }

        // Truy vấn ledger giữ nguyên page/limit để handler chịu trách nhiệm chuẩn hóa business rule.
        var query = new GetLedgerListQuery(userId, page, limit);
        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);

        return Ok(result);
    }
}
