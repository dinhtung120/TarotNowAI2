using Microsoft.Extensions.Configuration;
using TarotNow.Api.Options;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    private static RateLimitPoliciesOptions ResolveRateLimitPolicies(IConfiguration configuration)
    {
        var configured = configuration.GetSection("RateLimitPolicies").Get<RateLimitPoliciesOptions>() ?? new();
        var hashPrefixes = configured.HashPrefixes ?? new RateLimitHashOptions();
        return new RateLimitPoliciesOptions
        {
            AuthLogin = NormalizeWindow(configured.AuthLogin, permitLimit: 5, windowSeconds: 60),
            AuthRegister = NormalizeWindow(configured.AuthRegister, permitLimit: 5, windowSeconds: 60),
            AuthPassword = NormalizeWindow(configured.AuthPassword, permitLimit: 5, windowSeconds: 60),
            AuthMfaChallenge = NormalizeWindow(configured.AuthMfaChallenge, permitLimit: 10, windowSeconds: 60),
            AuthSession = NormalizeWindow(configured.AuthSession, permitLimit: 100, windowSeconds: 60),
            AuthRefresh = NormalizeWindow(configured.AuthRefresh, permitLimit: 30, windowSeconds: 60),
            AuthRefreshTokenFamily = NormalizeWindow(configured.AuthRefreshTokenFamily, permitLimit: 10, windowSeconds: 60),
            AuthLogout = NormalizeWindow(configured.AuthLogout, permitLimit: 30, windowSeconds: 60),
            WithdrawalCreate = NormalizeWindow(configured.WithdrawalCreate, permitLimit: 8, windowSeconds: 60),
            PaymentCreateOrder = NormalizeWindow(configured.PaymentCreateOrder, permitLimit: 10, windowSeconds: 60),
            ReportCreate = NormalizeWindow(configured.ReportCreate, permitLimit: 20, windowSeconds: 60),
            CommunityWrite = NormalizeWindow(configured.CommunityWrite, permitLimit: 60, windowSeconds: 60),
            ChatStandard = NormalizeWindow(configured.ChatStandard, permitLimit: 200, windowSeconds: 60),
            PaymentWebhook = NormalizeWindow(configured.PaymentWebhook, permitLimit: 40, windowSeconds: 60),
            HashPrefixes = new RateLimitHashOptions
            {
                RefreshDeviceLength = NormalizeHashLength(hashPrefixes.RefreshDeviceLength, fallback: 24),
                RefreshTokenLength = NormalizeHashLength(hashPrefixes.RefreshTokenLength, fallback: 16)
            }
        };
    }

    private static RateLimitWindowOptions NormalizeWindow(
        RateLimitWindowOptions? configured,
        int permitLimit,
        int windowSeconds)
    {
        if (configured is null)
        {
            return new RateLimitWindowOptions
            {
                PermitLimit = permitLimit,
                WindowSeconds = windowSeconds
            };
        }

        return new RateLimitWindowOptions
        {
            PermitLimit = configured.PermitLimit > 0 ? configured.PermitLimit : permitLimit,
            WindowSeconds = configured.WindowSeconds > 0 ? configured.WindowSeconds : windowSeconds
        };
    }

    private static int NormalizeHashLength(int value, int fallback)
    {
        var normalized = value > 0 ? value : fallback;
        return Math.Max(HashPrefixMinLength, normalized);
    }
}
