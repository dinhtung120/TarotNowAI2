/*
 * FILE: TotpMfaService.cs
 * MỤC ĐÍCH: Service TOTP (Time-based One-Time Password) cho MFA (Multi-Factor Authentication).
 *   Cho phép User bật xác thực 2 yếu tố bằng Google Authenticator / Authy.
 *
 *   CÁC CHỨC NĂNG:
 *   → GenerateSecretKey: tạo secret key 20 bytes (Base32) cho User mới bật MFA
 *   → GenerateQrCodeUri: tạo URI chuẩn otpauth:// để quét QR bằng app
 *   → VerifyCode: xác minh mã 6 số từ app (cho phép ±2 time-step = 60s trễ)
 *   → GenerateBackupCodes: tạo 6 mã backup 8 số (dùng khi mất điện thoại)
 *   → EncryptSecret / DecryptSecret: mã hóa/giải mã secret bằng AES-256
 *
 *   BẢO MẬT:
 *   → Secret key được MÃ HÓA bằng AES-256 trước khi lưu vào DB.
 *   → Encryption key đọc từ config → SHA-256 hash → 32 bytes = AES-256 key.
 *   → IV (Initialization Vector) sinh ngẫu nhiên mỗi lần encrypt → cùng plaintext ≠ cùng ciphertext.
 *   → IV được nối vào đầu ciphertext → decrypt tách IV + ciphertext.
 *
 *   TOTP PROTOCOL:
 *   → RFC 6238: mã = f(secret, floor(time/30)) → mỗi 30 giây sinh 1 mã mới.
 *   → VerificationWindow(2, 2): chấp nhận mã ±2 time-step (±60s) → tolerance cho đồng hồ lệch.
 */

using Microsoft.Extensions.Options;
using OtpNet;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Implement IMfaService — TOTP MFA với AES-256 encrypted secrets.
/// </summary>
public class TotpMfaService : IMfaService
{
    private const string Issuer = "TarotNowAI"; // Tên app hiển thị trong Google Authenticator
    private readonly byte[] _encryptionKey; // AES-256 key (32 bytes)

    public TotpMfaService(IOptions<SecurityOptions> options)
    {
        // Đọc encryption key từ config — bắt buộc
        var configuredKey = options.Value.MfaEncryptionKey?.Trim();
        if (string.IsNullOrWhiteSpace(configuredKey))
            throw new InvalidOperationException("Missing Security:MfaEncryptionKey configuration.");

        // Anti-placeholder check: chặn key mặc định trong template
        if (configuredKey.Contains("REPLACE", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Security:MfaEncryptionKey is not configured with a real secret.");

        // Hash config string → 32 bytes (AES-256 key size)
        _encryptionKey = SHA256.HashData(Encoding.UTF8.GetBytes(configuredKey));
    }

    /// <summary>
    /// Tạo secret key mới cho User — 20 bytes random → Base32 encode.
    /// User sẽ nhập key này vào Google Authenticator hoặc quét QR code.
    /// </summary>
    public string GenerateSecretKey()
    {
        var secret = KeyGeneration.GenerateRandomKey(20); // 160 bits
        return Base32Encoding.ToString(secret);
    }

    /// <summary>
    /// Tạo URI chuẩn otpauth:// để sinh QR code.
    /// Format: otpauth://totp/TarotNowAI:user@email.com?secret=ABC123&issuer=TarotNowAI
    /// → User quét QR code bằng Google Authenticator → app tự thêm entry.
    /// </summary>
    public string GenerateQrCodeUri(string plainSecret, string userEmail)
    {
        var secretBytes = Base32Encoding.ToBytes(plainSecret);
        var totp = new Totp(secretBytes);
        
        var encodedIssuer = Uri.EscapeDataString(Issuer);
        var encodedEmail = Uri.EscapeDataString(userEmail);
        
        return $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={plainSecret}&issuer={encodedIssuer}";
    }

    /// <summary>
    /// Xác minh mã TOTP 6 số từ app.
    /// VerificationWindow(2, 2): chấp nhận mã trong khoảng ±2 time-step (±60 giây).
    /// Tại sao ±60s? → Đồng hồ điện thoại có thể lệch vài giây so với server.
    /// </summary>
    public bool VerifyCode(string plainSecret, string code)
    {
        if (string.IsNullOrWhiteSpace(plainSecret) || string.IsNullOrWhiteSpace(code)) return false;

        var secretBytes = Base32Encoding.ToBytes(plainSecret);
        var totp = new Totp(secretBytes);
        
        // VerificationWindow(previous=2, future=2): chấp nhận 5 mã (current ± 2)
        return totp.VerifyTotp(code, out long timeStepMatched, new VerificationWindow(2, 2));
    }

    /// <summary>
    /// Tạo backup codes — dùng khi User mất điện thoại không truy cập app.
    /// Mỗi code = 8 chữ số random (CSPRNG) → User lưu ở nơi an toàn.
    /// Mỗi code chỉ dùng được 1 lần (one-time use).
    /// </summary>
    public List<string> GenerateBackupCodes(int count = 6)
    {
        var result = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var bytes = new byte[4]; // 32 bits entropy
            RandomNumberGenerator.Fill(bytes);
            var code = BitConverter.ToUInt32(bytes, 0) % 100000000; // 8 chữ số
            result.Add(code.ToString("D8")); // Đệm 0 nếu < 8 chữ số
        }
        return result;
    }

    /// <summary>
    /// MÃ HÓA secret bằng AES-256 trước khi lưu vào DB.
    /// → Sinh IV ngẫu nhiên mỗi lần → cùng secret ≠ cùng ciphertext.
    /// → Nối IV + ciphertext → Base64 encode → lưu vào DB.
    /// → Tại sao nối IV? → Khi decrypt cần IV → lưu chung cho tiện.
    /// </summary>
    public string EncryptSecret(string plainSecret)
    {
        using var aes = Aes.Create();
        aes.Key = _encryptionKey;
        aes.GenerateIV(); // IV ngẫu nhiên mỗi lần encrypt

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(plainSecret);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Nối IV (16 bytes) + ciphertext → Base64
        var result = new byte[aes.IV.Length + encryptedBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// GIẢI MÃ secret từ DB — tách IV (16 bytes đầu) + ciphertext → AES decrypt.
    /// Trả về plaintext secret (Base32 string) → dùng cho VerifyCode.
    /// </summary>
    public string DecryptSecret(string encryptedSecret)
    {
        var fullBytes = Convert.FromBase64String(encryptedSecret);

        using var aes = Aes.Create();
        aes.Key = _encryptionKey;

        // Tách IV (16 bytes đầu) khỏi ciphertext
        var iv = new byte[aes.BlockSize / 8]; // 128 bit / 8 = 16 bytes
        var cipherText = new byte[fullBytes.Length - iv.Length];

        Buffer.BlockCopy(fullBytes, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullBytes, iv.Length, cipherText, 0, cipherText.Length);
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
}
