using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

public record ConversationUpdatedNotification(ConversationUpdatedDomainEvent DomainEvent) : INotification;
