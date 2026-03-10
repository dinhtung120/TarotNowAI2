namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho phiên đăng nhập linh hoạt của User.
/// Refresh Token chỉ lưu ở HttpOnly Cookie phía Client, giúp reissue Access Token (Phase 1.1).
/// </summary>
public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedByIp { get; private set; }

    /// <summary>
    /// Thời điểm bị vô hiệu hóa (Revoked). Có thể do User đăng xuất 
    /// hoặc bị hệ thống khóa do phát hiện Fraud (Reuse detection).
    /// </summary>
    public DateTime? RevokedAt { get; private set; }

    // Navigation property
    public User User { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    protected RefreshToken() { } // Dành cho EF Core

    public RefreshToken(Guid userId, string token, DateTime expiresAt, string createdByIp)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
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
}
