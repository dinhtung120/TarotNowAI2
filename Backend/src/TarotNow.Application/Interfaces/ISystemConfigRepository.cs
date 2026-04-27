using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract thao tác cấu hình hệ thống key-value trên PostgreSQL.
/// </summary>
public interface ISystemConfigRepository
{
    /// <summary>
    /// Lấy toàn bộ cấu hình hiện có.
    /// </summary>
    Task<IReadOnlyList<SystemConfig>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy cấu hình theo key.
    /// </summary>
    Task<SystemConfig?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Upsert một key cấu hình.
    /// </summary>
    Task<SystemConfig> UpsertAsync(
        string key,
        string value,
        string valueKind,
        string? description,
        Guid? updatedBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa cấu hình theo key.
    /// </summary>
    Task DeleteByKeyAsync(string key, CancellationToken cancellationToken = default);
}
