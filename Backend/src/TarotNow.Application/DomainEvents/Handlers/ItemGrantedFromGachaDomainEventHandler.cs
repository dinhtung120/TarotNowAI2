using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Gacha;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects phụ khi gacha cấp item cho người dùng.
/// </summary>
public sealed class ItemGrantedFromGachaDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ItemGrantedFromGachaDomainEvent>
{
    /// <summary>
    /// Khởi tạo handler.
    /// </summary>
    public ItemGrantedFromGachaDomainEventHandler(
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        ItemGrantedFromGachaDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // In-app notification cho reward gacha đã bị tắt để tránh spam.
        return Task.CompletedTask;
    }
}
