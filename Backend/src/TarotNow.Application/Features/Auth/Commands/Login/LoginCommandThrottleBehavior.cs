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
        var response = await next();
        await ClearLoginFailureCountersAsync(request, cancellationToken);
        return response;
    }

    private async Task EnsureLoginThrottleNotExceededAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        var identityHash = HashValue(request.EmailOrUsername);
        var ipHash = HashValue(request.ClientIpAddress);
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

    private async Task ClearLoginFailureCountersAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(BuildLoginIdentityFailureKey(HashValue(request.EmailOrUsername)), cancellationToken);
        await _cacheService.RemoveAsync(BuildLoginIpFailureKey(HashValue(request.ClientIpAddress)), cancellationToken);
    }

    private static string BuildLoginIdentityFailureKey(string identityHash)
    {
        return $"auth:login-fail:identity:{identityHash}";
    }

    private static string BuildLoginIpFailureKey(string ipHash)
    {
        return $"auth:login-fail:ip:{ipHash}";
    }

    private static string HashValue(string? raw)
    {
        var normalized = string.IsNullOrWhiteSpace(raw) ? "unknown" : raw.Trim();
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
