using System.Security.Cryptography;
using System.Text;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho phiên đăng nhập linh hoạt của User.
/// Refresh Token chỉ lưu ở HttpOnly Cookie phía Client, giúp reissue Access Token (Phase 1.1).
/// </summary>
public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedByIp { get; private set; } = string.Empty;

    /// <summary>
    /// Thời điểm bị vô hiệu hóa (Revoked). Có thể do User đăng xuất 
    /// hoặc bị hệ thống khóa do phát hiện Fraud (Reuse detection).
    /// </summary>
    public DateTime? RevokedAt { get; private set; }

    // Navigation property
    public User User { get; private set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    protected RefreshToken() { } // Dành cho EF Core

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
    /// Thu hồi token.
    /// </summary>
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }

    public bool MatchesToken(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken)) return false;

        // Backward compatibility: token cũ lưu plaintext trước khi áp dụng hashing.
        if (Token.Length < 64)
            return string.Equals(Token, rawToken, StringComparison.Ordinal);

        var hashedInput = HashToken(rawToken);
        return FixedTimeEquals(Token, hashedInput);
    }

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
