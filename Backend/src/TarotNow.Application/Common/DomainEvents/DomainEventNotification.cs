using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Common.DomainEvents;

/// <summary>
/// Generic MediatR notification wrapper cho mọi Domain Event.
/// </summary>
/// <typeparam name="TDomainEvent">Kiểu domain event cụ thể.</typeparam>
public sealed record DomainEventNotification<TDomainEvent>(
    TDomainEvent DomainEvent,
    Guid? OutboxMessageId = null) : INotification
    where TDomainEvent : IDomainEvent;
