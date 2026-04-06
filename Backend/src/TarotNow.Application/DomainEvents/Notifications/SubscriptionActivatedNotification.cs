using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

public sealed class SubscriptionActivatedNotification : INotification
{
    public SubscriptionActivatedNotification(SubscriptionActivatedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public SubscriptionActivatedDomainEvent DomainEvent { get; }
}
