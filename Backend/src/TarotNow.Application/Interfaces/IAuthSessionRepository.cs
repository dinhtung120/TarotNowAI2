using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract quản lý phiên đăng nhập theo thiết bị.
/// </summary>
public interface IAuthSessionRepository
{
    /// <summary>
    /// Tạo session mới cho user/device.
    /// </summary>
    Task<AuthSession> CreateAsync(
        Guid userId,
        string deviceId,
        string userAgentHash,
        string ipHash,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy session active theo id.
    /// </summary>
    Task<AuthSession?> GetActiveAsync(Guid sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật metadata hoạt động mới nhất cho session đang active.
    /// </summary>
    Task TouchAsync(
        Guid sessionId,
        string ipHash,
        string userAgentHash,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu hồi một session.
    /// </summary>
    Task RevokeAsync(Guid sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu hồi toàn bộ session của user.
    /// </summary>
    Task RevokeAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách session id đang active của user.
    /// </summary>
    Task<IReadOnlyCollection<Guid>> GetActiveSessionIdsByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dọn các session đã revoke trước thời điểm cutoff (batch bounded).
    /// </summary>
    Task<int> CleanupRevokedBeforeAsync(
        DateTime cutoffUtc,
        int batchSize,
        CancellationToken cancellationToken = default);
}
