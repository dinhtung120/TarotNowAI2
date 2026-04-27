namespace TarotNow.Infrastructure.Options;

// Options cấu hình khóa bảo mật nội bộ.
public sealed class SecurityOptions
{
    // Khóa mã hóa secret MFA.
    public string MfaEncryptionKey { get; set; } = string.Empty;

    // Khóa mã hóa dữ liệu nhạy cảm tài chính (fallback dùng MfaEncryptionKey nếu để trống).
    public string WithdrawalDataEncryptionKey { get; set; } = string.Empty;

    // Số vòng lặp PBKDF2 để dẫn xuất khóa MFA.
    public int MfaKdfIterations { get; set; } = 210_000;

    // Độ dài backup code MFA.
    public int MfaBackupCodeLength { get; set; } = 12;

    // Bật one-off backfill mã hóa dữ liệu withdrawal cũ trong maintenance window.
    public bool EnableWithdrawalEncryptionBackfill { get; set; }
}
