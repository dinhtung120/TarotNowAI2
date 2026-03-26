using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReadingSessionRepository
{
    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetSessionsByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 200);

        var filter = Builders<ReadingSessionDocument>.Filter.Eq(r => r.UserId, userId.ToString())
            & Builders<ReadingSessionDocument>.Filter.Eq(r => r.IsDeleted, false);

        var totalCount = await _mongoContext.ReadingSessions.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await QueryPagedAsync(filter, normalizedPage, normalizedPageSize, cancellationToken);

        return (docs.Select(MapToEntity).ToList(), (int)totalCount);
    }

    public async Task<(IEnumerable<ReadingSession> Items, int TotalCount)> GetAllSessionsAsync(
        int page,
        int pageSize,
        List<string>? userIds = null,
        string? spreadType = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 200);
        var filter = BuildAdminFilter(userIds, spreadType, startDate, endDate);

        var totalCount = await _mongoContext.ReadingSessions.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await QueryPagedAsync(filter, normalizedPage, normalizedPageSize, cancellationToken);

        return (docs.Select(MapToEntity).ToList(), (int)totalCount);
    }

    private static FilterDefinition<ReadingSessionDocument> BuildAdminFilter(
        List<string>? userIds,
        string? spreadType,
        DateTime? startDate,
        DateTime? endDate)
    {
        var builder = Builders<ReadingSessionDocument>.Filter;
        var filter = builder.Eq(r => r.IsDeleted, false);

        if (userIds != null && userIds.Any())
        {
            filter &= builder.In(r => r.UserId, userIds);
        }

        if (!string.IsNullOrWhiteSpace(spreadType))
        {
            filter &= builder.Eq(r => r.SpreadType, spreadType);
        }

        if (startDate.HasValue)
        {
            filter &= builder.Gte(r => r.CreatedAt, startDate.Value);
        }

        if (endDate.HasValue)
        {
            filter &= builder.Lte(r => r.CreatedAt, endDate.Value);
        }

        return filter;
    }

    private Task<List<ReadingSessionDocument>> QueryPagedAsync(
        FilterDefinition<ReadingSessionDocument> filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        return _mongoContext.ReadingSessions
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }
}
