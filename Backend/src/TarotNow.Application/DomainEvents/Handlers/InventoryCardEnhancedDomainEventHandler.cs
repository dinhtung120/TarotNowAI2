using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler hậu xử lý khi card được enhancement từ inventory item.
/// </summary>
public sealed class CardEnhancedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CardEnhancedDomainEvent>
{
    /// <summary>
    /// Khởi tạo handler card enhanced.
    /// </summary>
    public CardEnhancedDomainEventHandler(
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        CardEnhancedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // In-app notification cho card enhancement đã bị tắt để tránh spam.
        return Task.CompletedTask;
    }
}
