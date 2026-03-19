/*
 * ===================================================================
 * FILE: EmailOtp.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Mã 6 Só Vàng Ló Kẹt Nơi DB Bám Ngựa Dùng Kêu Khách Rửa Mail (Mail OTP).
 *   Sử Dụng Chủ Yếu Dò Xác Thực Register Đăng Kí Có Đúng Chủ Hay Khôi Phục Forgot Pass.
 * ===================================================================
 */

using System.Security.Cryptography;
using System.Text;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity SQL `email_otps` Kho Giam Lại Bột Mật Mã Bắt Bền Giữ Để Kiểm Duyệt Hòm Thư Đúng Chính Chủ (One-Time Password Pumping).
/// Cấu Cắt Chẻ Sẵn Cả Mớ Băm Hash SHA256 Chống Hách Nhìn Thấy Vào DB Đọc Mất Mã Xin Lừa Rút Quyền.
/// </summary>
public class EmailOtp
{
    // Cột Đinh Chữ Mã ID Quản Lý.
    public Guid Id { get; private set; }
    // Gắn Gấp Với Thằng Trọ Đang Bị Giam Vào Mức OTP.
    public Guid UserId { get; private set; }
    
    // Cái Cột Lòng Chữ Lưu Đã Ra Khuôn 6 Số Của Khách Băm Cục (Không Sinh Lưu Rõ Số Cho An Toàn SQL).
    public string OtpCode { get; private set; } = string.Empty;
    // Cớ Gì Đòi Mã (Quyên Pass/ Xác Nhận Account Reset_Password / Verify Email ...).
    public string Type { get; private set; } = string.Empty;
    
    // Hạt Nổ Đồng Hồ Bom Cát (Thường Cho 15 Phút Khách Lấu Phải Vã Qua Hết Đòi Bỏ Xin Mã Mới).
    public DateTime ExpiresAt { get; private set; }
    // Thẻ Dấu 1 Lần Nhấn Xong Nổ Ra "Đã Dùng" Không Hồi Sinh Dùng 1 Mật Mã Lọc Cho 2 Khách Cùng Lúc Lừa Quên Pass Rồi Đổi Trộm Email Lại).
    public bool IsUsed { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Rãnh Sinh Kết Thằng Cáo (User).
    public User User { get; private set; } = null!;

    // Cổng Thở Biết Ngay Khách Đã Kêu Nhầm Mã Chua Lét (Phán Hạn Tức Thời Tại Ram Trả Về True/False Nếu Giờ Vượt Hạn).
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    protected EmailOtp() { } // Dành cho Búa Phá Mộng EF Core Nó Gọi Tự Kẹp Dữ Liệu SQL Ở Lõi Rỗng Lên.

    /// <summary>
    /// Vành Chân Bấm Đầu Hóa Dõi Mã Của Tên (Kéo Nó Thành Hash An Tâm Rồi Bưng Đọc Sinh Rơi Xuống Dưới Entity Chờ Gọi Lên Kêu).
    /// </summary>
    public EmailOtp(Guid userId, string otpCode, string type, int expiryMinutes = 15)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OtpCode = HashCode(otpCode); // An Ninh Ngay Tụ Khỏi Trễ Ra Sql
        Type = type;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
        IsUsed = false;
    }

    /// <summary>
    /// Bọc Lệnh Bắn Súng: Cát Nổ Tách Phá Trinh Trắng OTP Này Mãi Bị Thả Trôi Không Được Cầm Nhét Vô Lừa Trượt Reset Pass Lần Thứ N.
    /// </summary>
    public void MarkAsUsed()
    {
        IsUsed = true;
    }

    /// <summary>
    /// Tay Vung Nắm Số So Khớp Hạt Mã (Khách Viết Chữ Trắng Vào So Có Đúng Số Hash Băm Sẵn Đang Dính DB Hay Lừa Vớ Vẩn).
    /// Kèm Hàng Rào Phòng Xuyên Thời Gian FixedTimeEquals So Cho Hacker Không Dò Trễ Từng Số Để Mò Xỉ Code Tốc Độ Bọc Từng Millisecond Oan Nghiệt.
    /// </summary>
    public bool VerifyCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;

        // Trả Về Thời Quá Khứ: Do Hệ Gắn Sau Khi Nâng Security Còn Mấy OTP Cũ Nằm SQL Xài Số 6 Chữ Bình Thường Chuyển Sang Kèo Băm Luôn Đoạt Check Cũ 1 Phát Nếu Mới Kéo Qua Kỷ Nguyên Mới.
        if (OtpCode.Length <= 10)
            return string.Equals(OtpCode, code, StringComparison.Ordinal);

        var hashedInput = HashCode(code);
        // Trò So Lẩn Cảo Trách Đứt Quãng Lệnh Xào Thời Gian Đều Bước Nhịp Hacker Ko Mò Nổi Password Từ Timing Attack Của Hàm Equals Bình Thường.
        return FixedTimeEquals(OtpCode, hashedInput);
    }

    /// <summary>Vò Quát SHA256 Code Làm Cho Database Mù Chỉ Biết 1 Chuỗi Bỏ Mã Rác Vô Vọng.</summary>
    private static string HashCode(string code)
    {
        var normalized = code.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>Độ Dốc Thời Gian Trượt Đồng Cấp Timing Attack Thần Tiên (Ép Check Toàn Bộ Chiều Dài Trả False Lâu Bằng Check Xong Đỡ Lòi Gợi Ý).</summary>
    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
            && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
