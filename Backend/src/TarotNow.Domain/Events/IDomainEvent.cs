namespace TarotNow.Domain.Events;

// Contract chung cho mọi domain event để chuẩn hóa mốc thời gian phát sinh.
public interface IDomainEvent
{
    // Thời điểm sự kiện phát sinh theo UTC.
    DateTime OccurredAtUtc { get; }
}
