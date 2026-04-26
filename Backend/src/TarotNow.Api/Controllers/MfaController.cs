

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;


using TarotNow.Application.Features.Mfa.Commands.MfaChallenge; 
using TarotNow.Application.Features.Mfa.Commands.MfaSetup;     
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;    
using TarotNow.Application.Features.Mfa.Queries.GetMfaStatus;  

namespace TarotNow.Api.Controllers;


[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
// API xác thực đa yếu tố (MFA).
// Luồng chính: setup secret, verify mã, challenge giao dịch và truy vấn trạng thái MFA.
public class MfaController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller MFA.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command/query MFA.</param>
    public MfaController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Lấy user id từ context xác thực hiện tại.
    /// </summary>
    /// <returns>User id nếu tồn tại; ngược lại <c>null</c>.</returns>
    private Guid? GetUserId()
    {
        return User.GetUserIdOrNull();
    }

    /// <summary>
    /// Khởi tạo thông tin MFA cho người dùng.
    /// Luồng xử lý: xác thực user, gửi command setup, trả dữ liệu thiết lập MFA.
    /// </summary>
    /// <returns>Dữ liệu setup MFA hoặc unauthorized khi thiếu user id.</returns>
    [HttpPost("setup")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> Setup()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            // Chặn setup MFA khi không có danh tính người dùng hợp lệ.
            return this.UnauthorizedProblem();
        }

        // Gọi command setup để tạo secret/otpauth cho user hiện tại.
        var result = await _mediator.Send(new MfaSetupCommand { UserId = userId.Value });
        return Ok(result);
    }

    /// <summary>
    /// Xác minh mã MFA để bật MFA cho tài khoản.
    /// </summary>
    /// <param name="body">Payload mã MFA người dùng nhập.</param>
    /// <returns>Thông báo bật MFA thành công.</returns>
    [HttpPost("verify")]
    [EnableRateLimiting("auth-mfa-challenge")]
    public async Task<IActionResult> Verify([FromBody] MfaVerifyBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            // Chặn verify MFA khi request không có user id.
            return this.UnauthorizedProblem();
        }

        // Verify thành công sẽ cập nhật trạng thái bật MFA ở tầng ứng dụng.
        await _mediator.Send(new MfaVerifyCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "MFA đã được bật thành công." });
    }

    /// <summary>
    /// Xác thực challenge MFA cho thao tác nhạy cảm.
    /// </summary>
    /// <param name="body">Payload mã challenge MFA.</param>
    /// <returns>Thông báo challenge thành công.</returns>
    [HttpPost("challenge")]
    [EnableRateLimiting("auth-mfa-challenge")]
    public async Task<IActionResult> Challenge([FromBody] MfaChallengeBody body)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            // Chặn challenge khi không xác định được chủ thể xác thực.
            return this.UnauthorizedProblem();
        }

        await _mediator.Send(new MfaChallengeCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "Xác thực MFA thành công." });
    }

    /// <summary>
    /// Lấy trạng thái bật/tắt MFA của người dùng.
    /// </summary>
    /// <returns>Trạng thái MFA hiện tại của user.</returns>
    [HttpGet("status")]
    [EnableRateLimiting("auth-session")]
    public async Task<IActionResult> Status()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            // Chặn truy vấn trạng thái MFA khi thiếu user id.
            return this.UnauthorizedProblem();
        }

        var result = await _mediator.Send(new GetMfaStatusQuery { UserId = userId.Value });
        return Ok(new { mfaEnabled = result.MfaEnabled });
    }
}
