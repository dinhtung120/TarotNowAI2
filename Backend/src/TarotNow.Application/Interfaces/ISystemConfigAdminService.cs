using TarotNow.Application.Common.SystemConfigs;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Service nghiệp vụ quản trị system configs (list/upsert + đồng bộ runtime).
/// </summary>
public interface ISystemConfigAdminService
{
    /// <summary>
    /// Lấy danh sách cấu hình cho màn hình admin.
    /// </summary>
    Task<IReadOnlyList<SystemConfigAdminItem>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Upsert một key cấu hình và đồng bộ snapshot/projection.
    /// </summary>
    Task<SystemConfigAdminItem> UpsertAsync(
        string key,
        string value,
        string valueKind,
        string? description,
        Guid updatedBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reload snapshot + projection runtime từ DB hiện tại.
    /// </summary>
    Task RefreshRuntimeAsync(CancellationToken cancellationToken = default);
}
