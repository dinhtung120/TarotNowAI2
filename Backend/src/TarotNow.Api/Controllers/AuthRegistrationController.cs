using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Features.Auth.Commands.Register;
using TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;
using TarotNow.Application.Features.Auth.Commands.VerifyEmail;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Auth)]
// API xử lý đăng ký và xác minh email tài khoản mới.
// Luồng chính: đăng ký user, gửi OTP xác minh và xác nhận kích hoạt tài khoản.
public sealed class AuthRegistrationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthRegistrationController> _logger;

    /// <summary>
    /// Khởi tạo controller đăng ký.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command xác thực/đăng ký.</param>
    /// <param name="logger">Logger ghi nhận lỗi gửi OTP hậu đăng ký.</param>
    public AuthRegistrationController(
        IMediator mediator,
        ILogger<AuthRegistrationController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Đăng ký tài khoản mới.
    /// Luồng xử lý: tạo user, dựng response chuẩn, trả CreatedAtAction để client có metadata tài nguyên mới.
    /// </summary>
    /// <param name="command">Command đăng ký người dùng.</param>
    /// <returns>Phản hồi 201 kèm thông tin user mới tạo.</returns>
    [HttpPost("register")]
    [EnableRateLimiting("auth-register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var userId = await _mediator.SendWithRequestCancellation(HttpContext, command);

        try
        {
            await _mediator.SendWithRequestCancellation(HttpContext, new SendEmailVerificationOtpCommand { Email = command.Email });
        }
        catch (Exception ex)
        {
            // Không fail luồng đăng ký nếu SMTP/provider lỗi tạm thời.
            _logger.LogWarning(ex, "Failed to send verification OTP after registration for {Email}.", command.Email);
        }

        var response = new RegisterResponse
        {
            UserId = userId,
            Message = "Registration successful. Please verify your email to activate your account."
        };

        // Trả CreatedAtAction để giữ semantics REST cho thao tác tạo tài nguyên.
        return CreatedAtAction("GetProfile", "Profile", null, response);
    }

    /// <summary>
    /// Gửi lại OTP xác minh email cho tài khoản chưa kích hoạt.
    /// </summary>
    /// <param name="command">Command gửi OTP xác minh email.</param>
    /// <returns>Thông báo đã xử lý yêu cầu gửi OTP.</returns>
    [HttpPost("send-verification-email")]
    [EnableRateLimiting("auth-register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendVerificationEmail([FromBody] SendEmailVerificationOtpCommand command)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(new { message = "If the email is valid and unverified, an OTP has been sent." });
    }

    /// <summary>
    /// Xác nhận email bằng OTP để kích hoạt tài khoản.
    /// </summary>
    /// <param name="command">Command xác minh email.</param>
    /// <returns>Thông báo kích hoạt tài khoản thành công.</returns>
    [HttpPost("verify-email")]
    [EnableRateLimiting("auth-register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
    {
        await _mediator.SendWithRequestCancellation(HttpContext, command);
        return Ok(new { message = "Email verified successfully. Account is now active." });
    }
}
