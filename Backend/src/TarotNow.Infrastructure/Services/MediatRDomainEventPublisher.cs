using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

public sealed class MediatRDomainEventPublisher : IDomainEventPublisher
{
    private static readonly IReadOnlyDictionary<Type, Func<IDomainEvent, INotification>> NotificationFactories =
        new Dictionary<Type, Func<IDomainEvent, INotification>>
        {
            [typeof(EscrowReleasedDomainEvent)] = domainEvent =>
                new EscrowReleasedNotification((EscrowReleasedDomainEvent)domainEvent),
            [typeof(EscrowRefundedDomainEvent)] = domainEvent =>
                new EscrowRefundedNotification((EscrowRefundedDomainEvent)domainEvent),
            [typeof(ReadingBillingCompletedDomainEvent)] = domainEvent =>
                new ReadingBillingCompletedNotification((ReadingBillingCompletedDomainEvent)domainEvent),
            [typeof(ConversationUpdatedDomainEvent)] = domainEvent =>
                new ConversationUpdatedNotification((ConversationUpdatedDomainEvent)domainEvent),
            [typeof(SubscriptionActivatedDomainEvent)] = domainEvent =>
                new SubscriptionActivatedNotification((SubscriptionActivatedDomainEvent)domainEvent),
            [typeof(SubscriptionExpiredDomainEvent)] = domainEvent =>
                new SubscriptionExpiredNotification((SubscriptionExpiredDomainEvent)domainEvent),
            [typeof(EntitlementConsumedDomainEvent)] = domainEvent =>
                new EntitlementConsumedNotification((EntitlementConsumedDomainEvent)domainEvent)
        };

    private readonly IMediator _mediator;

    public MediatRDomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = MapNotification(domainEvent);
        return notification == null
            ? Task.CompletedTask
            : _mediator.Publish(notification, cancellationToken);
    }

    private static INotification? MapNotification(IDomainEvent domainEvent)
    {
        return NotificationFactories.TryGetValue(domainEvent.GetType(), out var factory)
            ? factory(domainEvent)
            : null;
    }
}
