namespace TarotNow.Domain.Entities;

/// <summary>
/// Bản ghi sở hữu item của người dùng trong Tarot Vault.
/// </summary>
public class UserItem
{
    /// <summary>
    /// Định danh bản ghi sở hữu item.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Định danh người dùng sở hữu item.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Định danh item definition.
    /// </summary>
    public Guid ItemDefinitionId { get; private set; }

    /// <summary>
    /// Navigation đến định nghĩa item.
    /// </summary>
    public ItemDefinition? ItemDefinition { get; private set; }

    /// <summary>
    /// Số lượng item đang sở hữu.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Thời điểm nhận item lần đầu.
    /// </summary>
    public DateTime AcquiredAtUtc { get; private set; }

    /// <summary>
    /// Thời điểm cập nhật quantity gần nhất.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM.
    /// </summary>
    protected UserItem()
    {
    }

    /// <summary>
    /// Khởi tạo bản ghi sở hữu item mới.
    /// </summary>
    public UserItem(Guid userId, Guid itemDefinitionId, int quantity)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (itemDefinitionId == Guid.Empty)
        {
            throw new ArgumentException("ItemDefinitionId is required.", nameof(itemDefinitionId));
        }

        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be >= 0.");
        }

        Id = Guid.NewGuid();
        UserId = userId;
        ItemDefinitionId = itemDefinitionId;
        Quantity = quantity;
        AcquiredAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = AcquiredAtUtc;
    }

    /// <summary>
    /// Cộng thêm số lượng item.
    /// </summary>
    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be > 0.");
        }

        Quantity += amount;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Trừ số lượng item theo lượng yêu cầu.
    /// </summary>
    public void DecreaseQuantity(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be > 0.");
        }

        if (Quantity < amount)
        {
            throw new InvalidOperationException("Insufficient quantity.");
        }

        Quantity -= amount;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
