namespace TarotNow.Domain.Entities;

/// <summary>
/// Trạng thái hiệu ứng may mắn đang hoạt động từ inventory item.
/// </summary>
public sealed class InventoryLuckEffect
{
    /// <summary>
    /// Định danh bản ghi hiệu ứng.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Định danh người dùng nhận hiệu ứng.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Tổng điểm may mắn hiện tại.
    /// </summary>
    public int LuckValue { get; private set; }

    /// <summary>
    /// Mốc hết hạn hiệu ứng theo UTC.
    /// </summary>
    public DateTime ExpiresAtUtc { get; private set; }

    /// <summary>
    /// Mã item nguồn áp dụng lần cuối.
    /// </summary>
    public string SourceItemCode { get; private set; } = string.Empty;

    /// <summary>
    /// Mốc tạo bản ghi theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Mốc cập nhật gần nhất theo UTC.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// </summary>
    private InventoryLuckEffect()
    {
    }

    /// <summary>
    /// Khởi tạo hiệu ứng may mắn mới.
    /// </summary>
    public InventoryLuckEffect(Guid userId, int luckValue, string sourceItemCode, TimeSpan duration)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
        Apply(luckValue, sourceItemCode, duration);
    }

    /// <summary>
    /// Áp thêm hiệu ứng may mắn cho user.
    /// </summary>
    public void Apply(int luckValue, string sourceItemCode, TimeSpan duration)
    {
        if (luckValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(luckValue), "Luck value must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(sourceItemCode))
        {
            throw new ArgumentException("Source item code is required.", nameof(sourceItemCode));
        }

        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be positive.");
        }

        var nowUtc = DateTime.UtcNow;
        var currentLuck = IsActive(nowUtc) ? LuckValue : 0;

        LuckValue = currentLuck + luckValue;
        SourceItemCode = sourceItemCode.Trim().ToLowerInvariant();
        ExpiresAtUtc = nowUtc.Add(duration);
        UpdatedAtUtc = nowUtc;
    }

    /// <summary>
    /// Kiểm tra hiệu ứng có còn hiệu lực tại thời điểm truyền vào.
    /// </summary>
    public bool IsActive(DateTime nowUtc)
    {
        return ExpiresAtUtc > nowUtc;
    }
}
