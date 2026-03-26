using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

public sealed class ReadingBillingCompletedNotification : INotification
{
    public ReadingBillingCompletedNotification(ReadingBillingCompletedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public ReadingBillingCompletedDomainEvent DomainEvent { get; }
}
