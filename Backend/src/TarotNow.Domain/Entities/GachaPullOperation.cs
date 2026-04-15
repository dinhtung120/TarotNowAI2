namespace TarotNow.Domain.Entities;

/// <summary>
/// Bản ghi idempotency cho một yêu cầu pull gacha.
/// </summary>
public sealed class GachaPullOperation
{
    /// <summary>
    /// Định danh operation.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Người dùng thực hiện pull.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Pool được pull.
    /// </summary>
    public Guid PoolId { get; private set; }

    /// <summary>
    /// Khóa idempotency.
    /// </summary>
    public string IdempotencyKey { get; private set; } = string.Empty;

    /// <summary>
    /// Số lượt pull trong request.
    /// </summary>
    public int PullCount { get; private set; }

    /// <summary>
    /// Pity hiện tại sau khi hoàn tất request.
    /// </summary>
    public int CurrentPityCount { get; private set; }

    /// <summary>
    /// Ngưỡng hard pity của pool.
    /// </summary>
    public int HardPityThreshold { get; private set; }

    /// <summary>
    /// Có trigger pity trong request hay không.
    /// </summary>
    public bool WasPityTriggered { get; private set; }

    /// <summary>
    /// Đã hoàn tất xử lý hay chưa.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Mốc tạo operation theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Mốc hoàn tất operation theo UTC.
    /// </summary>
    public DateTime? CompletedAtUtc { get; private set; }

    /// <summary>
    /// Constructor cho ORM.
    /// </summary>
    private GachaPullOperation()
    {
    }

    /// <summary>
    /// Khởi tạo operation pull mới.
    /// </summary>
    public GachaPullOperation(Guid userId, Guid poolId, string idempotencyKey, int pullCount)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (poolId == Guid.Empty)
        {
            throw new ArgumentException("PoolId is required.", nameof(poolId));
        }

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new ArgumentException("Idempotency key is required.", nameof(idempotencyKey));
        }

        if (pullCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pullCount), "Pull count must be greater than zero.");
        }

        Id = Guid.NewGuid();
        UserId = userId;
        PoolId = poolId;
        IdempotencyKey = idempotencyKey.Trim();
        PullCount = pullCount;
        CurrentPityCount = 0;
        HardPityThreshold = 0;
        WasPityTriggered = false;
        IsCompleted = false;
        CreatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu operation hoàn tất.
    /// </summary>
    public void MarkCompleted(int currentPityCount, int hardPityThreshold, bool wasPityTriggered)
    {
        if (currentPityCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentPityCount), "Current pity must be >= 0.");
        }

        if (hardPityThreshold < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hardPityThreshold), "Hard pity threshold must be >= 0.");
        }

        CurrentPityCount = currentPityCount;
        HardPityThreshold = hardPityThreshold;
        WasPityTriggered = wasPityTriggered;
        IsCompleted = true;
        CompletedAtUtc = DateTime.UtcNow;
    }
}
