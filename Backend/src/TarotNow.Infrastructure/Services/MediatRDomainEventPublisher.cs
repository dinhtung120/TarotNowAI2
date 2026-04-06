using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Services;

public sealed class MediatRDomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public MediatRDomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        return domainEvent switch
        {
            EscrowReleasedDomainEvent released => _mediator.Publish(new EscrowReleasedNotification(released), cancellationToken),
            EscrowRefundedDomainEvent refunded => _mediator.Publish(new EscrowRefundedNotification(refunded), cancellationToken),
            ReadingBillingCompletedDomainEvent readingBilling => _mediator.Publish(new ReadingBillingCompletedNotification(readingBilling), cancellationToken),
            ConversationUpdatedDomainEvent updated => _mediator.Publish(new ConversationUpdatedNotification(updated), cancellationToken),
            SubscriptionActivatedDomainEvent subAct => _mediator.Publish(new SubscriptionActivatedNotification(subAct), cancellationToken),
            SubscriptionExpiredDomainEvent subExp => _mediator.Publish(new SubscriptionExpiredNotification(subExp), cancellationToken),
            EntitlementConsumedDomainEvent entCons => _mediator.Publish(new EntitlementConsumedNotification(entCons), cancellationToken),
            _ => Task.CompletedTask
        };
    }
}
