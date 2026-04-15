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
}
