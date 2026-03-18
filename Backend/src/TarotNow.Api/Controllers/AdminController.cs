using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        if (string.IsNullOrWhiteSpace(command.IdempotencyKey))
        {
            command.IdempotencyKey = Request.Headers["X-Idempotency-Key"].ToString();
        }

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể cộng tiền cho người dùng này." });
    }

    [HttpPatch("deposits/process")]
    public async Task<IActionResult> ProcessDeposit([FromBody] TarotNow.Application.Features.Admin.Commands.ProcessDeposit.ProcessDepositCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể xử lý đơn nạp tiền này." });
    }

    // ======================================================================
    // Phase 2.1 — Reader Request Management
    // ======================================================================

    /// <summary>
    /// Danh sách đơn xin Reader có phân trang.
    /// Hỗ trợ filter theo status (pending/approved/rejected).
    /// </summary>
    [HttpGet("reader-requests")]
    public async Task<IActionResult> ListReaderRequests(
        [FromQuery] TarotNow.Application.Features.Admin.Queries.ListReaderRequests.ListReaderRequestsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Admin phê duyệt hoặc từ chối đơn xin Reader.
    ///
    /// Body: { requestId: string, action: "approve"|"reject", adminNote?: string }
    /// AdminId tự lấy từ JWT claims — không cho client gửi.
    /// </summary>
    [HttpPatch("reader-requests/process")]
    public async Task<IActionResult> ProcessReaderRequest([FromBody] ProcessReaderRequestBody body)
    {
        // Lấy adminId từ JWT — bảo mật, không tin client
        var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(adminIdString) || !Guid.TryParse(adminIdString, out var adminId))
            return Unauthorized();

        var command = new TarotNow.Application.Features.Admin.Commands.ApproveReader.ApproveReaderCommand
        {
            RequestId = body.RequestId,
            Action = body.Action,
            AdminNote = body.AdminNote,
            AdminId = adminId
        };

        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể xử lý đơn xin Reader." });
    }

    /// <summary>
    /// Admin xử lý dispute: release cho reader hoặc refund cho user.
    /// Action: "release" | "refund"
    /// </summary>
    [HttpPost("escrow/resolve-dispute")]
    public async Task<IActionResult> ResolveDispute([FromBody] ResolveDisputeBody body)
    {
        var adminId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (Guid?)null;
        if (adminId == null) return Unauthorized();

        var command = new TarotNow.Application.Features.Admin.Commands.ResolveDispute.ResolveDisputeCommand
        {
            ItemId = body.ItemId,
            Action = body.Action,
            AdminNote = body.AdminNote,
            AdminId = adminId.Value
        };

        await _mediator.Send(command);
        return Ok(new { success = true, action = body.Action?.Trim().ToLowerInvariant() });
    }

    // ======================================================================
    // Phase 2.4 — Withdrawal Admin Endpoints
    // ======================================================================

    /// <summary>Admin lấy danh sách yêu cầu rút tiền pending.</summary>
    [HttpGet("withdrawals/queue")]
    public async Task<IActionResult> WithdrawalQueue([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = new TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals.ListWithdrawalsQuery
        {
            PendingOnly = true,
            Page = page,
            PageSize = pageSize,
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>Admin approve/reject yêu cầu rút tiền.</summary>
    [HttpPost("withdrawals/process")]
    public async Task<IActionResult> ProcessWithdrawal([FromBody] ProcessWithdrawalBody body)
    {
        var adminId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (Guid?)null;
        if (adminId == null) return Unauthorized();

        var command = new TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal.ProcessWithdrawalCommand
        {
            RequestId = body.WithdrawalId,
            AdminId = adminId.Value,
            Action = body.Action,
            AdminNote = body.AdminNote,
            MfaCode = body.MfaCode,
        };

        await _mediator.Send(command);
        return Ok(new { success = true, action = body.Action });
    }
}

/// <summary>
/// DTO cho PATCH /admin/reader-requests/process.
/// Tách riêng để AdminId lấy từ JWT, không từ body (chống giả mạo).
/// </summary>
public class ProcessReaderRequestBody
{
    /// <summary>ObjectId string của reader_requests document trong MongoDB.</summary>
    public string RequestId { get; set; } = string.Empty;

    /// <summary>Hành động: "approve" | "reject".</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Ghi chú admin (tùy chọn).</summary>
    public string? AdminNote { get; set; }
}

/// <summary>Body cho POST /admin/escrow/resolve-dispute.</summary>
public class ResolveDisputeBody
{
    /// <summary>UUID chat_question_items.id bị dispute.</summary>
    public Guid ItemId { get; set; }

    /// <summary>Hành động: "release" → reader nhận tiền, "refund" → user nhận lại.</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Ghi chú admin.</summary>
    public string? AdminNote { get; set; }
}

/// <summary>Body cho POST /admin/withdrawals/process.</summary>
public class ProcessWithdrawalBody
{
    public Guid WithdrawalId { get; set; }
    /// <summary>"approve" | "reject"</summary>
    public string Action { get; set; } = string.Empty;
    public string? AdminNote { get; set; }
    public string MfaCode { get; set; } = string.Empty;
}
