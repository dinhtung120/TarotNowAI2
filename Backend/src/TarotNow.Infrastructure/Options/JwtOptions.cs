namespace TarotNow.Infrastructure.Options;

// Options cấu hình phát hành JWT cho hệ thống xác thực.
public sealed class JwtOptions
{
    // Secret ký JWT.
    public string SecretKey { get; set; } = string.Empty;

    // Issuer của token.
    public string Issuer { get; set; } = string.Empty;

    // Audience hợp lệ của token.
    public string Audience { get; set; } = string.Empty;

    // Thời hạn access token (phút).
    public int ExpiryMinutes { get; set; } = 15;

    // Thời hạn refresh token (ngày).
    public int RefreshExpiryDays { get; set; } = 7;
}
