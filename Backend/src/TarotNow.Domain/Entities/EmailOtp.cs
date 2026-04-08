

using System.Security.Cryptography;
using System.Text;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

// Entity OTP email để quản lý mã xác thực một lần theo người dùng và thời hạn hiệu lực.
public class EmailOtp
{
    // Định danh OTP.
    public Guid Id { get; private set; }

    // Người dùng sở hữu OTP.
    public Guid UserId { get; private set; }

    // Mã OTP đã hash (hoặc plain legacy để tương thích ngược).
    public string OtpCode { get; private set; } = string.Empty;

    // Loại OTP theo nghiệp vụ (verify/reset/...).
    public string Type { get; private set; } = string.Empty;

    // Thời điểm OTP hết hiệu lực.
    public DateTime ExpiresAt { get; private set; }

    // Cờ đánh dấu OTP đã sử dụng.
    public bool IsUsed { get; private set; }

    // Thời điểm tạo OTP.
    public DateTime CreatedAt { get; private set; }

    // Navigation tới user.
    public User User { get; private set; } = null!;

    // Cờ tiện ích kiểm tra hết hạn theo UTC hiện tại.
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu đã lưu.
    /// </summary>
    protected EmailOtp() { }

    /// <summary>
    /// Khởi tạo OTP mới cho người dùng với thời hạn mặc định 15 phút.
    /// Luồng xử lý: sinh id, hash otpCode, gán loại OTP và thiết lập mốc thời gian hiệu lực.
    /// </summary>
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
    /// Đánh dấu OTP đã dùng để ngăn tái sử dụng mã cũ.
    /// Luồng xử lý: cập nhật cờ IsUsed sang true trên entity hiện tại.
    /// </summary>
    public void MarkAsUsed()
    {
        IsUsed = true;
        // Khóa OTP ngay sau khi xác thực thành công để đảm bảo one-time usage.
    }

    /// <summary>
    /// Xác minh mã OTP đầu vào với giá trị đã lưu theo cơ chế chống timing attack.
    /// Luồng xử lý: chặn input rỗng, hỗ trợ legacy plain text, còn lại hash và so sánh fixed-time.
    /// </summary>
    public bool VerifyCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            // Edge case: input rỗng luôn bị từ chối để tránh bypass xác thực.
            return false;
        }

        if (OtpCode.Length <= 10)
        {
            // Nhánh tương thích dữ liệu cũ từng lưu plain OTP thay vì hash.
            return string.Equals(OtpCode, code, StringComparison.Ordinal);
        }

        var hashedInput = HashCode(code);

        // Dùng so sánh constant-time để giảm rò rỉ thông tin qua timing.
        return FixedTimeEquals(OtpCode, hashedInput);
    }

    /// <summary>
    /// Băm OTP bằng SHA-256 để không lưu mã xác thực ở dạng thô.
    /// Luồng xử lý: trim input, hash bytes UTF-8 và trả chuỗi hex lowercase.
    /// </summary>
    private static string HashCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>
    /// So sánh hai chuỗi ở thời gian gần cố định để hạn chế timing attack.
    /// Luồng xử lý: chuyển chuỗi sang bytes, kiểm tra độ dài và dùng API FixedTimeEquals.
    /// </summary>
    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
            && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
