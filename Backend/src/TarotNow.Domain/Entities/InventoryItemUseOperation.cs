namespace TarotNow.Domain.Entities;

/// <summary>
/// Bản ghi idempotency cho thao tác dùng item inventory.
/// </summary>
public class InventoryItemUseOperation
{
    /// <summary>
    /// Định danh bản ghi idempotency.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Người dùng thực hiện thao tác.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Idempotency key từ request client.
    /// </summary>
    public string IdempotencyKey { get; private set; } = string.Empty;

    /// <summary>
    /// Mã item đã xử lý.
    /// </summary>
    public string ItemCode { get; private set; } = string.Empty;

    /// <summary>
    /// Card mục tiêu nếu có.
    /// </summary>
    public int? TargetCardId { get; private set; }

    /// <summary>
    /// Mốc xử lý theo UTC.
    /// </summary>
    public DateTime ProcessedAtUtc { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM.
    /// </summary>
    protected InventoryItemUseOperation()
    {
    }

    /// <summary>
    /// Khởi tạo bản ghi xử lý idempotency mới.
    /// </summary>
    public InventoryItemUseOperation(Guid userId, string idempotencyKey, string itemCode, int? targetCardId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new ArgumentException("IdempotencyKey is required.", nameof(idempotencyKey));
        }

        if (string.IsNullOrWhiteSpace(itemCode))
        {
            throw new ArgumentException("ItemCode is required.", nameof(itemCode));
        }

        Id = Guid.NewGuid();
        UserId = userId;
        IdempotencyKey = idempotencyKey.Trim();
        ItemCode = itemCode.Trim();
        TargetCardId = targetCardId;
        ProcessedAtUtc = DateTime.UtcNow;
    }
}
