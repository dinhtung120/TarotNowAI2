using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Api.Services;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;
using TarotNow.Application.Features.Auth.Commands.RevokeToken;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Auth)]
public sealed class AuthSessionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthCookieService _authCookieService;

    /// <summary>
    /// Khởi tạo auth session controller.
    /// </summary>
    public AuthSessionController(IMediator mediator, IAuthCookieService authCookieService)
    {
        _mediator = mediator;
        _authCookieService = authCookieService;
    }

    /// <summary>
    /// Đăng nhập user và cấp access/refresh tokens.
    /// </summary>
    [HttpPost("login")]
    [EnableRateLimiting("auth-login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        command.ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        command.DeviceId = ResolveDeviceId(Request.Headers[AuthHeaders.DeviceId].ToString());
        command.UserAgentHash = HashValue(Request.Headers.UserAgent.ToString());

        var result = await _mediator.Send(command, cancellationToken);

        _authCookieService.SetAccessToken(Request, Response, result.Response.AccessToken, result.Response.ExpiresInSeconds);
        _authCookieService.SetRefreshToken(Request, Response, result.RefreshToken);
        return Ok(result.Response);
    }

    /// <summary>
    /// Refresh access token từ refresh token cookie.
    /// </summary>
    [HttpPost("refresh")]
    [EnableRateLimiting("auth-refresh-token-family")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshTokens(
        [FromHeader(Name = AuthHeaders.IdempotencyKey)] string? idempotencyKey,
        CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[AuthCookieNames.RefreshToken];
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            _authCookieService.ClearAuthCookies(Request, Response);
            return this.UnauthorizedProblem("Missing refresh token.");
        }

        var command = new RefreshTokenCommand
        {
            Token = refreshToken,
            IdempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? Guid.NewGuid().ToString("N") : idempotencyKey,
            ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            DeviceId = ResolveDeviceId(Request.Headers[AuthHeaders.DeviceId].ToString()),
            UserAgentHash = HashValue(Request.Headers.UserAgent.ToString())
        };

        var result = await _mediator.Send(command, cancellationToken);

        _authCookieService.SetAccessToken(Request, Response, result.Response.AccessToken, result.Response.ExpiresInSeconds);
        _authCookieService.SetRefreshToken(Request, Response, result.NewRefreshToken);
        return Ok(result.Response);
    }

    /// <summary>
    /// Logout session hiện tại hoặc toàn bộ sessions.
    /// </summary>
    [HttpPost("logout")]
    [EnableRateLimiting("auth-refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromQuery] bool revokeAll = false, CancellationToken cancellationToken = default)
    {
        var refreshToken = Request.Cookies[AuthCookieNames.RefreshToken];

        var command = new RevokeTokenCommand
        {
            Token = refreshToken ?? string.Empty,
            RevokeAll = revokeAll
        };

        if (revokeAll)
        {
            if (!User.TryGetUserId(out var userId))
            {
                _authCookieService.ClearAuthCookies(Request, Response);
                return this.UnauthorizedProblem("Must be authenticated to revoke all sessions.");
            }

            command.UserId = userId;
        }
        else if (string.IsNullOrWhiteSpace(command.Token))
        {
            _authCookieService.ClearAuthCookies(Request, Response);
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Missing refresh token",
                detail: "No refresh token provided.");
        }

        await _mediator.Send(command, cancellationToken);
        _authCookieService.ClearAuthCookies(Request, Response);
        return Ok(new { message = "Logged out successfully." });
    }

    private static string ResolveDeviceId(string? deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return "unknown-device";
        }

        var trimmed = deviceId.Trim();
        return trimmed.Length <= 128 ? trimmed : trimmed[..128];
    }

    private static string HashValue(string? raw)
    {
        var normalized = string.IsNullOrWhiteSpace(raw) ? "unknown" : raw.Trim();
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
