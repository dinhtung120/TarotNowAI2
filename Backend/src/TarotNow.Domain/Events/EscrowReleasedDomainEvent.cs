namespace TarotNow.Domain.Events;

// Domain event phát sinh khi escrow được giải ngân thành công.
public sealed class EscrowReleasedDomainEvent : IDomainEvent
{
    // Định danh item escrow đã release.
    public Guid ItemId { get; init; }

    // Người trả tiền.
    public Guid PayerId { get; init; }

    // Người nhận tiền.
    public Guid ReceiverId { get; init; }

    // Tổng số Diamond gốc của item.
    public long GrossAmountDiamond { get; init; }

    // Số Diamond thực nhận sau khi trừ phí.
    public long ReleasedAmountDiamond { get; init; }

    // Số Diamond phí nền tảng.
    public long FeeAmountDiamond { get; init; }

    // Cờ cho biết release tự động hay thủ công.
    public bool IsAutoRelease { get; init; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
