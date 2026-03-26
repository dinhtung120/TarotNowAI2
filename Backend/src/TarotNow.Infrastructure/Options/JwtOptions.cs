namespace TarotNow.Infrastructure.Options;

public sealed class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; } = 15;
    public int RefreshExpiryDays { get; set; } = 7;
}

