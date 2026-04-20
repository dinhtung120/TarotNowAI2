namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi đơn nạp thanh toán thành công, dùng để cấp ví.
/// </summary>
public sealed class DepositPaymentSucceededDomainEvent : IDomainEvent
{
    /// <summary>
    /// Id đơn nạp thành công.
    /// </summary>
    public Guid OrderId { get; init; }

    /// <summary>
    /// User sở hữu đơn nạp.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Diamond cần cộng.
    /// </summary>
    public long DiamondAmount { get; init; }

    /// <summary>
    /// Gold khuyến mãi cần cộng.
    /// </summary>
    public long BonusGoldAmount { get; init; }

    /// <summary>
    /// Mã giao dịch từ gateway.
    /// </summary>
    public string ReferenceId { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
