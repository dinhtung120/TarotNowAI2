using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

public sealed class SubscriptionExpiredNotification : INotification
{
    public SubscriptionExpiredNotification(SubscriptionExpiredDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public SubscriptionExpiredDomainEvent DomainEvent { get; }
}
