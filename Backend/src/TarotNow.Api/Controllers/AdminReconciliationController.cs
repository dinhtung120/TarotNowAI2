using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API đối soát dữ liệu tài chính cho admin.
// Luồng chính: truy xuất các mismatch giữa ledger và số dư ví.
public sealed class AdminReconciliationController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller đối soát.
    /// </summary>
    /// <param name="mediator">MediatR dùng để gọi query đối soát.</param>
    public AdminReconciliationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách sai lệch ví phục vụ kiểm tra và xử lý vận hành.
    /// </summary>
    /// <returns>Danh sách mismatch giữa ledger và wallet aggregate.</returns>
    [HttpGet("reconciliation/wallet")]
    public async Task<IActionResult> GetWalletMismatches()
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetLedgerMismatchQuery());
        return Ok(result);
    }
}
