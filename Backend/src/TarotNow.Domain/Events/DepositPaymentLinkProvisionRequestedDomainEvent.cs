namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu tạo payment link cho đơn nạp đã được persist.
/// </summary>
public sealed class DepositPaymentLinkProvisionRequestedDomainEvent : IIdempotentDomainEvent
{
    /// <summary>
    /// Định danh đơn nạp cần provision payment link.
    /// </summary>
    public Guid OrderId { get; init; }

    /// <summary>
    /// PayOS order code đã cấp cho đơn nạp.
    /// </summary>
    public long PayOsOrderCode { get; init; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventIdempotencyKey => $"deposit:payment-link:{OrderId:N}";
}
