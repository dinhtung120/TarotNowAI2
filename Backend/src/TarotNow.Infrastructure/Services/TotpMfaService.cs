using Microsoft.Extensions.Options;
using OtpNet;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

// Service MFA dựa trên TOTP: sinh secret, verify mã và mã hóa secret lưu trữ.
public class TotpMfaService : IMfaService
{
    // Issuer hiển thị trong ứng dụng Authenticator để người dùng nhận diện đúng tài khoản.
    private const string Issuer = "TarotNowAI";
    // Khóa AES dẫn xuất từ cấu hình để mã hóa secret MFA trong database.
    private readonly byte[] _encryptionKey;

    /// <summary>
    /// Khởi tạo service MFA và dẫn xuất khóa mã hóa từ cấu hình bảo mật.
    /// Luồng fail-fast khi khóa thiếu hoặc placeholder để tránh lưu secret ở dạng không an toàn.
    /// </summary>
    public TotpMfaService(IOptions<SecurityOptions> options)
    {
        // Đọc khóa mã hóa MFA từ cấu hình vận hành.
        var configuredKey = options.Value.MfaEncryptionKey?.Trim();
        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            throw new InvalidOperationException("Missing Security:MfaEncryptionKey configuration.");
        }

        // Chặn khóa placeholder để giảm rủi ro lộ secret MFA.
        if (configuredKey.Contains("REPLACE", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Security:MfaEncryptionKey is not configured with a real secret.");
        }

        // Dẫn xuất SHA-256 để lấy key dài cố định, phù hợp yêu cầu AES key bytes.
        _encryptionKey = SHA256.HashData(Encoding.UTF8.GetBytes(configuredKey));
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
        var result = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);
            // Ràng buộc 8 chữ số để dễ nhập tay nhưng vẫn đủ không gian giá trị.
            var code = BitConverter.ToUInt32(bytes, 0) % 100000000;
            result.Add(code.ToString("D8"));
        }
        return result;
    }

    /// <summary>
    /// Mã hóa secret TOTP trước khi lưu trữ.
    /// Luồng dùng AES với IV ngẫu nhiên; kết quả gồm IV + ciphertext dạng Base64.
    /// </summary>
    public string EncryptSecret(string plainSecret)
    {
        using var aes = Aes.Create();
        aes.Key = _encryptionKey;
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(plainSecret);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Ghép IV + ciphertext để phục vụ giải mã về sau trong cùng chuỗi lưu trữ.
        var result = new byte[aes.IV.Length + encryptedBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Giải mã secret TOTP đã lưu.
    /// Luồng tách IV và ciphertext từ payload Base64 rồi giải mã bằng cùng khóa AES dẫn xuất.
    /// </summary>
    public string DecryptSecret(string encryptedSecret)
    {
        var fullBytes = Convert.FromBase64String(encryptedSecret);

        using var aes = Aes.Create();
        aes.Key = _encryptionKey;

        // Tách block đầu làm IV, phần còn lại là ciphertext.
        var iv = new byte[aes.BlockSize / 8];
        var cipherText = new byte[fullBytes.Length - iv.Length];

        Buffer.BlockCopy(fullBytes, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullBytes, iv.Length, cipherText, 0, cipherText.Length);
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
}
