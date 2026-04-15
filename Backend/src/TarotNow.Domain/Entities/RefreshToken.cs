
using System.Security.Cryptography;
using System.Text;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Refresh token aggregate hỗ trợ rotation one-time-use và replay detection.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Định danh refresh token.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Chủ sở hữu token.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Định danh session thiết bị.
    /// </summary>
    public Guid SessionId { get; private set; }

    /// <summary>
    /// Định danh token family để revoke theo chuỗi rotation.
    /// </summary>
    public Guid FamilyId { get; private set; }

    /// <summary>
    /// Token cha trong chuỗi rotation.
    /// </summary>
    public Guid? ParentTokenId { get; private set; }

    /// <summary>
    /// Token đã thay thế token hiện tại.
    /// </summary>
    public Guid? ReplacedByTokenId { get; private set; }

    /// <summary>
    /// Giá trị token đã hash (hoặc plain legacy để tương thích dữ liệu cũ).
    /// </summary>
    public string Token { get; private set; } = string.Empty;

    /// <summary>
    /// Thời điểm token hết hạn.
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// Thời điểm token được tạo.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// IP tạo token để phục vụ audit.
    /// </summary>
    public string CreatedByIp { get; private set; } = string.Empty;

    /// <summary>
    /// DeviceId lúc tạo token.
    /// </summary>
    public string CreatedDeviceId { get; private set; } = string.Empty;

    /// <summary>
    /// User-agent hash lúc tạo token.
    /// </summary>
    public string CreatedUserAgentHash { get; private set; } = string.Empty;

    /// <summary>
    /// Thời điểm token được dùng để rotate.
    /// </summary>
    public DateTime? UsedAtUtc { get; private set; }

    /// <summary>
    /// Thời điểm token bị revoke.
    /// </summary>
    public DateTime? RevokedAt { get; private set; }

    /// <summary>
    /// Lý do revoke phục vụ security analytics.
    /// </summary>
    public string RevocationReason { get; private set; } = string.Empty;

    /// <summary>
    /// Idempotency key gần nhất đã rotate token này.
    /// </summary>
    public string LastRotateIdempotencyKey { get; private set; } = string.Empty;

    /// <summary>
    /// Navigation tới user.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Token đã quá hạn.
    /// </summary>
    public bool IsExpired => IsExpiredAt(DateTime.UtcNow);

    /// <summary>
    /// Token đã bị revoke.
    /// </summary>
    public bool IsRevoked => RevokedAt != null;

    /// <summary>
    /// Token còn hoạt động (chưa used/chưa revoked/chưa expired).
    /// </summary>
    public bool IsActive => UsedAtUtc is null && RevokedAt is null && !IsExpired;

    /// <summary>
    /// Kiểm tra token đã quá hạn tại mốc thời gian chỉ định.
    /// </summary>
    public bool IsExpiredAt(DateTime nowUtc) => nowUtc >= ExpiresAt;

    /// <summary>
    /// Constructor rỗng cho EF.
    /// </summary>
    protected RefreshToken()
    {
    }

    /// <summary>
    /// Khởi tạo refresh token mới.
    /// </summary>
    public RefreshToken(
        Guid userId,
        string token,
        DateTime expiresAt,
        string createdByIp,
        Guid sessionId = default,
        Guid familyId = default,
        Guid? parentTokenId = null,
        string? createdDeviceId = null,
        string? createdUserAgentHash = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        SessionId = sessionId;
        FamilyId = familyId == Guid.Empty ? Id : familyId;
        ParentTokenId = parentTokenId;
        Token = HashToken(token);
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        CreatedByIp = Normalize(createdByIp, 64, "unknown");
        CreatedDeviceId = Normalize(createdDeviceId, 128, "unknown");
        CreatedUserAgentHash = Normalize(createdUserAgentHash, 128, "unknown");
    }

    /// <summary>
    /// Cập nhật session id cho token legacy.
    /// </summary>
    public void BindSession(Guid sessionId)
    {
        if (sessionId == Guid.Empty || SessionId != Guid.Empty)
        {
            return;
        }

        SessionId = sessionId;
    }

    /// <summary>
    /// Thu hồi token với lý do cụ thể.
    /// </summary>
    public void Revoke(string reason)
    {
        var now = DateTime.UtcNow;
        RevokedAt = now;
        RevocationReason = Normalize(reason, 64, RefreshRevocationReasons.ManualRevoke);
        if (UsedAtUtc is null && string.Equals(reason, RefreshRevocationReasons.Rotated, StringComparison.Ordinal))
        {
            UsedAtUtc = now;
        }
    }

    /// <summary>
    /// Thu hồi token theo kiểu tương thích ngược.
    /// </summary>
    public void Revoke()
    {
        Revoke(RefreshRevocationReasons.ManualRevoke);
    }

    /// <summary>
    /// Đánh dấu token đã dùng thành công trong vòng rotate.
    /// </summary>
    public void MarkUsed(DateTime nowUtc, string idempotencyKey)
    {
        UsedAtUtc = nowUtc;
        RevokedAt = nowUtc;
        RevocationReason = RefreshRevocationReasons.Rotated;
        LastRotateIdempotencyKey = Normalize(idempotencyKey, 128, string.Empty);
    }

    /// <summary>
    /// Đánh dấu token bị compromise/replay.
    /// </summary>
    public void MarkCompromised(DateTime nowUtc)
    {
        if (RevokedAt is null)
        {
            RevokedAt = nowUtc;
        }

        RevocationReason = RefreshRevocationReasons.ReplayDetected;
    }

    /// <summary>
    /// Gắn token thay thế để truy vết chuỗi rotation.
    /// </summary>
    public void LinkReplacement(Guid replacementTokenId)
    {
        ReplacedByTokenId = replacementTokenId;
    }

    /// <summary>
    /// Kiểm tra token thô có khớp token đã lưu.
    /// </summary>
    public bool MatchesToken(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return false;
        }

        if (Token.Length < 64)
        {
            return string.Equals(Token, rawToken, StringComparison.Ordinal);
        }

        var hashedInput = HashToken(rawToken);
        return FixedTimeEquals(Token, hashedInput);
    }

    /// <summary>
    /// Băm token bằng SHA-256 trước khi lưu.
    /// </summary>
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

/// <summary>
/// Lý do revoke chuẩn hóa cho refresh token.
/// </summary>
public static class RefreshRevocationReasons
{
    /// <summary>
    /// Token bị rotate sau refresh hợp lệ.
    /// </summary>
    public const string Rotated = "ROTATED";

    /// <summary>
    /// Token bị replay/reuse bất thường.
    /// </summary>
    public const string ReplayDetected = "REPLAY_DETECTED";

    /// <summary>
    /// Token bị revoke thủ công (logout/admin revoke).
    /// </summary>
    public const string ManualRevoke = "MANUAL_REVOKE";
}
