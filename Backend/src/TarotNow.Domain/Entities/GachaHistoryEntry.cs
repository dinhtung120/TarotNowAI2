namespace TarotNow.Domain.Entities;

/// <summary>
/// Bản ghi lịch sử pull-level của gacha.
/// </summary>
public sealed class GachaHistoryEntry
{
    /// <summary>
    /// Định danh lịch sử.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Định danh pull operation tương ứng.
    /// </summary>
    public Guid PullOperationId { get; private set; }

    /// <summary>
    /// Định danh user thực hiện pull.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Định danh pool.
    /// </summary>
    public Guid PoolId { get; private set; }

    /// <summary>
    /// Mã pool.
    /// </summary>
    public string PoolCode { get; private set; } = string.Empty;

    /// <summary>
    /// Số lượt pull trong operation.
    /// </summary>
    public int PullCount { get; private set; }

    /// <summary>
    /// Pity trước khi chạy operation.
    /// </summary>
    public int PityBefore { get; private set; }

    /// <summary>
    /// Pity sau khi hoàn tất operation.
    /// </summary>
    public int PityAfter { get; private set; }

    /// <summary>
    /// Có reset pity trong operation hay không.
    /// </summary>
    public bool WasPityReset { get; private set; }

    /// <summary>
    /// Mốc tạo bản ghi UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Constructor cho ORM.
    /// </summary>
    private GachaHistoryEntry()
    {
    }

    /// <summary>
    /// Khởi tạo bản ghi lịch sử pull.
    /// </summary>
    public GachaHistoryEntry(
        Guid pullOperationId,
        Guid userId,
        Guid poolId,
        string poolCode,
        int pullCount,
        int pityBefore,
        int pityAfter,
        bool wasPityReset)
    {
        if (pullOperationId == Guid.Empty)
        {
            throw new ArgumentException("PullOperationId is required.", nameof(pullOperationId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (poolId == Guid.Empty)
        {
            throw new ArgumentException("PoolId is required.", nameof(poolId));
        }

        if (string.IsNullOrWhiteSpace(poolCode))
        {
            throw new ArgumentException("PoolCode is required.", nameof(poolCode));
        }

        if (pullCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pullCount), "PullCount must be > 0.");
        }

        if (pityBefore < 0 || pityAfter < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pityBefore), "Pity values must be >= 0.");
        }

        Id = Guid.NewGuid();
        PullOperationId = pullOperationId;
        UserId = userId;
        PoolId = poolId;
        PoolCode = poolCode.Trim().ToLowerInvariant();
        PullCount = pullCount;
        PityBefore = pityBefore;
        PityAfter = pityAfter;
        WasPityReset = wasPityReset;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
