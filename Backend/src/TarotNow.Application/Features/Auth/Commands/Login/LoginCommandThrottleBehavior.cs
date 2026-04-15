using System;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Pipeline behavior kiểm soát throttle đăng nhập theo identity và IP.
/// </summary>
public sealed class LoginCommandThrottleBehavior : IPipelineBehavior<LoginCommand, LoginResult>
{
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Khởi tạo behavior throttle cho login.
    /// </summary>
    public LoginCommandThrottleBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <inheritdoc />
    public async Task<LoginResult> Handle(
        LoginCommand request,
        RequestHandlerDelegate<LoginResult> next,
        CancellationToken cancellationToken)
    {
        await EnsureLoginThrottleNotExceededAsync(request, cancellationToken);
        try
        {
            var response = await next();
            await ClearLoginFailureCountersAsync(request, cancellationToken);
            return response;
        }
        catch (BusinessRuleException ex) when (ShouldCountAsFailedAttempt(ex.ErrorCode))
        {
            await IncreaseLoginFailureCountersAsync(request, cancellationToken);
            throw;
        }
    }

    private async Task EnsureLoginThrottleNotExceededAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        var identityHash = HashIdentity(request.EmailOrUsername);
        var ipHash = HashIpAddress(request.ClientIpAddress);
        var identityCount = await _cacheService.GetAsync<long>(BuildLoginIdentityFailureKey(identityHash), cancellationToken);
        if (identityCount >= AuthSecurityPolicyConstants.LoginIdentityFailureLimit)
        {
            throw new BusinessRuleException(AuthErrorCodes.RateLimited, "Too many failed login attempts. Please try again later.");
        }

        var ipCount = await _cacheService.GetAsync<long>(BuildLoginIpFailureKey(ipHash), cancellationToken);
        if (ipCount >= AuthSecurityPolicyConstants.LoginIpFailureLimit)
        {
            throw new BusinessRuleException(AuthErrorCodes.RateLimited, "Too many failed login attempts. Please try again later.");
        }
    }

    private async Task IncreaseLoginFailureCountersAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        var window = AuthSecurityPolicyConstants.LoginFailureWindow;
        await _cacheService.IncrementAsync(
            BuildLoginIdentityFailureKey(HashIdentity(request.EmailOrUsername)),
            window,
            cancellationToken);
        await _cacheService.IncrementAsync(
            BuildLoginIpFailureKey(HashIpAddress(request.ClientIpAddress)),
            window,
            cancellationToken);
    }

    private async Task ClearLoginFailureCountersAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(BuildLoginIdentityFailureKey(HashIdentity(request.EmailOrUsername)), cancellationToken);
        await _cacheService.RemoveAsync(BuildLoginIpFailureKey(HashIpAddress(request.ClientIpAddress)), cancellationToken);
    }

    private static string BuildLoginIdentityFailureKey(string identityHash)
    {
        return $"auth:login-fail:identity:{identityHash}";
    }

    private static string BuildLoginIpFailureKey(string ipHash)
    {
        return $"auth:login-fail:ip:{ipHash}";
    }

    private static bool ShouldCountAsFailedAttempt(string errorCode)
    {
        return string.Equals(errorCode, AuthErrorCodes.Unauthorized, StringComparison.Ordinal)
               || string.Equals(errorCode, AuthErrorCodes.UserBlocked, StringComparison.Ordinal);
    }

    private static string HashIdentity(string? identity)
    {
        var normalizedIdentity = string.IsNullOrWhiteSpace(identity)
            ? "unknown"
            : identity.Trim().ToLowerInvariant();
        return HashValue(normalizedIdentity);
    }

    private static string HashIpAddress(string? ipAddress)
    {
        var normalizedIp = string.IsNullOrWhiteSpace(ipAddress)
            ? "unknown"
            : ipAddress.Trim();
        return HashValue(normalizedIp);
    }

    private static string HashValue(string raw)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
