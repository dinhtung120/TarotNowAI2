namespace TarotNow.Infrastructure.Options;

// Options cấu hình khóa bảo mật nội bộ.
public sealed class SecurityOptions
{
    // Khóa mã hóa secret MFA.
    public string MfaEncryptionKey { get; set; } = string.Empty;
}
