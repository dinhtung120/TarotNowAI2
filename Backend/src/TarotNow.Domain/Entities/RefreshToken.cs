/*
 * ===================================================================
 * FILE: RefreshToken.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Giữ Vạt Áo Phiên Đăng Nhập Dài Gần Nửa Tháng (Refresh Token).
 *   Không Nên Hiện Text Mạng Vì Chống Attacker Đụng Được Nếu Có Nhảy Mất DB Vẫn Hoàn Toàn Gỉa Được Khách.
 * ===================================================================
 */

using System.Security.Cryptography;
using System.Text;
using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity SQL `refresh_tokens`. Lõi Gốc Chứa Cán Cân Chìa Bất Khả Sinh Khóa Ngắn Ảo Hóa Cho Người Đăng Nhập Client Thường Kèm HttpOnly Cookie Tự Lưu Tự Kí Sống 10 Ngày Rảnh Đỡ Bắt Khách Đo Login Ở App Nhiều Lần Sinh Phiền.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; private set; }
    // Khách Hàng Là Thằng Nào Dùng Sinh Trạm Trụy Lệnh Cookie Đang Sống Này.
    public Guid UserId { get; private set; }
    
    // Cấm Lộ Nguyên Lá Chữ: Nếu Ai Hacker Phá Lủng Vào DB Vào Lấy Nguyên Nắm Refresh Xong Ra Tạo Access Token Cho Tất Cả -> Quá Rủi. (Nên Phải Hash Giống Password Rồi Giữ Bản Hash Tượng Tự Dưới Cột Này Cho An Toàn).
    public string Token { get; private set; } = string.Empty;
    
    // Đếm Bom Giải Ván Refresh (Vd 7 Ngày Cắt Là Bắt Phải Nhập Hẳn Lại Form Email Password Rồi Đăng Nhập Rõ Lại Do Expired Lâu Òi).
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    // Chắn Khớp Kẻ Ngụy Trang (Đòi Máy Trạm Ban Cùng IP Hay Gọi Nếu IP Nhảy Liên Quốc Gia Lòng Vòng Trong Tiếng Đuổi Cổ Kẻ Thay Trộm Máy Hắn).
    public string CreatedByIp { get; private set; } = string.Empty;

    /// <summary>
    /// Vành Chân Treo Lơ Lửng (Revoked): Vô Chỗ Chĩa Ngang Bị Chặt Chém Dù Chưa Hết Hạn Vì Lý Do Bảo Mật (Đổi Pass Ngang Hoặc Báo Hack Ấn Khóa Token Này Trên Panel Web Hoặc Tình Nghi Dùng Reuse Detection Token Bị Nhồi 2 Lần Giữa Cả Cũ Và Mới Tố Nhau).
    /// </summary>
    public DateTime? RevokedAt { get; private set; }

    // Rãnh Sinh Kết Thằng Cáo (User).
    public User User { get; private set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    protected RefreshToken() { } // Dành cho EF Core Bóp Sống Code Nho

    /// <summary>
    /// Ép Tạo Lệnh Ra Đều Băm Hash Lại Trắng Sạch Lặn Im Vô Code.
    /// </summary>
    public RefreshToken(Guid userId, string token, DateTime expiresAt, string createdByIp)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = HashToken(token);
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        CreatedByIp = createdByIp;
    }

    /// <summary>
    /// Khai Tử Mã Kẹt Giết Vĩnh Viễn Không Cho Mở Ra Ánh Nắng (Bị Tước Tích Ấn Ngay Tức Khắc).
    /// </summary>
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }

    /// <summary>Tay So Dò Hash: Chặn Timming Attack - Thằng Hacker Lò Dò Nhanh.</summary>
    public bool MatchesToken(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken)) return false;

        // Cho Mấy Bác Khách Ở Hồi Xưa Rớt Vô System Chưa Hashing Chở Khứ So Mộc Bình Dân Khớp Nhóm.
        if (Token.Length < 64)
            return string.Equals(Token, rawToken, StringComparison.Ordinal);

        var hashedInput = HashToken(rawToken);
        return FixedTimeEquals(Token, hashedInput);
    }

    /// <summary>Đạn Xoáy Xẻ Code 64 Chữ Cho DB Nằm Không Lái Tù.</summary>
    public static string HashToken(string token)
    {
        var normalized = token.Trim();
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
