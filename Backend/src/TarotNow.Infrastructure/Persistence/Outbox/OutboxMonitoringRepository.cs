using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Features.Admin.Queries.GetOutboxDashboard;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Repository đọc dữ liệu vận hành outbox cho dashboard admin.
/// </summary>
public sealed class OutboxMonitoringRepository : IOutboxMonitoringRepository
{
    private const int LastErrorPreviewMaxLength = 240;

    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository monitoring outbox.
    /// </summary>
    public OutboxMonitoringRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<OutboxDashboardDto> GetDashboardAsync(
        int top,
        DateTime nowUtc,
        CancellationToken cancellationToken = default)
    {
        var statusCounts = await GetStatusCountsAsync(cancellationToken);
        var oldestPendingAge = await GetOldestAgeSecondsAsync(OutboxMessageStatus.Pending, nowUtc, cancellationToken);
        var oldestFailedAge = await GetOldestAgeSecondsAsync(OutboxMessageStatus.Failed, nowUtc, cancellationToken);
        var oldestDeadLetterAge = await GetOldestAgeSecondsAsync(OutboxMessageStatus.DeadLetter, nowUtc, cancellationToken);
        var retryOverdueCount = await GetRetryOverdueCountAsync(nowUtc, cancellationToken);
        var maxRetryAge = await GetMaxRetryAgeSecondsAsync(nowUtc, cancellationToken);

        return new OutboxDashboardDto
        {
            PendingCount = GetCount(statusCounts, OutboxMessageStatus.Pending),
            ProcessingCount = GetCount(statusCounts, OutboxMessageStatus.Processing),
            FailedCount = GetCount(statusCounts, OutboxMessageStatus.Failed),
            DeadLetterCount = GetCount(statusCounts, OutboxMessageStatus.DeadLetter),
            RetryOverdueCount = retryOverdueCount,
            OldestPendingAgeSeconds = oldestPendingAge,
            OldestFailedAgeSeconds = oldestFailedAge,
            OldestDeadLetterAgeSeconds = oldestDeadLetterAge,
            MaxRetryAgeSeconds = maxRetryAge,
            TopFailed = await GetTopFailedAsync(top, nowUtc, cancellationToken),
            TopDeadLetters = await GetTopDeadLettersAsync(top, nowUtc, cancellationToken)
        };
    }

    private async Task<IReadOnlyDictionary<string, int>> GetStatusCountsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.OutboxMessages
            .AsNoTracking()
            .GroupBy(message => message.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count()
            })
            .ToDictionaryAsync(
                item => item.Status,
                item => item.Count,
                StringComparer.Ordinal,
                cancellationToken);
    }

    private async Task<int> GetRetryOverdueCountAsync(DateTime nowUtc, CancellationToken cancellationToken)
    {
        return await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message =>
                message.Status == OutboxMessageStatus.Failed
                && message.NextAttemptAtUtc <= nowUtc)
            .CountAsync(cancellationToken);
    }

    private async Task<long> GetOldestAgeSecondsAsync(
        string status,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var createdAtUtc = await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message => message.Status == status)
            .Select(message => (DateTime?)message.CreatedAtUtc)
            .MinAsync(cancellationToken);

        return CalculateAgeSeconds(nowUtc, createdAtUtc);
    }

    private async Task<long> GetMaxRetryAgeSecondsAsync(DateTime nowUtc, CancellationToken cancellationToken)
    {
        var oldestFailedDueAt = await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message =>
                message.Status == OutboxMessageStatus.Failed
                && message.NextAttemptAtUtc <= nowUtc)
            .Select(message => (DateTime?)message.NextAttemptAtUtc)
            .MinAsync(cancellationToken);

        var oldestDeadLetterCreatedAt = await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message => message.Status == OutboxMessageStatus.DeadLetter)
            .Select(message => (DateTime?)message.CreatedAtUtc)
            .MinAsync(cancellationToken);

        var failedRetryAge = CalculateAgeSeconds(nowUtc, oldestFailedDueAt);
        var deadLetterAge = CalculateAgeSeconds(nowUtc, oldestDeadLetterCreatedAt);
        return Math.Max(failedRetryAge, deadLetterAge);
    }

    private async Task<IReadOnlyList<OutboxMessageSummaryDto>> GetTopFailedAsync(
        int top,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var messages = await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message => message.Status == OutboxMessageStatus.Failed)
            .OrderBy(message => message.NextAttemptAtUtc)
            .ThenByDescending(message => message.AttemptCount)
            .ThenBy(message => message.CreatedAtUtc)
            .Take(top)
            .ToListAsync(cancellationToken);

        return messages
            .Select(message => MapMessageSummary(message, nowUtc, isDeadLetter: false))
            .ToList();
    }

    private async Task<IReadOnlyList<OutboxMessageSummaryDto>> GetTopDeadLettersAsync(
        int top,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var messages = await _dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message => message.Status == OutboxMessageStatus.DeadLetter)
            .OrderBy(message => message.CreatedAtUtc)
            .ThenByDescending(message => message.AttemptCount)
            .Take(top)
            .ToListAsync(cancellationToken);

        return messages
            .Select(message => MapMessageSummary(message, nowUtc, isDeadLetter: true))
            .ToList();
    }

    private static OutboxMessageSummaryDto MapMessageSummary(
        OutboxMessage message,
        DateTime nowUtc,
        bool isDeadLetter)
    {
        return new OutboxMessageSummaryDto
        {
            MessageId = message.Id,
            EventType = message.EventType,
            AttemptCount = message.AttemptCount,
            NextAttemptAtUtc = message.NextAttemptAtUtc,
            CreatedAtUtc = message.CreatedAtUtc,
            RetryAgeSeconds = isDeadLetter
                ? CalculateAgeSeconds(nowUtc, message.CreatedAtUtc)
                : CalculateFailedRetryAgeSeconds(nowUtc, message.NextAttemptAtUtc),
            LastErrorPreview = Truncate(message.LastError, LastErrorPreviewMaxLength)
        };
    }

    private static int GetCount(IReadOnlyDictionary<string, int> statusCounts, string status)
    {
        return statusCounts.TryGetValue(status, out var value) ? value : 0;
    }

    private static long CalculateFailedRetryAgeSeconds(DateTime nowUtc, DateTime nextAttemptAtUtc)
    {
        return nextAttemptAtUtc >= nowUtc
            ? 0
            : (long)(nowUtc - nextAttemptAtUtc).TotalSeconds;
    }

    private static long CalculateAgeSeconds(DateTime nowUtc, DateTime? referenceUtc)
    {
        if (referenceUtc is null || referenceUtc >= nowUtc)
        {
            return 0;
        }

        return (long)(nowUtc - referenceUtc.Value).TotalSeconds;
    }

    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
