using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

public sealed class EscrowReleasedNotification : INotification
{
    public EscrowReleasedNotification(EscrowReleasedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public EscrowReleasedDomainEvent DomainEvent { get; }
}
