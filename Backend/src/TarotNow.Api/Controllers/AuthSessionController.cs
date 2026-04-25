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
    private const string LogoutSuccessMessage = "Logged out successfully.";
    private readonly IMediator _mediator;
    private readonly IAuthCookieService _authCookieService;
    private readonly IForwardedHeaderTrustEvaluator _forwardedHeaderTrustEvaluator;

    /// <summary>
    /// Khởi tạo auth session controller.
    /// </summary>
    public AuthSessionController(
        IMediator mediator,
        IAuthCookieService authCookieService,
        IForwardedHeaderTrustEvaluator forwardedHeaderTrustEvaluator)
    {
        _mediator = mediator;
        _authCookieService = authCookieService;
        _forwardedHeaderTrustEvaluator = forwardedHeaderTrustEvaluator;
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
        command.DeviceId = ResolveDeviceId(Request);
        command.UserAgentHash = HashValue(ResolveUserAgent(Request));

        var result = await _mediator.Send(command, cancellationToken);

        _authCookieService.SetAccessToken(Request, Response, result.Response.AccessToken, result.Response.ExpiresInSeconds);
        _authCookieService.SetRefreshToken(Request, Response, result.RefreshToken, result.RefreshTokenExpiresAtUtc);
        return Ok(result.Response);
    }

    /// <summary>
    /// Refresh access token từ refresh token cookie.
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

        var deviceId = ResolveDeviceId(Request);
        var userAgentHash = HashValue(ResolveUserAgent(Request));

        var command = new RefreshTokenCommand
        {
            Token = refreshToken,
            IdempotencyKey = ResolveRefreshIdempotencyKey(refreshToken, deviceId, userAgentHash),
            ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            DeviceId = deviceId,
            UserAgentHash = userAgentHash
        };

        var result = await _mediator.Send(command, cancellationToken);

        _authCookieService.SetAccessToken(Request, Response, result.Response.AccessToken, result.Response.ExpiresInSeconds);
        _authCookieService.SetRefreshToken(Request, Response, result.NewRefreshToken, result.RefreshTokenExpiresAtUtc);
        return Ok(result.Response);
    }

    /// <summary>
    /// Logout session hiện tại hoặc toàn bộ sessions.
    /// </summary>
    [HttpPost("logout")]
    [EnableRateLimiting("auth-logout")]
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
            if (User.TryGetUserId(out var userId))
            {
                command.UserId = userId;
            }
            else if (string.IsNullOrWhiteSpace(command.Token))
            {
                _authCookieService.ClearAuthCookies(Request, Response);
                return this.UnauthorizedProblem("Must be authenticated or provide refresh token to revoke all sessions.");
            }
        }
        else if (string.IsNullOrWhiteSpace(command.Token))
        {
            _authCookieService.ClearAuthCookies(Request, Response);
            return this.UnauthorizedProblem("Missing refresh token.");
        }

        await _mediator.Send(command, cancellationToken);
        _authCookieService.ClearAuthCookies(Request, Response);
        return Ok(new { success = true, message = LogoutSuccessMessage });
    }

    private static string ResolveDeviceId(HttpRequest request)
    {
        var headerValue = request.Headers[AuthHeaders.DeviceId].ToString();
        if (TryNormalizeDeviceId(headerValue, out var normalizedFromHeader))
        {
            return normalizedFromHeader;
        }

        if (request.Cookies.TryGetValue(AuthCookieNames.DeviceId, out var cookieValue)
            && TryNormalizeDeviceId(cookieValue, out var normalizedFromCookie))
        {
            return normalizedFromCookie;
        }

        // Fallback deterministic fingerprint để tránh gom mọi client vào một unknown-device.
        var ip = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip";
        var ua = request.Headers.UserAgent.ToString();
        return $"fp-{HashValue($"{ip}|{ua}")}";
    }

    private static bool TryNormalizeDeviceId(string? raw, out string normalized)
    {
        normalized = string.Empty;
        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }

        var trimmed = raw.Trim();
        normalized = trimmed.Length <= 128 ? trimmed : trimmed[..128];
        return normalized.Length > 0;
    }

    private string ResolveUserAgent(HttpRequest request)
    {
        if (_forwardedHeaderTrustEvaluator.IsTrustedProxy(request.HttpContext.Connection.RemoteIpAddress))
        {
            var forwardedUserAgent = request.Headers[AuthHeaders.ForwardedUserAgent].ToString();
            if (string.IsNullOrWhiteSpace(forwardedUserAgent) == false)
            {
                return forwardedUserAgent;
            }
        }

        return request.Headers.UserAgent.ToString();
    }

    private static string HashValue(string? raw)
    {
        var normalized = string.IsNullOrWhiteSpace(raw) ? "unknown" : raw.Trim();
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string ResolveRefreshIdempotencyKey(
        string refreshToken,
        string deviceId,
        string userAgentHash)
    {
        var source = $"{refreshToken.Trim()}|{deviceId.Trim()}|{userAgentHash.Trim()}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(source));
        return $"auto-{Convert.ToHexString(hash).ToLowerInvariant()}";
    }
}
