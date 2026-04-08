using MediatR;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Notifications;

// Notification bao domain event cập nhật hội thoại để phát qua MediatR pipeline.
public record ConversationUpdatedNotification(ConversationUpdatedDomainEvent DomainEvent) : INotification;
