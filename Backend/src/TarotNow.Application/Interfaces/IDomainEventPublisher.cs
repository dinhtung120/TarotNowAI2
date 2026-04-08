using TarotNow.Domain.Events;

namespace TarotNow.Application.Interfaces;

// Contract publish domain event để tách nơi phát sinh sự kiện và nơi xử lý subscriber.
public interface IDomainEventPublisher
{
    /// <summary>
    /// Phát một domain event ra pipeline xử lý để kích hoạt các side-effect liên quan.
    /// Luồng xử lý: nhận event đã phát sinh từ domain và dispatch tới các handler đăng ký.
    /// </summary>
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
