namespace TarotNow.Application.Interfaces;

/// <summary>
/// Service xử lý Multi-Factor Authentication (TOTP).
/// Phase 2.5 — MFA.
/// </summary>
public interface IMfaService
{
    /// <summary>
    /// Tạo một TOTP secret ngẫu nhiên (chưa mã hóa).
    /// </summary>
    string GenerateSecretKey();

    /// <summary>
    /// Mã hóa secret để lưu vào database (mfa_secret_encrypted).
    /// </summary>
    string EncryptSecret(string plainSecret);

    /// <summary>
    /// Giải mã secret từ database.
    /// </summary>
    string DecryptSecret(string encryptedSecret);

    /// <summary>
    /// Tạo URI chuẩn otpauth:// để cho app (Google Authenticator) quét mã QR.
    /// </summary>
    string GenerateQrCodeUri(string plainSecret, string userEmail);

    /// <summary>
    /// Kiểm tra mã TOTP người dùng nhập có đúng không.
    /// </summary>
    bool VerifyCode(string plainSecret, string code);

    /// <summary>
    /// Tạo danh sách các mã backup (chỉ dùng 1 lần) để phòng hờ mất thiết bị.
    /// </summary>
    List<string> GenerateBackupCodes(int count = 6);
}
