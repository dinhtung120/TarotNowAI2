using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.Messaging.DomainEvents;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.BackgroundJobs.Outbox;

/// <summary>
/// Xử lý một vòng claim/dispatch outbox batch.
/// </summary>
public sealed class OutboxBatchProcessor : IOutboxBatchProcessor
{
    private const int BatchSize = 50;
    private const int MaxRetryAttempts = 12;
    private static readonly TimeSpan LockTimeout = TimeSpan.FromMinutes(2);
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly string WorkerId = $"{Environment.MachineName}:{Guid.NewGuid():N}";

    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ILogger<OutboxBatchProcessor> _logger;

    /// <summary>
    /// Khởi tạo outbox batch processor.
    /// </summary>
    public OutboxBatchProcessor(
        ApplicationDbContext dbContext,
        IMediator mediator,
        ILogger<OutboxBatchProcessor> logger)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ProcessOnceAsync(CancellationToken cancellationToken = default)
    {
        var messages = await ClaimBatchAsync(cancellationToken);
        if (messages.Count == 0)
        {
            return;
        }

        foreach (var message in messages)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            await ProcessMessageAsync(message, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        _dbContext.ChangeTracker.Clear();
    }

    private async Task<List<OutboxMessage>> ClaimBatchAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var staleLockThreshold = now - LockTimeout;

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var messages = await _dbContext.OutboxMessages
            .FromSqlInterpolated($@"
SELECT *
FROM outbox_messages
WHERE (
        ((status = {OutboxMessageStatus.Pending} OR status = {OutboxMessageStatus.Failed})
          AND next_attempt_at_utc <= {now})
        OR (status = {OutboxMessageStatus.Processing}
            AND locked_at_utc IS NOT NULL
            AND locked_at_utc <= {staleLockThreshold})
      )
ORDER BY created_at_utc
LIMIT {BatchSize}
FOR UPDATE SKIP LOCKED")
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            message.Status = OutboxMessageStatus.Processing;
            message.LockedAtUtc = now;
            message.LockOwner = WorkerId;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return messages;
    }

    private async Task ProcessMessageAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        try
        {
            await DispatchAsync(message, cancellationToken);
            MarkProcessed(message);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            MarkFailed(message, exception);
        }
    }

    private async Task DispatchAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        if (DomainEventTypeRegistry.TryResolve(outboxMessage.EventType, out var eventClrType) == false)
        {
            throw new InvalidOperationException($"Unknown domain event type '{outboxMessage.EventType}'.");
        }

        var deserialized = JsonSerializer.Deserialize(outboxMessage.PayloadJson, eventClrType, JsonOptions);
        if (deserialized is not IDomainEvent domainEvent)
        {
            throw new InvalidOperationException($"Cannot deserialize payload as IDomainEvent. OutboxId={outboxMessage.Id}");
        }

        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventClrType);
        var notification = Activator.CreateInstance(notificationType, domainEvent, outboxMessage.Id) as INotification;
        if (notification == null)
        {
            throw new InvalidOperationException($"Cannot create notification for outbox message {outboxMessage.Id}.");
        }

        await _mediator.Publish(notification, cancellationToken);
    }

    private void MarkProcessed(OutboxMessage message)
    {
        message.Status = OutboxMessageStatus.Processed;
        message.ProcessedAtUtc = DateTime.UtcNow;
        message.LastError = null;
        message.LockedAtUtc = null;
        message.LockOwner = null;
    }

    private void MarkFailed(OutboxMessage message, Exception exception)
    {
        message.AttemptCount += 1;
        var now = DateTime.UtcNow;
        var isDeadLetter = message.AttemptCount >= MaxRetryAttempts;

        message.Status = isDeadLetter ? OutboxMessageStatus.DeadLetter : OutboxMessageStatus.Failed;
        message.NextAttemptAtUtc = now.AddSeconds(CalculateBackoffSeconds(message.AttemptCount));
        message.LastError = Truncate(exception.ToString(), 3900);
        message.LockedAtUtc = null;
        message.LockOwner = null;

        if (isDeadLetter)
        {
            _logger.LogError(
                exception,
                "Outbox message moved to dead-letter. MessageId={MessageId}, Attempts={AttemptCount}",
                message.Id,
                message.AttemptCount);
            return;
        }

        _logger.LogWarning(
            exception,
            "Outbox message failed and will retry. MessageId={MessageId}, Attempts={AttemptCount}, NextAttemptAtUtc={NextAttemptAtUtc}",
            message.Id,
            message.AttemptCount,
            message.NextAttemptAtUtc);
    }

    private static int CalculateBackoffSeconds(int attemptCount)
    {
        var seconds = Math.Pow(2, attemptCount);
        return (int)Math.Min(300, seconds);
    }

    private static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
