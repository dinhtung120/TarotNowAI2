using MediatR;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Dispatcher publish domain event đồng bộ bằng MediatR trong request hiện tại.
/// </summary>
public sealed class InlineMediatRDomainEventDispatcher : IInlineDomainEventDispatcher
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Khởi tạo dispatcher inline domain events.
    /// </summary>
    public InlineMediatRDomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var eventType = domainEvent.GetType();
        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventType);
        var notification = Activator.CreateInstance(notificationType, domainEvent, null) as INotification;
        if (notification is null)
        {
            throw new InvalidOperationException($"Cannot create DomainEventNotification for {eventType.FullName}.");
        }

        return _mediator.Publish(notification, cancellationToken);
    }
}
