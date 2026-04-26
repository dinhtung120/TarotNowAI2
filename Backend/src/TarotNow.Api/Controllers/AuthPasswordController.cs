using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Auth.Commands.ForgotPassword;
using TarotNow.Application.Features.Auth.Commands.ResetPassword;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Auth)]
// API xử lý luồng quên mật khẩu và đặt lại mật khẩu.
// Luồng chính: gửi OTP reset và xác nhận đổi mật khẩu bằng mã hợp lệ.
public sealed class AuthPasswordController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo controller xử lý mật khẩu.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command xác thực.</param>
    public AuthPasswordController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gửi yêu cầu quên mật khẩu.
    /// Luồng xử lý: dispatch command và luôn trả thông điệp trung tính để tránh lộ tồn tại email.
    /// </summary>
    /// <param name="command">Command chứa email quên mật khẩu.</param>
    /// <returns>Thông báo đã xử lý yêu cầu gửi OTP.</returns>
    [HttpPost("forgot-password")]
    [EnableRateLimiting("auth-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        // Trả thông điệp cố định để giảm nguy cơ enumeration tài khoản.
        return Ok(new { message = "If the email exists, a password reset OTP has been sent." });
    }

    /// <summary>
    /// Đặt lại mật khẩu bằng OTP hợp lệ.
    /// Luồng xử lý: dispatch command reset, sau đó trả thông báo logout thiết bị cũ.
    /// </summary>
    /// <param name="command">Command chứa OTP và mật khẩu mới.</param>
    /// <returns>Thông báo đặt lại mật khẩu thành công.</returns>
    [HttpPost("reset-password")]
    [EnableRateLimiting("auth-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Password has been successfully reset. All existing devices have been logged out." });
    }
}
