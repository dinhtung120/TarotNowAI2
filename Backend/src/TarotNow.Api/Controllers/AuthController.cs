using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Features.Auth.Commands.Register;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;
using TarotNow.Application.Features.Auth.Commands.RevokeToken;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public AuthController(IMediator mediator, IWebHostEnvironment environment, IConfiguration configuration)
    {
        _mediator = mediator;
        _environment = environment;
        _configuration = configuration;
    }

    /// <summary>
    /// Đăng ký tài khoản người dùng mới.
    /// Phase 1.1 Auth Baseline -> Yêu cầu verify email sau khi đăng ký thành công.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Domain Rule violation (email exist)
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var userId = await _mediator.Send(command);

        var response = new RegisterResponse
        {
            UserId = userId,
            Message = "Registration successful. Please verify your email to activate your account."
        };

        // Đã có ProfileController.GetProfile, dùng CreatedAtAction
        return CreatedAtAction("GetProfile", "Profile", null, response);
    }

    /// <summary>
    /// Đăng nhập để lấy JWT Access Token và Refresh Token (HttpOnly Cookie).
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)] // Domain Rule violation (invalid criteria)
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        // Lấy IP client
        command.ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var result = await _mediator.Send(command);

        // Thiết lập Refresh Token thành HttpOnly Cookie để chống XSS
        var cookieOptions = BuildRefreshCookieOptions();
        
        Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);

        return Ok(result.Response);
    }

    /// <summary>
    /// Xoay vòng Refresh Token (Rotation) để cấp JWT mới.
    /// Đọc Token từ HttpOnly Cookie.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Cookie không hợp lệ hoặc Token compromised
    public async Task<IActionResult> RefreshTokens()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Refresh token is missing." });
        }

        var command = new RefreshTokenCommand
        {
            Token = refreshToken,
            ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        };

        var result = await _mediator.Send(command);

        // Reset HttpOnly Cookie mới
        var cookieOptions = BuildRefreshCookieOptions();
        Response.Cookies.Append("refreshToken", result.NewRefreshToken, cookieOptions);

        return Ok(result.Response);
    }

    /// <summary>
    /// Đăng xuất: Thu hồi Refresh Token hiện hành hoặc toàn bộ.
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromQuery] bool revokeAll = false)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        
        var command = new RevokeTokenCommand
        {
            Token = refreshToken ?? string.Empty,
            RevokeAll = revokeAll
        };

        if (revokeAll)
        {
            // Trích xuất UserId từ claims nếu có để truyền vào Command
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var parsedUserId))
            {
                command.UserId = parsedUserId;
            }
            else
            {
                // Nếu chưa authenticate nhưng gửi request Logout-All -> báo lỗi
                return Unauthorized(new { message = "Must be authenticated to revoke all sessions." });
            }
        }
        else if (string.IsNullOrEmpty(command.Token))
        {
            return BadRequest(new { message = "No refresh token provided." });
        }

        await _mediator.Send(command);

        // Xóa Cookie ở phía client
        Response.Cookies.Delete("refreshToken", BuildRefreshCookieOptions());

        return Ok(new { message = "Logged out successfully." });
    }

    /// <summary>
    /// Gửi mã xác thực OTP qua Email (Mock).
    /// </summary>
    [HttpPost("send-verification-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendVerificationEmail([FromBody] TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp.SendEmailVerificationOtpCommand command)
    {
        // Luôn trả về OK để chống dò email
        await _mediator.Send(command);
        return Ok(new { message = "If the email is valid and unverified, an OTP has been sent." });
    }

    /// <summary>
    /// Kiểm tra OTP và kích hoạt tài khoản.
    /// </summary>
    [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail([FromBody] TarotNow.Application.Features.Auth.Commands.VerifyEmail.VerifyEmailCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { message = "Email verified successfully. Account is now active." });
    }

    /// <summary>
    /// Gửi mã OTP Đặt lại mật khẩu.
    /// </summary>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] TarotNow.Application.Features.Auth.Commands.ForgotPassword.ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If the email exists, a password reset OTP has been sent." });
    }

    /// <summary>
    /// Đặt lại mật khẩu mới. Update Hash và Revoke toàn bộ Refresh Tokens.
    /// </summary>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] TarotNow.Application.Features.Auth.Commands.ResetPassword.ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { message = "Password has been successfully reset. All existing devices have been logged out." });
    }

    private CookieOptions BuildRefreshCookieOptions()
    {
        var expiryDays = ResolveRefreshTokenExpiryDays();
        var shouldUseSecureCookie = !_environment.IsDevelopment() || Request.IsHttps;

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = shouldUseSecureCookie,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(expiryDays)
        };
    }

    private int ResolveRefreshTokenExpiryDays()
    {
        var configured = _configuration["Jwt:RefreshExpiryDays"]
                         ?? _configuration["Jwt:RefreshTokenExpirationDays"];

        return int.TryParse(configured, out var value) && value > 0
            ? value
            : 7;
    }
}
