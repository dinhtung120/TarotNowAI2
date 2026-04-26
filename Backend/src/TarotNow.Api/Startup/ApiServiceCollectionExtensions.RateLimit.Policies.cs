using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Options;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    private static void AddLoginPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-login",
            ResolveClientIp,
            permitLimit: policies.AuthLogin.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthLogin.WindowSeconds));
    }

    private static void AddRegisterPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-register",
            ResolveClientIp,
            permitLimit: policies.AuthRegister.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthRegister.WindowSeconds));
    }

    private static void AddPasswordPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-password",
            ResolveClientIp,
            permitLimit: policies.AuthPassword.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthPassword.WindowSeconds));
    }

    private static void AddMfaChallengePolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-mfa-challenge",
            ResolveAuthenticatedPartitionKey,
            permitLimit: policies.AuthMfaChallenge.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthMfaChallenge.WindowSeconds));
    }

    private static void AddSessionPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-session",
            ResolveAuthenticatedPartitionKey,
            permitLimit: policies.AuthSession.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthSession.WindowSeconds));
    }

    private static void AddRefreshPolicies(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-refresh",
            ctx => ResolveRefreshPartitionKey(
                ctx,
                policies.HashPrefixes.RefreshDeviceLength,
                policies.HashPrefixes.RefreshTokenLength),
            permitLimit: policies.AuthRefresh.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthRefresh.WindowSeconds));
        AddFixedWindowPolicy(
            options,
            "auth-refresh-token-family",
            ctx => ResolveRefreshFamilyKey(
                ctx,
                policies.HashPrefixes.RefreshDeviceLength,
                policies.HashPrefixes.RefreshTokenLength),
            permitLimit: policies.AuthRefreshTokenFamily.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthRefreshTokenFamily.WindowSeconds));
        AddFixedWindowPolicy(
            options,
            "auth-logout",
            ctx => ResolveRefreshPartitionKey(
                ctx,
                policies.HashPrefixes.RefreshDeviceLength,
                policies.HashPrefixes.RefreshTokenLength),
            permitLimit: policies.AuthLogout.PermitLimit,
            window: TimeSpan.FromSeconds(policies.AuthLogout.WindowSeconds));
    }

    private static void AddWithdrawalCreatePolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "withdrawal-create",
            ResolveAuthenticatedPartitionKey,
            permitLimit: policies.WithdrawalCreate.PermitLimit,
            window: TimeSpan.FromSeconds(policies.WithdrawalCreate.WindowSeconds));
    }

    private static void AddPaymentCreateOrderPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "payment-create-order",
            ResolveAuthenticatedPartitionKey,
            permitLimit: policies.PaymentCreateOrder.PermitLimit,
            window: TimeSpan.FromSeconds(policies.PaymentCreateOrder.WindowSeconds));
    }

    private static void AddReportCreatePolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "report-create",
            ResolveAuthenticatedPartitionKey,
            permitLimit: policies.ReportCreate.PermitLimit,
            window: TimeSpan.FromSeconds(policies.ReportCreate.WindowSeconds));
    }

    private static void AddCommunityPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "community-write",
            ResolveAuthenticatedPartitionKey,
            permitLimit: policies.CommunityWrite.PermitLimit,
            window: TimeSpan.FromSeconds(policies.CommunityWrite.WindowSeconds));
    }

    private static void AddChatPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "chat-standard",
            ResolveAuthenticatedPartitionKey,
            permitLimit: policies.ChatStandard.PermitLimit,
            window: TimeSpan.FromSeconds(policies.ChatStandard.WindowSeconds));
    }

    private static void AddPaymentWebhookPolicy(RateLimiterOptions options, RateLimitPoliciesOptions policies)
    {
        AddFixedWindowPolicy(
            options,
            "payment-webhook",
            ResolveClientIp,
            permitLimit: policies.PaymentWebhook.PermitLimit,
            window: TimeSpan.FromSeconds(policies.PaymentWebhook.WindowSeconds));
    }
}
