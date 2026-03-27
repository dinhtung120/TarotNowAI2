using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

/// <summary>
/// Wrapper MediatR cho sự kiện ConversationUpdatedDomainEvent.
/// </summary>
public record ConversationUpdatedNotification(ConversationUpdatedDomainEvent DomainEvent) : INotification;
