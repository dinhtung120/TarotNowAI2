using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.Outbox;

namespace TarotNow.Infrastructure.BackgroundJobs;

/// <summary>
/// Worker reconcile projection để tự-heal drift Reading/Profile khi outbox bị dead-letter hoặc projection lệch dữ liệu.
/// </summary>
public sealed class ProjectionReconcileWorker : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(2);
    private static readonly string[] RequeueEventTypes =
    {
        typeof(ReadingSessionContentSyncRequestedDomainEvent).FullName!,
        typeof(UserProfileProjectionSyncRequestedDomainEvent).FullName!
    };

    private const int RequeueBatchSize = 100;
    private const int ProfileScanBatchSize = 200;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProjectionReconcileWorker> _logger;

    public ProjectionReconcileWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<ProjectionReconcileWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[ProjectionReconcile] Worker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOnceAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "[ProjectionReconcile] Iteration failed.");
            }

            await Task.Delay(PollInterval, stoppingToken);
        }

        _logger.LogInformation("[ProjectionReconcile] Worker stopped.");
    }

    private async Task ProcessOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var readerProfileRepository = scope.ServiceProvider.GetRequiredService<IReaderProfileRepository>();

        var requeuedDeadLetters = await RequeueDeadLetterProjectionEventsAsync(dbContext, cancellationToken);
        var repairedProfiles = await ReconcileReaderProfilesAsync(
            dbContext,
            readerProfileRepository,
            cancellationToken);

        if (requeuedDeadLetters > 0 || repairedProfiles > 0)
        {
            _logger.LogInformation(
                "[ProjectionReconcile] Completed iteration. Requeued={RequeuedDeadLetters}, RepairedProfiles={RepairedProfiles}",
                requeuedDeadLetters,
                repairedProfiles);
        }
    }

    private static async Task<int> RequeueDeadLetterProjectionEventsAsync(
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var candidates = await dbContext.OutboxMessages
            .Where(message =>
                message.Status == OutboxMessageStatus.DeadLetter
                && RequeueEventTypes.Contains(message.EventType))
            .OrderBy(message => message.CreatedAtUtc)
            .Take(RequeueBatchSize)
            .ToListAsync(cancellationToken);
        if (candidates.Count == 0)
        {
            return 0;
        }

        var now = DateTime.UtcNow;
        foreach (var candidate in candidates)
        {
            candidate.Status = OutboxMessageStatus.Failed;
            candidate.NextAttemptAtUtc = now;
            candidate.LockedAtUtc = null;
            candidate.LockOwner = null;
            candidate.LastError = BuildRequeueAuditMessage(candidate.LastError, now);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return candidates.Count;
    }

    private static async Task<int> ReconcileReaderProfilesAsync(
        ApplicationDbContext dbContext,
        IReaderProfileRepository readerProfileRepository,
        CancellationToken cancellationToken)
    {
        var readers = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Role == UserRole.TarotReader)
            .OrderByDescending(user => user.UpdatedAt ?? user.CreatedAt)
            .Take(ProfileScanBatchSize)
            .Select(user => new ReaderProjectionSource(
                user.Id.ToString(),
                user.DisplayName,
                user.AvatarUrl))
            .ToListAsync(cancellationToken);
        if (readers.Count == 0)
        {
            return 0;
        }

        var existingProfiles = (await readerProfileRepository.GetByUserIdsAsync(
                readers.Select(reader => reader.UserId),
                cancellationToken))
            .ToDictionary(profile => profile.UserId, StringComparer.Ordinal);

        var repairedCount = 0;
        foreach (var reader in readers)
        {
            if (!existingProfiles.TryGetValue(reader.UserId, out var profile))
            {
                continue;
            }

            if (string.Equals(profile.DisplayName, reader.DisplayName, StringComparison.Ordinal)
                && string.Equals(profile.AvatarUrl, reader.AvatarUrl, StringComparison.Ordinal))
            {
                continue;
            }

            profile.DisplayName = reader.DisplayName;
            profile.AvatarUrl = reader.AvatarUrl;
            await readerProfileRepository.UpdateAsync(profile, cancellationToken);
            repairedCount++;
        }

        return repairedCount;
    }

    private static string BuildRequeueAuditMessage(string? previousError, DateTime requeuedAtUtc)
    {
        const int maxLength = 3900;
        var audit = $"requeued_at={requeuedAtUtc:O}";
        if (string.IsNullOrWhiteSpace(previousError))
        {
            return audit;
        }

        var previous = previousError.Trim();
        var merged = $"{audit} | previous={previous}";
        return merged.Length <= maxLength ? merged : merged[..maxLength];
    }

    private readonly record struct ReaderProjectionSource(
        string UserId,
        string DisplayName,
        string? AvatarUrl);
}
