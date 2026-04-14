namespace TarotNow.Domain.Entities;

/// <summary>
/// Phiên đăng nhập theo thiết bị để hỗ trợ multi-device và revoke theo session.
/// </summary>
public sealed class AuthSession
{
    /// <summary>
    /// Định danh session.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Chủ sở hữu session.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Device id do client cung cấp.
    /// </summary>
    public string DeviceId { get; private set; } = string.Empty;

    /// <summary>
    /// Hash user-agent để theo dõi thay đổi bất thường.
    /// </summary>
    public string UserAgentHash { get; private set; } = string.Empty;

    /// <summary>
    /// Hash IP lần hoạt động gần nhất.
    /// </summary>
    public string LastIpHash { get; private set; } = string.Empty;

    /// <summary>
    /// Thời điểm tạo session.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Thời điểm session được nhìn thấy gần nhất.
    /// </summary>
    public DateTime LastSeenAtUtc { get; private set; }

    /// <summary>
    /// Thời điểm revoke session (null nếu còn active).
    /// </summary>
    public DateTime? RevokedAtUtc { get; private set; }

    /// <summary>
    /// Navigation tới user.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Session còn hoạt động.
    /// </summary>
    public bool IsActive => RevokedAtUtc is null;

    /// <summary>
    /// Constructor rỗng cho EF.
    /// </summary>
    private AuthSession()
    {
    }

    /// <summary>
    /// Tạo session mới.
    /// </summary>
    public AuthSession(Guid userId, string deviceId, string userAgentHash, string lastIpHash)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        DeviceId = Normalize(deviceId, 128, "unknown");
        UserAgentHash = Normalize(userAgentHash, 128, "unknown");
        LastIpHash = Normalize(lastIpHash, 128, "unknown");
        CreatedAtUtc = DateTime.UtcNow;
        LastSeenAtUtc = CreatedAtUtc;
    }

    /// <summary>
    /// Cập nhật hoạt động session.
    /// </summary>
    public void Touch(string ipHash, DateTime nowUtc)
    {
        LastIpHash = Normalize(ipHash, 128, "unknown");
        LastSeenAtUtc = nowUtc;
    }

    /// <summary>
    /// Revoke session.
    /// </summary>
    public void Revoke(DateTime nowUtc)
    {
        RevokedAtUtc = nowUtc;
    }

    private static string Normalize(string? value, int maxLength, string fallback)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return fallback;
        }

        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }
}
