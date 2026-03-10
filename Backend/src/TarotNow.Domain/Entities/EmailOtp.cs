using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity lưu trữ mã xác thực OTP dùng để verify Email hoặc Reset Password.
/// </summary>
public class EmailOtp
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string OtpCode { get; private set; }
    public string Type { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation Property
    public User User { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    protected EmailOtp() { } // Dành cho EF Core

    public EmailOtp(Guid userId, string otpCode, string type, int expiryMinutes = 15)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OtpCode = otpCode;
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
}
