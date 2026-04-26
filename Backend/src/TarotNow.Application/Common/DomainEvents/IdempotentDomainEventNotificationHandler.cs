using MediatR;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Common.DomainEvents;

/// <summary>
/// Base handler hỗ trợ idempotency cho domain event notifications đọc từ outbox.
/// </summary>
public abstract class IdempotentDomainEventNotificationHandler<TDomainEvent>
    : INotificationHandler<DomainEventNotification<TDomainEvent>>
    where TDomainEvent : IDomainEvent
{
    private readonly IEventHandlerIdempotencyService _idempotencyService;

    /// <summary>
    /// Khởi tạo base idempotent domain event handler.
    /// </summary>
    protected IdempotentDomainEventNotificationHandler(IEventHandlerIdempotencyService idempotencyService)
    {
        _idempotencyService = idempotencyService;
    }

    /// <inheritdoc />
    public async Task Handle(DomainEventNotification<TDomainEvent> notification, CancellationToken cancellationToken)
    {
        var outboxMessageId = notification.OutboxMessageId;
        var eventIdempotencyKey = notification.EventIdempotencyKey?.Trim();
        var handlerName = GetType().FullName ?? GetType().Name;

        if (await IsAlreadyProcessedAsync(outboxMessageId, eventIdempotencyKey, handlerName, cancellationToken))
        {
            return;
        }

        await HandleDomainEventAsync(notification.DomainEvent, outboxMessageId, cancellationToken);

        await MarkProcessedAsync(outboxMessageId, eventIdempotencyKey, handlerName, cancellationToken);
    }

    private async Task<bool> IsAlreadyProcessedAsync(
        Guid? outboxMessageId,
        string? eventIdempotencyKey,
        string handlerName,
        CancellationToken cancellationToken)
    {
        if (outboxMessageId.HasValue
            && await _idempotencyService.HasProcessedAsync(outboxMessageId.Value, handlerName, cancellationToken))
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(eventIdempotencyKey)
               && await _idempotencyService.HasProcessedInlineEventAsync(
                   eventIdempotencyKey,
                   handlerName,
                   cancellationToken);
    }

    private async Task MarkProcessedAsync(
        Guid? outboxMessageId,
        string? eventIdempotencyKey,
        string handlerName,
        CancellationToken cancellationToken)
    {
        if (outboxMessageId.HasValue)
        {
            await _idempotencyService.MarkProcessedAsync(outboxMessageId.Value, handlerName, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(eventIdempotencyKey))
        {
            await _idempotencyService.MarkInlineEventProcessedAsync(eventIdempotencyKey, handlerName, cancellationToken);
        }
    }

    /// <summary>
    /// Xử lý domain event cụ thể ở lớp dẫn xuất.
    /// </summary>
    protected abstract Task HandleDomainEventAsync(
        TDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken);
}
