

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
[EnableRateLimiting("auth-session")]
// API rút tiền người dùng.
// Luồng chính: tạo yêu cầu rút tiền và truy vấn danh sách yêu cầu rút của chính user.
public class WithdrawalController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller rút tiền.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query rút tiền.</param>
    public WithdrawalController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Lấy user id từ context xác thực hiện tại.
    /// </summary>
    /// <returns>User id nếu tồn tại; ngược lại <c>null</c>.</returns>
    private Guid? GetUserId()
    {
        return User.GetUserIdOrNull();
    }

    /// <summary>
    /// Tạo yêu cầu rút tiền mới.
    /// Luồng xử lý: xác thực user, map payload sang command, gửi command và trả request id.
    /// </summary>
    /// <param name="body">Payload tạo yêu cầu rút tiền.</param>
    /// <returns>Kết quả tạo yêu cầu rút tiền.</returns>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateWithdrawalBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            // Chặn tạo yêu cầu rút khi không có user id hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Mapping rõ ràng toàn bộ thông tin ngân hàng + MFA cho command xử lý.
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

        // Gọi command tạo yêu cầu và nhận request id để client theo dõi trạng thái.
        var requestId = await _mediator.Send(command);
        return Ok(new { success = true, requestId });
    }

    /// <summary>
    /// Lấy danh sách yêu cầu rút tiền của user hiện tại.
    /// </summary>
    /// <param name="page">Trang hiện tại.</param>
    /// <param name="pageSize">Số yêu cầu mỗi trang.</param>
    /// <returns>Danh sách yêu cầu rút tiền của user.</returns>
    [HttpGet("my")]
    public async Task<IActionResult> MyWithdrawals([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            // Chặn truy vấn yêu cầu rút khi thiếu user id.
            return this.UnauthorizedProblem();
        }

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
