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
    /// Thu hồi một session.
    /// </summary>
    Task RevokeAsync(Guid sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu hồi toàn bộ session của user.
    /// </summary>
    Task RevokeAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
