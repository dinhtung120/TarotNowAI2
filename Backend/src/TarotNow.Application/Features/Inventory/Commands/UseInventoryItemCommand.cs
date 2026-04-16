using MediatR;

namespace TarotNow.Application.Features.Inventory.Commands;

/// <summary>
/// Command yêu cầu sử dụng một item trong kho đồ tarot.
/// </summary>
public sealed record UseInventoryItemCommand : IRequest<UseInventoryItemResult>
{
    /// <summary>
    /// Người dùng thực hiện thao tác.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã item cần sử dụng.
    /// </summary>
    public string ItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Lá bài mục tiêu nếu item là card enhancer.
    /// </summary>
    public int? TargetCardId { get; init; }

    /// <summary>
    /// Idempotency key chống xử lý lặp.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;
}

/// <summary>
/// Kết quả trả về sau khi command dùng item được tiếp nhận.
/// </summary>
public sealed class UseInventoryItemResult
{
    /// <summary>
    /// Mã item đã xử lý.
    /// </summary>
    public string ItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Lá bài mục tiêu của thao tác, nếu có.
    /// </summary>
    public int? TargetCardId { get; init; }

    /// <summary>
    /// Cờ cho biết request là replay theo idempotency key.
    /// </summary>
    public bool IsIdempotentReplay { get; init; }

    /// <summary>
    /// Thông điệp kết quả ở mức command.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Tóm tắt hiệu ứng áp dụng nếu có.
    /// </summary>
    public UseInventoryItemEffectSummary? EffectSummary { get; init; }
}

/// <summary>
/// Tóm tắt hiệu ứng item trả về API.
/// </summary>
public sealed class UseInventoryItemEffectSummary
{
    /// <summary>
    /// Loại hiệu ứng đã áp dụng.
    /// </summary>
    public string EffectType { get; init; } = string.Empty;

    /// <summary>
    /// Giá trị roll thực tế (% hoặc EXP hoặc số vé).
    /// </summary>
    public decimal RolledValue { get; init; }

    /// <summary>
    /// Card mục tiêu nếu có.
    /// </summary>
    public int? CardId { get; init; }

    /// <summary>
    /// Snapshot trước khi áp dụng.
    /// </summary>
    public UseInventoryCardStatSnapshot? Before { get; init; }

    /// <summary>
    /// Snapshot sau khi áp dụng.
    /// </summary>
    public UseInventoryCardStatSnapshot? After { get; init; }
}

/// <summary>
/// Snapshot chỉ số card trước/sau.
/// </summary>
public sealed class UseInventoryCardStatSnapshot
{
    /// <summary>
    /// Level card.
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// EXP hiện tại.
    /// </summary>
    public decimal CurrentExp { get; init; }

    /// <summary>
    /// EXP cần để lên level kế tiếp.
    /// </summary>
    public decimal ExpToNextLevel { get; init; }

    /// <summary>
    /// Base ATK.
    /// </summary>
    public decimal BaseAtk { get; init; }

    /// <summary>
    /// Base DEF.
    /// </summary>
    public decimal BaseDef { get; init; }

    /// <summary>
    /// Bonus % ATK.
    /// </summary>
    public decimal BonusAtkPercent { get; init; }

    /// <summary>
    /// Bonus % DEF.
    /// </summary>
    public decimal BonusDefPercent { get; init; }

    /// <summary>
    /// Tổng ATK.
    /// </summary>
    public decimal TotalAtk { get; init; }

    /// <summary>
    /// Tổng DEF.
    /// </summary>
    public decimal TotalDef { get; init; }
}
