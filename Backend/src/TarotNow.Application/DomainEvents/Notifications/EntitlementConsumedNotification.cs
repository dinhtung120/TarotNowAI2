using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

public sealed class EntitlementConsumedNotification : INotification
{
    public EntitlementConsumedNotification(EntitlementConsumedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public EntitlementConsumedDomainEvent DomainEvent { get; }
}
