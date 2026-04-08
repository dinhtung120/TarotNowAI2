using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Extensions;
using TarotNow.Api.Services;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;
using TarotNow.Application.Features.Auth.Commands.RevokeToken;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Auth)]
// API quản lý phiên đăng nhập.
// Luồng chính: login, refresh token và logout (một phiên hoặc toàn bộ phiên).
public sealed class AuthSessionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRefreshTokenCookieService _cookieService;

    /// <summary>
    /// Khởi tạo controller phiên đăng nhập.
    /// </summary>
    /// <param name="mediator">MediatR điều phối command auth.</param>
    /// <param name="cookieService">Service dựng options cookie refresh token.</param>
    public AuthSessionController(
        IMediator mediator,
        IRefreshTokenCookieService cookieService)
    {
        _mediator = mediator;
        _cookieService = cookieService;
    }

    /// <summary>
    /// Đăng nhập và cấp access token + refresh token.
    /// Luồng xử lý: gắn IP client vào command, gọi login, lưu refresh token vào cookie an toàn.
    /// </summary>
    /// <param name="command">Command đăng nhập.</param>
    /// <returns>Auth response chứa access token và thông tin user.</returns>
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        // Lưu IP nguồn để phục vụ audit phiên và phát hiện hành vi bất thường.
        command.ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var result = await _mediator.Send(command);

        // Đồng bộ refresh token vào cookie HttpOnly theo cấu hình hiện tại của request.
        Response.Cookies.Append("refreshToken", result.RefreshToken, _cookieService.BuildOptions(Request));
        return Ok(result.Response);
    }

    /// <summary>
    /// Làm mới cặp token dựa trên refresh token trong cookie.
    /// Luồng xử lý: đọc cookie, kiểm tra thiếu token, gọi command refresh, cập nhật cookie mới.
    /// </summary>
    /// <returns>Auth response mới hoặc lỗi 401 khi thiếu refresh token.</returns>
    [HttpPost("refresh")]
    [EnableRateLimiting("auth-session")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshTokens()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            // Edge case thiếu cookie: từ chối sớm để không chạy logic refresh vô nghĩa.
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Missing refresh token",
                detail: "Refresh token is missing.");
        }

        // Tạo command refresh với IP hiện tại để lưu dấu vết thay đổi phiên.
        var command = new RefreshTokenCommand
        {
            Token = refreshToken,
            ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        };

        var result = await _mediator.Send(command);

        // Ghi đè refresh token cũ bằng token mới để duy trì cơ chế rotating token.
        Response.Cookies.Append("refreshToken", result.NewRefreshToken, _cookieService.BuildOptions(Request));
        return Ok(result.Response);
    }

    /// <summary>
    /// Đăng xuất phiên hiện tại hoặc toàn bộ phiên của người dùng.
    /// Luồng xử lý: dựng command revoke, kiểm tra điều kiện revokeAll, xóa cookie sau khi revoke thành công.
    /// </summary>
    /// <param name="revokeAll">Cờ yêu cầu thu hồi toàn bộ phiên.</param>
    /// <returns>Thông báo đăng xuất thành công hoặc lỗi điều kiện đầu vào/quyền truy cập.</returns>
    [HttpPost("logout")]
    [EnableRateLimiting("auth-session")]
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
            if (!User.TryGetUserId(out var userId))
            {
                // Rule bảo mật: chỉ user đã xác thực mới được thu hồi toàn bộ phiên của chính họ.
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Authentication required",
                detail: "Must be authenticated to revoke all sessions.");
            }

            // Gắn UserId để backend revoke toàn bộ token theo chủ tài khoản.
            command.UserId = userId;
        }
        else if (string.IsNullOrEmpty(command.Token))
        {
            // Nhánh revoke một phiên bắt buộc phải có refresh token hiện tại.
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Missing refresh token",
                detail: "No refresh token provided.");
        }

        await _mediator.Send(command);
        // Xóa cookie ở client sau khi revoke thành công để đồng bộ trạng thái phiên.
        Response.Cookies.Delete("refreshToken", _cookieService.BuildOptions(Request));
        return Ok(new { message = "Logged out successfully." });
    }
}
