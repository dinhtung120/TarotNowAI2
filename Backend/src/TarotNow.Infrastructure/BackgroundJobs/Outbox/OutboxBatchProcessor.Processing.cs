using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.Messaging.DomainEvents;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.BackgroundJobs.Outbox;

public sealed partial class OutboxBatchProcessor
{
    private async Task ProcessMessageInNewScopeAsync(Guid messageId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var scopedDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await using var transaction = await scopedDbContext.Database.BeginTransactionAsync(cancellationToken);
        var message = await scopedDbContext.OutboxMessages
            .FromSqlInterpolated($"SELECT * FROM outbox_messages WHERE id = {messageId} FOR UPDATE")
            .FirstOrDefaultAsync(cancellationToken);
        if (message == null
            || message.Status != OutboxMessageStatus.Processing
            || !string.Equals(message.LockOwner, WorkerId, StringComparison.Ordinal))
        {
            await transaction.RollbackAsync(cancellationToken);
            return;
        }

        await ProcessMessageAsync(message, scopedMediator, cancellationToken);
        await scopedDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    private async Task ProcessMessageAsync(OutboxMessage message, IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            await DispatchAsync(mediator, message, cancellationToken);
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

    private async Task DispatchAsync(IMediator mediator, OutboxMessage outboxMessage, CancellationToken cancellationToken)
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

        var eventIdempotencyKey = (domainEvent as IIdempotentDomainEvent)?.EventIdempotencyKey?.Trim();
        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventClrType);
        var notification = Activator.CreateInstance(
            notificationType,
            domainEvent,
            outboxMessage.Id,
            eventIdempotencyKey) as INotification;
        if (notification == null)
        {
            throw new InvalidOperationException($"Cannot create notification for outbox message {outboxMessage.Id}.");
        }

        await mediator.Publish(notification, cancellationToken);
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
        var isDeadLetter = message.AttemptCount >= ResolveMaxRetryAttempts();

        message.Status = isDeadLetter ? OutboxMessageStatus.DeadLetter : OutboxMessageStatus.Failed;
        message.NextAttemptAtUtc = now.AddSeconds(CalculateBackoffSeconds(message.AttemptCount));
        message.LastError = Truncate(exception.ToString(), LastErrorMaxLength);
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

    private int CalculateBackoffSeconds(int attemptCount)
    {
        var seconds = Math.Pow(2, attemptCount);
        return (int)Math.Min(ResolveMaxBackoffSeconds(), seconds);
    }

    private static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..maxLength];
    }

    private int ResolveParallelism()
    {
        return Math.Clamp(_systemConfigSettings.OperationalOutboxParallelism, 1, 64);
    }

    private int ResolveMaxRetryAttempts()
    {
        return Math.Clamp(_systemConfigSettings.OperationalOutboxMaxRetryAttempts, 1, 100);
    }

    private TimeSpan ResolveLockTimeout()
    {
        var seconds = _systemConfigSettings.OperationalOutboxLockTimeoutSeconds;
        return TimeSpan.FromSeconds(Math.Clamp(seconds, 30, 3600));
    }

    private int ResolveMaxBackoffSeconds()
    {
        return Math.Clamp(_systemConfigSettings.OperationalOutboxMaxBackoffSeconds, 1, 3600);
    }
}
