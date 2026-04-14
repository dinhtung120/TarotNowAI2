
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract quản lý refresh token để kiểm soát vòng đời session và rotation an toàn.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Lấy refresh token theo token thô.
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu refresh token mới.
    /// </summary>
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật trạng thái refresh token.
    /// </summary>
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rotate refresh token one-time-use với lock + idempotency.
    /// </summary>
    Task<RefreshRotateResult> RotateAsync(
        RefreshRotateRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu hồi toàn bộ token trong cùng family.
    /// </summary>
    Task RevokeFamilyAsync(Guid familyId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu hồi toàn bộ token trong cùng session.
    /// </summary>
    Task RevokeSessionAsync(Guid sessionId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu hồi toàn bộ token của user.
    /// </summary>
    Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Request rotate refresh token.
/// </summary>
public sealed class RefreshRotateRequest
{
    /// <summary>
    /// Refresh token hiện tại.
    /// </summary>
    public string RawToken { get; init; } = string.Empty;

    /// <summary>
    /// Refresh token mới (raw) dùng để thay thế.
    /// </summary>
    public string NewRawToken { get; init; } = string.Empty;

    /// <summary>
    /// Hạn dùng refresh token mới.
    /// </summary>
    public DateTime NewExpiresAtUtc { get; init; }

    /// <summary>
    /// Idempotency key của request refresh.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <summary>
    /// Ip của request refresh.
    /// </summary>
    public string IpAddress { get; init; } = string.Empty;

    /// <summary>
    /// Device id của request refresh.
    /// </summary>
    public string DeviceId { get; init; } = string.Empty;

    /// <summary>
    /// User-agent hash của request refresh.
    /// </summary>
    public string UserAgentHash { get; init; } = string.Empty;
}

/// <summary>
/// Kết quả rotate refresh token.
/// </summary>
public sealed class RefreshRotateResult
{
    /// <summary>
    /// Trạng thái rotate.
    /// </summary>
    public RefreshRotateStatus Status { get; init; }

    /// <summary>
    /// Token hiện tại đã xử lý.
    /// </summary>
    public RefreshToken? CurrentToken { get; init; }

    /// <summary>
    /// Token mới đã tạo (nếu thành công).
    /// </summary>
    public RefreshToken? NewToken { get; init; }

    /// <summary>
    /// Raw refresh token mới (dùng set cookie).
    /// </summary>
    public string NewRawToken { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm hết hạn của refresh token mới (hoặc token idempotent trả lại).
    /// </summary>
    public DateTime? NewTokenExpiresAtUtc { get; init; }

    /// <summary>
    /// Cờ idempotent hit.
    /// </summary>
    public bool IsIdempotent => Status == RefreshRotateStatus.Idempotent;

    /// <summary>
    /// Cờ rotation thành công.
    /// </summary>
    public bool IsSuccess => Status == RefreshRotateStatus.Success || Status == RefreshRotateStatus.Idempotent;

    /// <summary>
    /// Factory thành công.
    /// </summary>
    public static RefreshRotateResult Success(RefreshToken currentToken, RefreshToken newToken, string newRawToken)
        => new()
        {
            Status = RefreshRotateStatus.Success,
            CurrentToken = currentToken,
            NewToken = newToken,
            NewRawToken = newRawToken,
            NewTokenExpiresAtUtc = newToken.ExpiresAt
        };

    /// <summary>
    /// Factory idempotent replay hợp lệ.
    /// </summary>
    public static RefreshRotateResult Idempotent(RefreshToken currentToken, string newRawToken, DateTime newTokenExpiresAtUtc)
        => new()
        {
            Status = RefreshRotateStatus.Idempotent,
            CurrentToken = currentToken,
            NewRawToken = newRawToken,
            NewTokenExpiresAtUtc = newTokenExpiresAtUtc
        };

    /// <summary>
    /// Factory token không hợp lệ.
    /// </summary>
    public static RefreshRotateResult Invalid()
        => new()
        {
            Status = RefreshRotateStatus.InvalidToken
        };

    /// <summary>
    /// Factory token đã hết hạn.
    /// </summary>
    public static RefreshRotateResult Expired(RefreshToken currentToken)
        => new()
        {
            Status = RefreshRotateStatus.Expired,
            CurrentToken = currentToken
        };

    /// <summary>
    /// Factory replay detected.
    /// </summary>
    public static RefreshRotateResult ReplayDetected(RefreshToken currentToken)
        => new()
        {
            Status = RefreshRotateStatus.ReplayDetected,
            CurrentToken = currentToken
        };

    /// <summary>
    /// Factory lock contention.
    /// </summary>
    public static RefreshRotateResult Locked()
        => new()
        {
            Status = RefreshRotateStatus.Locked
        };
}

/// <summary>
/// Trạng thái rotate refresh token.
/// </summary>
public enum RefreshRotateStatus
{
    /// <summary>
    /// Rotate thành công.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Trả kết quả idempotent từ cùng request.
    /// </summary>
    Idempotent = 2,

    /// <summary>
    /// Token không hợp lệ.
    /// </summary>
    InvalidToken = 3,

    /// <summary>
    /// Token hết hạn.
    /// </summary>
    Expired = 4,

    /// <summary>
    /// Token bị replay bất thường.
    /// </summary>
    ReplayDetected = 5,

    /// <summary>
    /// Đang có refresh khác giữ lock.
    /// </summary>
    Locked = 6
}
