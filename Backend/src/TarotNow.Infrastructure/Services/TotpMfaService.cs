using Microsoft.Extensions.Options;
using OtpNet;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

// Service MFA dựa trên TOTP: sinh secret, verify mã và mã hóa secret lưu trữ.
public partial class TotpMfaService : IMfaService
{
    // Issuer hiển thị trong ứng dụng Authenticator để người dùng nhận diện đúng tài khoản.
    private const string Issuer = "TarotNowAI";
    private const string EncryptionVersionV2Prefix = "v2";
    private const string BackupHashAlgorithm = "pbkdf2-sha256";
    private const string BackupCodeAlphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    // Material key lấy từ cấu hình để dẫn xuất key theo từng secret.
    private readonly byte[] _masterKeyBytes;
    // Key legacy để giải mã dữ liệu MFA cũ trước khi migration lên v2.
    private readonly byte[] _legacyEncryptionKey;
    private readonly int _kdfIterations;
    private readonly int _backupCodeLength;

    /// <summary>
    /// Khởi tạo service MFA và dẫn xuất khóa mã hóa từ cấu hình bảo mật.
    /// Luồng fail-fast khi khóa thiếu hoặc placeholder để tránh lưu secret ở dạng không an toàn.
    /// </summary>
    public TotpMfaService(IOptions<SecurityOptions> options)
    {
        var securityOptions = options.Value;
        // Đọc khóa mã hóa MFA từ cấu hình vận hành.
        var configuredKey = securityOptions.MfaEncryptionKey?.Trim();
        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            throw new InvalidOperationException("Missing Security:MfaEncryptionKey configuration.");
        }

        // Chặn khóa placeholder để giảm rủi ro lộ secret MFA.
        if (configuredKey.Contains("REPLACE", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Security:MfaEncryptionKey is not configured with a real secret.");
        }

        _masterKeyBytes = Encoding.UTF8.GetBytes(configuredKey);
        _legacyEncryptionKey = SHA256.HashData(_masterKeyBytes);
        _kdfIterations = Math.Max(100_000, securityOptions.MfaKdfIterations);
        _backupCodeLength = Math.Clamp(securityOptions.MfaBackupCodeLength, 10, 24);
    }

    /// <summary>
    /// Sinh secret key TOTP mới cho người dùng.
    /// Luồng tạo 20 byte ngẫu nhiên rồi encode Base32 để tương thích app Authenticator.
    /// </summary>
    public string GenerateSecretKey()
    {
        var secret = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(secret);
    }

    /// <summary>
    /// Tạo URI chuẩn `otpauth://` để sinh QR code cấu hình MFA.
    /// Luồng encode issuer/email nhằm tránh lỗi ký tự đặc biệt trong URI.
    /// </summary>
    public string GenerateQrCodeUri(string plainSecret, string userEmail)
    {
        var secretBytes = Base32Encoding.ToBytes(plainSecret);
        var totp = new Totp(secretBytes);
        // Khởi tạo Totp để đảm bảo secret có thể parse hợp lệ trước khi trả URI.
        _ = totp;

        var encodedIssuer = Uri.EscapeDataString(Issuer);
        var encodedEmail = Uri.EscapeDataString(userEmail);

        return $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={plainSecret}&issuer={encodedIssuer}";
    }

    /// <summary>
    /// Xác minh mã TOTP người dùng nhập.
    /// Luồng trả false sớm cho input rỗng và dùng cửa sổ lệch thời gian để giảm lỗi lệch đồng hồ thiết bị.
    /// </summary>
    public bool VerifyCode(string plainSecret, string code)
    {
        if (string.IsNullOrWhiteSpace(plainSecret) || string.IsNullOrWhiteSpace(code))
        {
            // Edge case: thiếu secret hoặc code thì không thể xác minh.
            return false;
        }

        var secretBytes = Base32Encoding.ToBytes(plainSecret);
        var totp = new Totp(secretBytes);

        // Cho phép lệch ±2 bước để giảm false negative khi thời gian client/server lệch nhẹ.
        return totp.VerifyTotp(code, out _, new VerificationWindow(2, 2));
    }

    /// <summary>
    /// Sinh danh sách backup code để đăng nhập khi mất thiết bị MFA.
    /// Luồng tạo code số 8 chữ số từ random bảo mật và trả về theo số lượng yêu cầu.
    /// </summary>
    public List<string> GenerateBackupCodes(int count = 6)
    {
        var result = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            result.Add(GenerateRandomBackupCode(_backupCodeLength));
        }

        return result;
    }

    private static string GenerateRandomBackupCode(int length)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        var builder = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            builder.Append(BackupCodeAlphabet[bytes[i] % BackupCodeAlphabet.Length]);
        }

        return builder.ToString();
    }
}
