namespace TarotNow.Domain.Events;

// Domain event phát sinh khi billing phiên đọc bài đã chốt trạng thái cuối.
public sealed class ReadingBillingCompletedDomainEvent : IDomainEvent
{
    // Người dùng của phiên billing.
    public Guid UserId { get; init; }

    // Yêu cầu AI liên quan billing.
    public Guid AiRequestId { get; init; }

    // Tham chiếu phiên đọc bài.
    public string ReadingSessionRef { get; init; } = string.Empty;

    // Số Diamond đã thu cho lượt xử lý.
    public long ChargeDiamond { get; init; }

    // Trạng thái cuối của quá trình AI/billing.
    public string FinalStatus { get; init; } = string.Empty;

    // Cờ cho biết có hoàn tiền hay không.
    public bool WasRefunded { get; init; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
