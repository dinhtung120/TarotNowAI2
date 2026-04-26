namespace TarotNow.Domain.Events;

// Contract chung cho mọi domain event để chuẩn hóa mốc thời gian phát sinh.
public interface IDomainEvent
{
    // Thời điểm sự kiện phát sinh theo UTC.
    DateTime OccurredAtUtc { get; }
}

/// <summary>
/// Optional contract cho domain event cần dedupe ở luồng inline (không qua outbox message id).
/// </summary>
public interface IIdempotentDomainEvent : IDomainEvent
{
    /// <summary>
    /// Idempotency key ổn định cho event để tránh handler xử lý lặp.
    /// </summary>
    string EventIdempotencyKey { get; }
}
