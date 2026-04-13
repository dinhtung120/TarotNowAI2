using TarotNow.Domain.Events;

namespace TarotNow.Application.Interfaces;

// Contract publish domain event để tách nơi phát sinh sự kiện và nơi xử lý subscriber.
public interface IDomainEventPublisher
{
    /// <summary>
    /// Ghi một domain event vào transactional outbox để xử lý bất đồng bộ.
    /// Luồng xử lý: yêu cầu transaction đang mở để business write và outbox enqueue được commit atomically.
    /// </summary>
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
