namespace TarotNow.Api.Options;

public sealed class RateLimitPoliciesOptions
{
    public RateLimitWindowOptions AuthLogin { get; set; } = new() { PermitLimit = 5, WindowSeconds = 60 };
    public RateLimitWindowOptions AuthRegister { get; set; } = new() { PermitLimit = 5, WindowSeconds = 60 };
    public RateLimitWindowOptions AuthPassword { get; set; } = new() { PermitLimit = 5, WindowSeconds = 60 };
    public RateLimitWindowOptions AuthMfaChallenge { get; set; } = new() { PermitLimit = 10, WindowSeconds = 60 };
    public RateLimitWindowOptions AuthSession { get; set; } = new() { PermitLimit = 100, WindowSeconds = 60 };
    public RateLimitWindowOptions AuthRefresh { get; set; } = new() { PermitLimit = 30, WindowSeconds = 60 };
    public RateLimitWindowOptions AuthRefreshTokenFamily { get; set; } = new() { PermitLimit = 10, WindowSeconds = 60 };
    public RateLimitWindowOptions AuthLogout { get; set; } = new() { PermitLimit = 30, WindowSeconds = 60 };
    public RateLimitWindowOptions WithdrawalCreate { get; set; } = new() { PermitLimit = 8, WindowSeconds = 60 };
    public RateLimitWindowOptions PaymentCreateOrder { get; set; } = new() { PermitLimit = 10, WindowSeconds = 60 };
    public RateLimitWindowOptions ReportCreate { get; set; } = new() { PermitLimit = 20, WindowSeconds = 60 };
    public RateLimitWindowOptions CommunityWrite { get; set; } = new() { PermitLimit = 60, WindowSeconds = 60 };
    public RateLimitWindowOptions ChatStandard { get; set; } = new() { PermitLimit = 200, WindowSeconds = 60 };
    public RateLimitWindowOptions PaymentWebhook { get; set; } = new() { PermitLimit = 40, WindowSeconds = 60 };
    public RateLimitHashOptions HashPrefixes { get; set; } = new();
}

public sealed class RateLimitWindowOptions
{
    public int PermitLimit { get; set; }
    public int WindowSeconds { get; set; }
}

public sealed class RateLimitHashOptions
{
    public int RefreshDeviceLength { get; set; } = 24;
    public int RefreshTokenLength { get; set; } = 16;
}
