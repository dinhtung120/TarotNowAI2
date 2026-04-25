namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Tập mã lỗi nghiệp vụ cho module inventory.
/// </summary>
public static class InventoryErrorCodes
{
    /// <summary>
    /// Item definition không tồn tại hoặc đã bị vô hiệu.
    /// </summary>
    public const string ItemNotFound = "INVENTORY_ITEM_NOT_FOUND";

    /// <summary>
    /// Người dùng không sở hữu item.
    /// </summary>
    public const string ItemNotOwned = "INVENTORY_ITEM_NOT_OWNED";

    /// <summary>
    /// Item tiêu hao nhưng quantity không đủ.
    /// </summary>
    public const string ItemOutOfStock = "INVENTORY_ITEM_OUT_OF_STOCK";

    /// <summary>
    /// Item yêu cầu card mục tiêu nhưng request thiếu dữ liệu.
    /// </summary>
    public const string TargetCardRequired = "INVENTORY_TARGET_CARD_REQUIRED";

    /// <summary>
    /// Card mục tiêu không tồn tại trong bộ sưu tập của user.
    /// </summary>
    public const string TargetCardNotOwned = "INVENTORY_TARGET_CARD_NOT_OWNED";

    /// <summary>
    /// Kiểu item không được hỗ trợ.
    /// </summary>
    public const string UnsupportedItemType = "INVENTORY_UNSUPPORTED_ITEM_TYPE";

    /// <summary>
    /// Idempotency key đã được xử lý trước đó.
    /// </summary>
    public const string AlreadyProcessed = "INVENTORY_ALREADY_PROCESSED";

    /// <summary>
    /// Số lượng sử dụng item không hợp lệ với loại item hiện tại.
    /// </summary>
    public const string InvalidQuantity = "INVENTORY_INVALID_QUANTITY";
}
