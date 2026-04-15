namespace TarotNow.Domain.Entities;

/// <summary>
/// Trạng thái pity theo từng user và pool gacha.
/// </summary>
public sealed class UserGachaPity
{
    /// <summary>
    /// Định danh bản ghi pity.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Định danh người dùng.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Định danh pool.
    /// </summary>
    public Guid PoolId { get; private set; }

    /// <summary>
    /// Số lượt hiện tại kể từ lần reset gần nhất.
    /// </summary>
    public int PullCount { get; private set; }

    /// <summary>
    /// Mốc reset pity gần nhất.
    /// </summary>
    public DateTime? LastPityResetAtUtc { get; private set; }

    /// <summary>
    /// Mốc tạo bản ghi.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Mốc cập nhật gần nhất.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Constructor cho ORM.
    /// </summary>
    private UserGachaPity()
    {
    }

    /// <summary>
    /// Khởi tạo trạng thái pity mới.
    /// </summary>
    public UserGachaPity(Guid userId, Guid poolId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (poolId == Guid.Empty)
        {
            throw new ArgumentException("PoolId is required.", nameof(poolId));
        }

        Id = Guid.NewGuid();
        UserId = userId;
        PoolId = poolId;
        PullCount = 0;
        LastPityResetAtUtc = null;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    /// <summary>
    /// Tăng pity counter theo một lượt pull không đạt mốc rare.
    /// </summary>
    public void Increment()
    {
        PullCount = checked(PullCount + 1);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Reset pity counter khi đạt reward rare theo quy tắc pool.
    /// </summary>
    public void Reset()
    {
        PullCount = 0;
        LastPityResetAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = LastPityResetAtUtc.Value;
    }
}
