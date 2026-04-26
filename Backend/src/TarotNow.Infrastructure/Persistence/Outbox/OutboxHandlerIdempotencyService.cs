using Microsoft.EntityFrameworkCore;
using Npgsql;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Triển khai idempotency cho domain event handlers dựa trên bảng outbox_handler_states.
/// </summary>
public sealed class OutboxHandlerIdempotencyService : IEventHandlerIdempotencyService
{
    private const string HandlerStateUniqueConstraintName = "ux_outbox_handler_states_message_handler";
    private const string InlineHandlerStateUniqueConstraintName = "ux_outbox_inline_handler_states_event_handler";

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

        _dbContext.OutboxHandlerStates.Add(new OutboxHandlerState
        {
            Id = Guid.NewGuid(),
            OutboxMessageId = outboxMessageId,
            HandlerName = handlerName,
            ProcessedAtUtc = DateTime.UtcNow
        });

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsHandledUniqueViolation(exception))
        {
            // Unique constraint hit => handler đã được đánh dấu bởi luồng khác.
        }
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

        _dbContext.OutboxInlineHandlerStates.Add(new OutboxInlineHandlerState
        {
            Id = Guid.NewGuid(),
            EventKey = eventKey.Trim(),
            HandlerName = handlerName,
            ProcessedAtUtc = DateTime.UtcNow
        });

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsHandledUniqueViolation(exception))
        {
            // Unique constraint hit => handler đã được đánh dấu bởi luồng khác.
        }
    }

    private static bool IsHandledUniqueViolation(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && (
                   string.Equals(
                       postgresException.ConstraintName,
                       HandlerStateUniqueConstraintName,
                       StringComparison.OrdinalIgnoreCase)
                   || string.Equals(
                       postgresException.ConstraintName,
                       InlineHandlerStateUniqueConstraintName,
                       StringComparison.OrdinalIgnoreCase));
    }
}
