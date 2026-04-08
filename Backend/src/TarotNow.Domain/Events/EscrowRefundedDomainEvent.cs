namespace TarotNow.Domain.Events;

// Domain event phát sinh khi khoản escrow được hoàn tiền.
public sealed class EscrowRefundedDomainEvent : IDomainEvent
{
    // Định danh item escrow đã hoàn tiền.
    public Guid ItemId { get; init; }

    // Người dùng nhận hoàn tiền.
    public Guid UserId { get; init; }

    // Số Diamond được hoàn.
    public long AmountDiamond { get; init; }

    // Nguồn hoàn tiền (auto/manual/...).
    public string RefundSource { get; init; } = string.Empty;

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
