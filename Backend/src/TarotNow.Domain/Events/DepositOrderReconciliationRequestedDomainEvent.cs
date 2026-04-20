namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu reconcile trạng thái một đơn nạp theo dữ liệu trực tiếp từ PayOS.
/// </summary>
public sealed class DepositOrderReconciliationRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// User sở hữu đơn nạp.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Id đơn nạp cần reconcile.
    /// </summary>
    public Guid OrderId { get; init; }

    /// <summary>
    /// Cờ đánh dấu event đã được xử lý thành công.
    /// </summary>
    public bool Handled { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
