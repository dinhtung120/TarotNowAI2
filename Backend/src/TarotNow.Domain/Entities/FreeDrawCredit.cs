namespace TarotNow.Domain.Entities;

/// <summary>
/// Số dư lượt xem bài miễn phí phát sinh từ inventory.
/// </summary>
public sealed class FreeDrawCredit
{
    /// <summary>
    /// Định danh bản ghi credit.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Định danh người dùng sở hữu credit.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Số lượt free draw còn khả dụng.
    /// </summary>
    public int AvailableCount { get; private set; }

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
    private FreeDrawCredit()
    {
    }

    /// <summary>
    /// Khởi tạo credit mới cho user.
    /// </summary>
    public FreeDrawCredit(Guid userId, int initialCount)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (initialCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialCount), "Initial credit must be greater than zero.");
        }

        Id = Guid.NewGuid();
        UserId = userId;
        AvailableCount = initialCount;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    /// <summary>
    /// Cộng thêm credit free draw.
    /// </summary>
    public void AddCredits(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
        }

        AvailableCount += count;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
