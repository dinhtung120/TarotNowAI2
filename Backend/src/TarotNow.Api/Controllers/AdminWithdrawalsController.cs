using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Withdrawal.Queries.GetWithdrawalDetail;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
[EnableRateLimiting("auth-session")]
// API quản trị rút tiền.
// Luồng chính: lấy hàng chờ withdrawal và xử lý duyệt/từ chối yêu cầu.
public sealed class AdminWithdrawalsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller quản trị rút tiền.
    /// </summary>
    /// <param name="mediator">MediatR điều phối query/command.</param>
    public AdminWithdrawalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy hàng chờ yêu cầu rút tiền đang pending.
    /// Luồng xử lý: dựng query với cờ pending và tham số phân trang từ request.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số mục mỗi trang.</param>
    /// <returns>Danh sách yêu cầu rút tiền đang chờ xử lý.</returns>
    [HttpGet("withdrawals/queue")]
    public async Task<IActionResult> WithdrawalQueue([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        // Cố định PendingOnly để endpoint này chỉ phục vụ màn hình queue cần hành động.
        var query = new TarotNow.Application.Features.Withdrawal.Queries.ListWithdrawals.ListWithdrawalsQuery
        {
            PendingOnly = true,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.SendWithRequestCancellation(HttpContext, query);
        return Ok(result);
    }

    /// <summary>
    /// Lấy chi tiết một yêu cầu rút tiền kèm QR chuyển khoản.
    /// </summary>
    /// <param name="withdrawalId">Định danh yêu cầu rút tiền.</param>
    /// <returns>Dữ liệu chi tiết phục vụ thao tác chuyển khoản thủ công.</returns>
    [HttpGet("withdrawals/{withdrawalId:guid}")]
    public async Task<IActionResult> WithdrawalDetail([FromRoute] Guid withdrawalId)
    {
        var result = await _mediator.SendWithRequestCancellation(HttpContext, new GetWithdrawalDetailQuery
        {
            WithdrawalId = withdrawalId
        });
        return Ok(result);
    }

    /// <summary>
    /// Xử lý một yêu cầu rút tiền từ hàng chờ.
    /// Luồng xử lý: xác thực admin id, ánh xạ payload sang command, thực thi nghiệp vụ.
    /// </summary>
    /// <param name="body">Payload xử lý withdrawal.</param>
    /// <returns>Kết quả thành công kèm action đã áp dụng.</returns>
    [HttpPost("withdrawals/process")]
    public async Task<IActionResult> ProcessWithdrawal([FromBody] ProcessWithdrawalBody body)
    {
        if (!User.TryGetUserId(out var adminId))
        {
            // Chặn request không có danh tính admin để không phát sinh thao tác tài chính không truy vết.
            return this.UnauthorizedProblem();
        }

        var idempotencyKey = ResolveIdempotencyKey(body.IdempotencyKey);
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid idempotency key",
                detail: "Idempotency key is required.");
        }

        // Mapping đầy đủ body sang command để handler kiểm soát idempotency và rule duyệt/từ chối.
        var command = new TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal.ProcessWithdrawalCommand
        {
            RequestId = body.WithdrawalId,
            AdminId = adminId,
            Action = body.Action,
            AdminNote = body.AdminNote,
            IdempotencyKey = idempotencyKey,
        };

        await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(new { success = true, action = body.Action });
    }

    private string ResolveIdempotencyKey(string? bodyKey)
    {
        return Request.GetIdempotencyKeyOrEmpty(bodyKey);
    }
}
