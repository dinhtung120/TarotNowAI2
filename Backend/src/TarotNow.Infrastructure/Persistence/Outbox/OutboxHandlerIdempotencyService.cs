using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Triển khai idempotency cho domain event handlers dựa trên bảng outbox_handler_states.
/// </summary>
public sealed class OutboxHandlerIdempotencyService : IEventHandlerIdempotencyService
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo service idempotency handler.
    /// </summary>
    public OutboxHandlerIdempotencyService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<bool> HasProcessedAsync(
        Guid outboxMessageId,
        string handlerName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(handlerName))
        {
            throw new ArgumentException("Handler name is required.", nameof(handlerName));
        }

        return _dbContext.OutboxHandlerStates
            .AnyAsync(x => x.OutboxMessageId == outboxMessageId && x.HandlerName == handlerName, cancellationToken);
    }

    /// <inheritdoc />
    public async Task MarkProcessedAsync(
        Guid outboxMessageId,
        string handlerName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(handlerName))
        {
            throw new ArgumentException("Handler name is required.", nameof(handlerName));
        }

        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             INSERT INTO outbox_handler_states (id, outbox_message_id, handler_name, processed_at_utc)
             VALUES ({Guid.NewGuid()}, {outboxMessageId}, {handlerName}, {DateTime.UtcNow})
             ON CONFLICT (outbox_message_id, handler_name) DO NOTHING;
             """,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> HasProcessedInlineEventAsync(
        string eventKey,
        string handlerName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(eventKey))
        {
            throw new ArgumentException("Event key is required.", nameof(eventKey));
        }

        if (string.IsNullOrWhiteSpace(handlerName))
        {
            throw new ArgumentException("Handler name is required.", nameof(handlerName));
        }

        var normalizedEventKey = eventKey.Trim();
        return _dbContext.OutboxInlineHandlerStates
            .AnyAsync(
                x => x.EventKey == normalizedEventKey && x.HandlerName == handlerName,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task MarkInlineEventProcessedAsync(
        string eventKey,
        string handlerName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(eventKey))
        {
            throw new ArgumentException("Event key is required.", nameof(eventKey));
        }

        if (string.IsNullOrWhiteSpace(handlerName))
        {
            throw new ArgumentException("Handler name is required.", nameof(handlerName));
        }

        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             INSERT INTO outbox_inline_handler_states (id, event_key, handler_name, processed_at_utc)
             VALUES ({Guid.NewGuid()}, {eventKey.Trim()}, {handlerName}, {DateTime.UtcNow})
             ON CONFLICT (event_key, handler_name) DO NOTHING;
             """,
            cancellationToken);
    }
}
