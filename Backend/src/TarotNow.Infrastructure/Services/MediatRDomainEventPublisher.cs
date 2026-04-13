using System.Text.Json;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Publisher ghi domain event vào transactional outbox trên PostgreSQL.
/// </summary>
public sealed class MediatRDomainEventPublisher : IDomainEventPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo outbox domain event publisher.
    /// </summary>
    public MediatRDomainEventPublisher(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Ghi domain event vào outbox trong cùng DbContext transaction.
    /// </summary>
    public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (_dbContext.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException(
                "Domain events must be published inside an active transaction to guarantee atomic outbox writes.");
        }

        var eventType = domainEvent.GetType().FullName;
        if (string.IsNullOrWhiteSpace(eventType))
        {
            throw new InvalidOperationException($"Cannot resolve event type name for {domainEvent.GetType().Name}.");
        }

        var now = DateTime.UtcNow;
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = eventType,
            PayloadJson = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), JsonOptions),
            OccurredAtUtc = domainEvent.OccurredAtUtc,
            Status = OutboxMessageStatus.Pending,
            AttemptCount = 0,
            NextAttemptAtUtc = now,
            CreatedAtUtc = now,
            LockedAtUtc = null,
            LockOwner = null
        };

        _dbContext.OutboxMessages.Add(outboxMessage);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
