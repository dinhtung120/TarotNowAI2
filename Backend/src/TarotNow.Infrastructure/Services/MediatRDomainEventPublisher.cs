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
            _ => Task.CompletedTask
        };
    }
}
