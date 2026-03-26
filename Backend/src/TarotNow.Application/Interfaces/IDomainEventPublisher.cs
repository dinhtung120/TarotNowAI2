using TarotNow.Domain.Events;

namespace TarotNow.Application.Interfaces;

public interface IDomainEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
