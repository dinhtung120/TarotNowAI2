using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract truy cập dữ liệu pool gacha và operation pull.
/// </summary>
public interface IGachaPoolRepository
{
    /// <summary>
    /// Lấy danh sách pool active.
    /// </summary>
    Task<IReadOnlyList<GachaPool>> GetActivePoolsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy pool active theo code.
    /// </summary>
    Task<GachaPool?> GetActivePoolByCodeAsync(string poolCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy reward rates active theo pool.
    /// </summary>
    Task<IReadOnlyList<GachaPoolRewardRate>> GetActiveRewardRatesAsync(Guid poolId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy pity hiện tại của user theo pool.
    /// </summary>
    Task<int> GetUserCurrentPityCountAsync(Guid userId, Guid poolId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy hoặc tạo bản ghi pity của user theo pool để xử lý write-side.
    /// </summary>
    Task<UserGachaPity> GetOrCreateUserPityAsync(Guid userId, Guid poolId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật bản ghi pity.
    /// </summary>
    Task SaveUserPityAsync(UserGachaPity userGachaPity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thử tạo pull operation mới theo idempotency key.
    /// </summary>
    Task<GachaPullOperationCreateResult> TryCreatePullOperationAsync(
        GachaPullOperation operation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật trạng thái hoàn tất cho operation.
    /// </summary>
    Task MarkPullOperationCompletedAsync(
        GachaPullOperation operation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu batch reward logs của một operation.
    /// </summary>
    Task AddPullRewardLogsAsync(
        IReadOnlyCollection<GachaPullRewardLog> rewardLogs,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu lịch sử pull-level.
    /// </summary>
    Task AddHistoryEntryAsync(GachaHistoryEntry historyEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy reward logs theo operation.
    /// </summary>
    Task<IReadOnlyList<GachaPullRewardLog>> GetRewardLogsByOperationIdAsync(
        Guid pullOperationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy reward logs theo tập operation id.
    /// </summary>
    Task<IReadOnlyList<GachaPullRewardLog>> GetRewardLogsByOperationIdsAsync(
        IReadOnlyCollection<Guid> pullOperationIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lịch sử reward line-level của user.
    /// </summary>
    Task<IReadOnlyList<GachaPullRewardLog>> GetUserRewardHistoryAsync(
        Guid userId,
        int limit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy lịch sử pull-level của user theo phân trang.
    /// </summary>
    Task<GachaHistoryPageReadModel> GetUserPullHistoryAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Kết quả tạo pull operation.
/// </summary>
public sealed class GachaPullOperationCreateResult
{
    /// <summary>
    /// Cờ cho biết operation vừa được tạo mới.
    /// </summary>
    public bool IsCreated { get; init; }

    /// <summary>
    /// Operation hiện tại (mới tạo hoặc đã tồn tại).
    /// </summary>
    public GachaPullOperation Operation { get; init; } = null!;
}

/// <summary>
/// Kết quả đọc lịch sử pull-level theo phân trang.
/// </summary>
public sealed class GachaHistoryPageReadModel
{
    /// <summary>
    /// Trang hiện tại.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Kích thước trang.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Tổng số bản ghi.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Danh sách lịch sử pull-level.
    /// </summary>
    public IReadOnlyList<GachaHistoryEntry> Items { get; init; } = Array.Empty<GachaHistoryEntry>();
}
