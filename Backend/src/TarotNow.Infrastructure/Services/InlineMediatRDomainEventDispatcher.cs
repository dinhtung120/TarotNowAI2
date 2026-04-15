using MediatR;
using System.Collections.Concurrent;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Dispatcher publish domain event đồng bộ bằng MediatR trong request hiện tại.
/// </summary>
public sealed class InlineMediatRDomainEventDispatcher : IInlineDomainEventDispatcher
{
    private static readonly ConcurrentDictionary<Type, Func<IDomainEvent, INotification>> NotificationFactoryCache = new();

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
        var notificationFactory = NotificationFactoryCache.GetOrAdd(eventType, CreateNotificationFactory);
        var notification = notificationFactory(domainEvent);
        return _mediator.Publish(notification, cancellationToken);
    }

    private static Func<IDomainEvent, INotification> CreateNotificationFactory(Type eventType)
    {
        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventType);
        var constructor = notificationType.GetConstructor(new[] { eventType, typeof(Guid?) });
        if (constructor is null)
        {
            throw new InvalidOperationException($"Cannot find DomainEventNotification constructor for {eventType.FullName}.");
        }

        return domainEvent =>
        {
            var notification = constructor.Invoke(new object?[] { domainEvent, null }) as INotification;
            if (notification is null)
            {
                throw new InvalidOperationException($"Cannot create DomainEventNotification for {eventType.FullName}.");
            }

            return notification;
        };
    }
}
