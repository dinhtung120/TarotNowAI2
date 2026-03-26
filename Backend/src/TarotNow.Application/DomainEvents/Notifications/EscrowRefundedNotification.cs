using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

public sealed class EscrowRefundedNotification : INotification
{
    public EscrowRefundedNotification(EscrowRefundedDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public EscrowRefundedDomainEvent DomainEvent { get; }
}
