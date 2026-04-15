using TarotNow.Domain.Events;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Dispatcher publish domain event theo cơ chế đồng bộ trong request hiện tại.
/// </summary>
public interface IInlineDomainEventDispatcher
{
    /// <summary>
    /// Publish domain event tới MediatR notification handlers ngay trong request hiện tại.
    /// </summary>
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
