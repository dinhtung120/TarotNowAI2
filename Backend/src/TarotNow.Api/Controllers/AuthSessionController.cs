using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Api.Services;
using TarotNow.Application.Features.Auth.Commands.Login;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Auth)]
public sealed class AuthSessionController : ControllerBase
{
    private const string LogoutSuccessMessage = "Logged out successfully.";
    private readonly IAuthService _authService;
    private readonly IAuthCookieService _authCookieService;

    public AuthSessionController(IAuthService authService, IAuthCookieService authCookieService)
    {
        _authService = authService;
        _authCookieService = authCookieService;
    }

    /// <summary>
    /// Xác thực thông tin đăng nhập và cấp access/refresh token mới.
    /// </summary>
    [HttpPost("login")]
    [EnableRateLimiting("auth-login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(command, HttpContext, cancellationToken);
        _authCookieService.SetAccessToken(Request, Response, result.Response.AccessToken, result.Response.ExpiresInSeconds);
        _authCookieService.SetRefreshToken(Request, Response, result.RefreshToken, result.RefreshTokenExpiresAtUtc);
        return Ok(result.Response);
    }

    /// <summary>
    /// Refresh access token theo mô hình refresh token rotation.
    /// </summary>
    [HttpPost("refresh")]
    [EnableRateLimiting("auth-refresh-token-family")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshTokens(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[AuthCookieNames.RefreshToken];
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            _authCookieService.ClearAuthCookies(Request, Response);
            return this.UnauthorizedProblem("Missing refresh token.");
        }

        var result = await _authService.RefreshAsync(
            refreshToken,
            ResolveIdempotencyKeyHeader(Request),
            HttpContext,
            cancellationToken);
        if (result?.Response is null
            || string.IsNullOrWhiteSpace(result.Response.AccessToken)
            || string.IsNullOrWhiteSpace(result.NewRefreshToken))
        {
            _authCookieService.ClearAuthCookies(Request, Response);
            return this.UnauthorizedProblem("Failed to refresh authentication session.");
        }

        _authCookieService.SetAccessToken(Request, Response, result.Response.AccessToken, result.Response.ExpiresInSeconds);
        _authCookieService.SetRefreshToken(Request, Response, result.NewRefreshToken, result.RefreshTokenExpiresAtUtc);
        return Ok(result.Response);
    }

    /// <summary>
    /// Đăng xuất phiên hiện tại hoặc thu hồi toàn bộ phiên của người dùng.
    /// </summary>
    [HttpPost("logout")]
    [EnableRateLimiting("auth-logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromQuery] bool revokeAll = false, CancellationToken cancellationToken = default)
    {
        var refreshToken = Request.Cookies[AuthCookieNames.RefreshToken] ?? string.Empty;
        Guid? userId = User.TryGetUserId(out var parsedUserId) ? parsedUserId : null;
        if (revokeAll && userId is null && string.IsNullOrWhiteSpace(refreshToken))
        {
            _authCookieService.ClearAuthCookies(Request, Response);
            return this.UnauthorizedProblem("Must be authenticated or provide refresh token to revoke all sessions.");
        }

        if (!revokeAll && string.IsNullOrWhiteSpace(refreshToken))
        {
            _authCookieService.ClearAuthCookies(Request, Response);
            return this.UnauthorizedProblem("Missing refresh token.");
        }

        await _authService.LogoutAsync(refreshToken, revokeAll, userId, HttpContext, cancellationToken);
        _authCookieService.ClearAuthCookies(Request, Response);
        return Ok(new { success = true, message = LogoutSuccessMessage });
    }

    private static string? ResolveIdempotencyKeyHeader(HttpRequest request)
    {
        var primary = request.Headers[AuthHeaders.IdempotencyKey].ToString();
        if (!string.IsNullOrWhiteSpace(primary))
        {
            return primary;
        }

        var legacy = request.Headers[AuthHeaders.LegacyIdempotencyKey].ToString();
        return string.IsNullOrWhiteSpace(legacy) ? null : legacy;
    }
}
