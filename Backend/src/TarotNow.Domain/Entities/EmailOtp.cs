using System.Security.Cryptography;
using System.Text;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity lưu trữ mã xác thực OTP dùng để verify Email hoặc Reset Password.
/// </summary>
public class EmailOtp
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string OtpCode { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation Property
    public User User { get; private set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    protected EmailOtp() { } // Dành cho EF Core

    public EmailOtp(Guid userId, string otpCode, string type, int expiryMinutes = 15)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OtpCode = HashCode(otpCode);
        Type = type;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
        IsUsed = false;
    }

    /// <summary>
    /// Đánh dấu xem OTP đã dùng chưa. 
    /// OTP chỉ cho phép xài đúng 1 lần.
    /// </summary>
    public void MarkAsUsed()
    {
        IsUsed = true;
    }

    public bool VerifyCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;

        // Backward compatibility cho OTP plaintext đã phát hành trước khi áp dụng hashing.
        if (OtpCode.Length <= 10)
            return string.Equals(OtpCode, code, StringComparison.Ordinal);

        var hashedInput = HashCode(code);
        return FixedTimeEquals(OtpCode, hashedInput);
    }

    private static string HashCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
            && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
