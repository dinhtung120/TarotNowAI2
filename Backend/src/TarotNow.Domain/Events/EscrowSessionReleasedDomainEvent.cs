namespace TarotNow.Domain.Events;

// Domain event phát sinh khi escrow của toàn finance session được giải ngân thành công.
public sealed class EscrowSessionReleasedDomainEvent : IDomainEvent
{
    // Định danh finance session đã release.
    public Guid FinanceSessionId { get; init; }

    // Người trả tiền của session.
    public Guid PayerId { get; init; }

    // Reader nhận tiền của session.
    public Guid ReceiverId { get; init; }

    // Tổng số Diamond gốc của toàn bộ accepted item trong session.
    public long GrossAmountDiamond { get; init; }

    // Số Diamond thực nhận sau khi trừ phí.
    public long ReleasedAmountDiamond { get; init; }

    // Số Diamond phí nền tảng.
    public long FeeAmountDiamond { get; init; }

    // Số item được release trong đợt settlement này.
    public int ReleasedItemCount { get; init; }

    // Cờ cho biết release tự động hay thủ công.
    public bool IsAutoRelease { get; init; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
