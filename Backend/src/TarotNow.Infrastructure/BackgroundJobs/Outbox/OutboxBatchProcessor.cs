using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.BackgroundJobs.Outbox;

/// <summary>
/// Xử lý một vòng claim/dispatch outbox batch.
/// </summary>
public sealed partial class OutboxBatchProcessor : IOutboxBatchProcessor
{
    private const int LastErrorMaxLength = 3900;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly string WorkerId = $"{Environment.MachineName}:{Guid.NewGuid():N}";

    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxBatchProcessor> _logger;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo outbox batch processor.
    /// </summary>
    public OutboxBatchProcessor(
        ApplicationDbContext dbContext,
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxBatchProcessor> logger,
        ISystemConfigSettings systemConfigSettings)
    {
        _dbContext = dbContext;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <inheritdoc />
    public async Task<int> ProcessOnceAsync(CancellationToken cancellationToken = default)
    {
        var messages = await ClaimBatchAsync(cancellationToken);
        if (messages.Count == 0)
        {
            return 0;
        }

        var parallelism = ResolveParallelism();
        var partitions = messages
            .GroupBy(message => ResolveProcessingPartitionKey(message), StringComparer.Ordinal)
            .Select(group => group.OrderBy(message => message.CreatedAtUtc).ToList())
            .ToList();

        using var throttler = new SemaphoreSlim(parallelism);
        var tasks = partitions.Select(async partition =>
        {
            await throttler.WaitAsync(cancellationToken);
            try
            {
                foreach (var message in partition)
                {
                    await ProcessMessageInNewScopeAsync(message.Id, cancellationToken);
                }
            }
            finally
            {
                throttler.Release();
            }
        });
        await Task.WhenAll(tasks);

        _dbContext.ChangeTracker.Clear();
        return messages.Count;
    }

    private async Task<List<OutboxMessage>> ClaimBatchAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var staleLockThreshold = now - ResolveLockTimeout();
        var batchSize = ResolveBatchSize();

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
LIMIT {batchSize}
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

    private int ResolveBatchSize()
    {
        return Math.Clamp(_systemConfigSettings.OperationalOutboxBatchSize, 1, 5000);
    }
}
