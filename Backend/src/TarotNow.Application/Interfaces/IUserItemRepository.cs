using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract truy cập item người dùng sở hữu và idempotency thao tác sử dụng item.
/// </summary>
public interface IUserItemRepository
{
    /// <summary>
    /// Lấy bản ghi user item theo user + item definition.
    /// </summary>
    Task<UserItem?> GetByUserAndItemDefinitionIdAsync(
        Guid userId,
        Guid itemDefinitionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ item inventory của user để hiển thị kho đồ.
    /// </summary>
    Task<IReadOnlyList<UserInventoryItemView>> GetUserInventoryAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật bản ghi user item.
    /// </summary>
    Task UpdateAsync(UserItem userItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tạo mới bản ghi user item.
    /// </summary>
    Task AddAsync(UserItem userItem, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thử consume item kèm đăng ký idempotency operation một cách an toàn cho concurrent requests.
    /// </summary>
    Task<InventoryItemConsumeResult> TryConsumeWithIdempotencyAsync(
        InventoryItemConsumeRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cấp thêm item theo item code cho người dùng.
    /// </summary>
    Task GrantItemByCodeAsync(
        Guid userId,
        string itemCode,
        int quantity,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Request consume item với thông tin cần cho kiểm tra idempotency.
/// </summary>
public sealed class InventoryItemConsumeRequest
{
    /// <summary>
    /// Định danh user thực hiện consume.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Định danh item definition cần consume.
    /// </summary>
    public Guid ItemDefinitionId { get; init; }

    /// <summary>
    /// Mã item dùng để lưu operation log.
    /// </summary>
    public string ItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Card mục tiêu nếu có.
    /// </summary>
    public int? TargetCardId { get; init; }

    /// <summary>
    /// Idempotency key của thao tác.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <summary>
    /// Cờ cho biết item cần consume quantity.
    /// </summary>
    public bool IsConsumable { get; init; }

    /// <summary>
    /// Số lượng consume mỗi lần.
    /// </summary>
    public int ConsumeQuantity { get; init; }
}

/// <summary>
/// Kết quả consume item theo trạng thái nghiệp vụ.
/// </summary>
public enum InventoryItemConsumeResult
{
    /// <summary>
    /// Consume thành công.
    /// </summary>
    Consumed = 0,

    /// <summary>
    /// Idempotency key đã xử lý trước đó.
    /// </summary>
    AlreadyProcessed = 1,

    /// <summary>
    /// User không sở hữu item.
    /// </summary>
    ItemNotOwned = 2,

    /// <summary>
    /// Item sở hữu nhưng không đủ quantity để consume.
    /// </summary>
    OutOfStock = 3,
}

/// <summary>
/// Dòng dữ liệu inventory dùng cho query hiển thị UI.
/// </summary>
public sealed class UserInventoryItemView
{
    /// <summary>
    /// Định danh item definition.
    /// </summary>
    public Guid ItemDefinitionId { get; init; }

    /// <summary>
    /// Mã item.
    /// </summary>
    public string ItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Loại item.
    /// </summary>
    public string ItemType { get; init; } = string.Empty;

    /// <summary>
    /// Kiểu enhancement nếu có.
    /// </summary>
    public string? EnhancementType { get; init; }

    /// <summary>
    /// Độ hiếm item.
    /// </summary>
    public string Rarity { get; init; } = string.Empty;

    /// <summary>
    /// Item có tiêu hao quantity hay không.
    /// </summary>
    public bool IsConsumable { get; init; }

    /// <summary>
    /// Item sở hữu vĩnh viễn hay không.
    /// </summary>
    public bool IsPermanent { get; init; }

    /// <summary>
    /// Giá trị hiệu ứng cơ bản.
    /// </summary>
    public int EffectValue { get; init; }

    /// <summary>
    /// Tỉ lệ thành công.
    /// </summary>
    public decimal SuccessRatePercent { get; init; }

    /// <summary>
    /// Tên hiển thị tiếng Việt.
    /// </summary>
    public string NameVi { get; init; } = string.Empty;

    /// <summary>
    /// Tên hiển thị tiếng Anh.
    /// </summary>
    public string NameEn { get; init; } = string.Empty;

    /// <summary>
    /// Tên hiển thị tiếng Trung.
    /// </summary>
    public string NameZh { get; init; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Việt.
    /// </summary>
    public string DescriptionVi { get; init; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Anh.
    /// </summary>
    public string DescriptionEn { get; init; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Trung.
    /// </summary>
    public string DescriptionZh { get; init; } = string.Empty;

    /// <summary>
    /// URL icon item.
    /// </summary>
    public string? IconUrl { get; init; }

    /// <summary>
    /// Quantity user đang sở hữu.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Mốc nhận item lần đầu.
    /// </summary>
    public DateTime AcquiredAtUtc { get; init; }
}
