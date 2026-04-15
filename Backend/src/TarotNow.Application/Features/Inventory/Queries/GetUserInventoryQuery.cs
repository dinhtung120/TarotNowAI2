using MediatR;

namespace TarotNow.Application.Features.Inventory.Queries;

/// <summary>
/// Query lấy toàn bộ item người dùng đang sở hữu trong Tarot Vault.
/// </summary>
public sealed record GetUserInventoryQuery(Guid UserId) : IRequest<GetUserInventoryResult>;

/// <summary>
/// Kết quả truy vấn kho đồ người dùng.
/// </summary>
public sealed class GetUserInventoryResult
{
    /// <summary>
    /// Danh sách item user đang sở hữu.
    /// </summary>
    public IReadOnlyList<UserInventoryItemDto> Items { get; init; } = Array.Empty<UserInventoryItemDto>();
}

/// <summary>
/// DTO item trả về cho màn hình inventory.
/// </summary>
public sealed class UserInventoryItemDto
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
    /// Giá trị hiệu ứng item.
    /// </summary>
    public int EffectValue { get; init; }

    /// <summary>
    /// Tỉ lệ thành công của item.
    /// </summary>
    public decimal SuccessRatePercent { get; init; }

    /// <summary>
    /// Tên tiếng Việt.
    /// </summary>
    public string NameVi { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Anh.
    /// </summary>
    public string NameEn { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Trung.
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
    /// Quantity item user đang sở hữu.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Cờ cho biết item đang có thể sử dụng ngay.
    /// </summary>
    public bool CanUse { get; init; }

    /// <summary>
    /// Cờ cho biết item yêu cầu chọn card mục tiêu.
    /// </summary>
    public bool RequiresTargetCard { get; init; }

    /// <summary>
    /// Mã lý do không thể dùng (nếu có).
    /// </summary>
    public string? BlockedReason { get; init; }

    /// <summary>
    /// Mốc nhận item đầu tiên.
    /// </summary>
    public DateTime AcquiredAtUtc { get; init; }
}
