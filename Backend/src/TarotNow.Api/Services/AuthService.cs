using System.Security.Cryptography;
using System.Text;
using MediatR;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Features.Auth.Commands.Logout;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;

namespace TarotNow.Api.Services;

public sealed class AuthService : IAuthService
{
    private readonly IMediator _mediator;
    private readonly IForwardedHeaderTrustEvaluator _forwardedHeaderTrustEvaluator;

    public AuthService(
        IMediator mediator,
        IForwardedHeaderTrustEvaluator forwardedHeaderTrustEvaluator)
    {
        _mediator = mediator;
        _forwardedHeaderTrustEvaluator = forwardedHeaderTrustEvaluator;
    }

    public Task<LoginResult> LoginAsync(
        LoginCommand command,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var metadata = ResolveRequestMetadata(httpContext.Request);
        command.ClientIpAddress = ResolveClientIp(httpContext);
        command.DeviceId = metadata.DeviceId;
        command.UserAgentHash = HashValue(metadata.UserAgent);
        return _mediator.SendWithRequestCancellation(httpContext, command, cancellationToken);
    }

    public Task<RefreshTokenResult> RefreshAsync(
        string refreshToken,
        string? idempotencyKey,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var metadata = ResolveRequestMetadata(httpContext.Request);
        var userAgentHash = HashValue(metadata.UserAgent);
        var command = new RefreshTokenCommand
        {
            Token = refreshToken,
            IdempotencyKey = ResolveRefreshIdempotencyKey(idempotencyKey, refreshToken, metadata.DeviceId, userAgentHash),
            ClientIpAddress = ResolveClientIp(httpContext),
            DeviceId = metadata.DeviceId,
            UserAgentHash = userAgentHash
        };
        return _mediator.SendWithRequestCancellation(httpContext, command, cancellationToken);
    }

    public Task<bool> LogoutAsync(
        string refreshToken,
        bool revokeAll,
        Guid? authenticatedUserId,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var command = new LogoutCommand
        {
            Token = refreshToken,
            RevokeAll = revokeAll,
            UserId = authenticatedUserId
        };
        return _mediator.SendWithRequestCancellation(httpContext, command, cancellationToken);
    }

    private RequestMetadata ResolveRequestMetadata(HttpRequest request)
    {
        return new RequestMetadata(
            DeviceId: ResolveDeviceId(request),
            UserAgent: ResolveUserAgent(request));
    }

    private string ResolveUserAgent(HttpRequest request)
    {
        if (_forwardedHeaderTrustEvaluator.IsTrustedProxy(request.HttpContext.Connection.RemoteIpAddress))
        {
            var forwardedUserAgent = request.Headers[AuthHeaders.ForwardedUserAgent].ToString();
            if (!string.IsNullOrWhiteSpace(forwardedUserAgent))
            {
                return forwardedUserAgent;
            }
        }

        return request.Headers.UserAgent.ToString();
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

        var ip = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip";
        var userAgent = request.Headers.UserAgent.ToString();
        return $"fp-{HashValue($"{ip}|{userAgent}")}";
    }

    private static string ResolveClientIp(HttpContext httpContext)
    {
        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private static string ResolveRefreshIdempotencyKey(
        string? rawKey,
        string refreshToken,
        string deviceId,
        string userAgentHash)
    {
        if (!string.IsNullOrWhiteSpace(rawKey))
        {
            var trimmed = rawKey.Trim();
            return trimmed.Length <= 128 ? trimmed : trimmed[..128];
        }

        var source = $"{refreshToken.Trim()}|{deviceId.Trim()}|{userAgentHash.Trim()}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(source));
        return $"auto-{Convert.ToHexString(hash).ToLowerInvariant()}";
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

    private static string HashValue(string? raw)
    {
        var normalized = string.IsNullOrWhiteSpace(raw) ? "unknown" : raw.Trim();
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private readonly record struct RequestMetadata(string DeviceId, string UserAgent);
}
