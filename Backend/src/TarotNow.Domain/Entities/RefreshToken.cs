
using System.Security.Cryptography;
using System.Text;
using System;

namespace TarotNow.Domain.Entities;

// Entity refresh token để quản lý vòng đời phiên đăng nhập dài hạn một cách an toàn.
public class RefreshToken
{
    // Định danh refresh token.
    public Guid Id { get; private set; }

    // Người dùng sở hữu token.
    public Guid UserId { get; private set; }

    // Giá trị token đã hash (hoặc plain legacy để tương thích).
    public string Token { get; private set; } = string.Empty;

    // Thời điểm token hết hạn.
    public DateTime ExpiresAt { get; private set; }

    // Thời điểm token được tạo.
    public DateTime CreatedAt { get; private set; }

    // IP tạo token để phục vụ audit bảo mật.
    public string CreatedByIp { get; private set; } = string.Empty;

    // Thời điểm token bị thu hồi.
    public DateTime? RevokedAt { get; private set; }

    // Navigation tới user.
    public User User { get; private set; } = null!;

    // Token đã quá hạn hay chưa.
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    // Token đã bị thu hồi hay chưa.
    public bool IsRevoked => RevokedAt != null;

    // Token còn hoạt động khi chưa revoke và chưa hết hạn.
    public bool IsActive => !IsRevoked && !IsExpired;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khôi phục entity từ cơ sở dữ liệu.
    /// </summary>
    protected RefreshToken() { }

    /// <summary>
    /// Khởi tạo refresh token mới cho một phiên đăng nhập.
    /// Luồng xử lý: sinh id, hash token thô, gán hạn dùng và metadata nguồn tạo.
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
    /// Thu hồi token để ngăn gia hạn phiên từ token này.
    /// Luồng xử lý: ghi mốc RevokedAt hiện tại.
    /// </summary>
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
        // Đổi trạng thái token sang revoked để mọi kiểm tra IsActive trả false.
    }

    /// <summary>
    /// Kiểm tra token thô có khớp token đã lưu hay không.
    /// Luồng xử lý: chặn input rỗng, hỗ trợ dữ liệu legacy plain text, còn lại hash và so sánh fixed-time.
    /// </summary>
    public bool MatchesToken(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            // Edge case: token đầu vào rỗng phải bị từ chối ngay để tránh bypass kiểm tra.
            return false;
        }

        if (Token.Length < 64)
        {
            // Nhánh tương thích ngược cho bản ghi cũ chưa hash đầy đủ.
            return string.Equals(Token, rawToken, StringComparison.Ordinal);
        }

        var hashedInput = HashToken(rawToken);
        // So sánh constant-time để giảm nguy cơ timing attack.
        return FixedTimeEquals(Token, hashedInput);
    }

    /// <summary>
    /// Băm token bằng SHA-256 trước khi lưu để tránh lộ token ở dạng thô.
    /// Luồng xử lý: trim token, hash bytes UTF-8 và trả chuỗi hex lowercase.
    /// </summary>
    public static string HashToken(string token)
    {
        var normalized = token.Trim();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>
    /// So sánh hai chuỗi ở thời gian gần cố định để hạn chế rò rỉ timing.
    /// Luồng xử lý: chuyển sang bytes, kiểm tra độ dài và gọi FixedTimeEquals của crypto API.
    /// </summary>
    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length
               && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
