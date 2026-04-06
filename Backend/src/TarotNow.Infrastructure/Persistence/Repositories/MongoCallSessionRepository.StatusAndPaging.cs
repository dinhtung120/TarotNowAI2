using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoCallSessionRepository
{
    public async Task<bool> UpdateStatusAsync(
        string id,
        CallSessionStatus newStatus,
        DateTime? startedAt = null,
        DateTime? endedAt = null,
        string? endReason = null,
        CallSessionStatus? expectedPreviousStatus = null,
        CancellationToken ct = default)
    {
        var filter = BuildUpdateStatusFilter(id, expectedPreviousStatus);
        var update = BuildUpdateStatusDefinition(newStatus, startedAt, endedAt, endReason);
        var oldDoc = await FindAndUpdateCallSessionAsync(filter, update, ct);

        if (oldDoc == null)
        {
            return false;
        }

        await UpdateDurationWhenEndedAsync(id, newStatus, endedAt, oldDoc.StartedAt, ct);
        return true;
    }

    public async Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.ConversationId, conversationId);
        var totalTask = _context.CallSessions.CountDocumentsAsync(filter, cancellationToken: ct);
        var itemsTask = _context.CallSessions
            .Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        await Task.WhenAll(totalTask, itemsTask);
        return (itemsTask.Result.Select(CallSessionDocumentMapper.ToDto).ToList(), totalTask.Result);
    }

    private static FilterDefinition<CallSessionDocument> BuildUpdateStatusFilter(string id, CallSessionStatus? expectedPreviousStatus)
    {
        var filterBuilder = Builders<CallSessionDocument>.Filter;
        var filter = filterBuilder.Eq(x => x.Id, id);

        if (expectedPreviousStatus.HasValue)
        {
            var expectedStatus = CallSessionDocumentMapper.MapStatus(expectedPreviousStatus.Value);
            filter &= filterBuilder.Eq(x => x.Status, expectedStatus);
        }

        return filter;
    }

    private static UpdateDefinition<CallSessionDocument> BuildUpdateStatusDefinition(
        CallSessionStatus newStatus,
        DateTime? startedAt,
        DateTime? endedAt,
        string? endReason)
    {
        var update = Builders<CallSessionDocument>.Update
            .Set(x => x.Status, CallSessionDocumentMapper.MapStatus(newStatus))
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        if (startedAt.HasValue)
        {
            update = update.Set(x => x.StartedAt, startedAt.Value);
        }

        if (endedAt.HasValue)
        {
            update = update.Set(x => x.EndedAt, endedAt.Value);
        }

        if (!string.IsNullOrWhiteSpace(endReason))
        {
            update = update.Set(x => x.EndReason, endReason);
        }

        return update;
    }

    private async Task<CallSessionDocument?> FindAndUpdateCallSessionAsync(
        FilterDefinition<CallSessionDocument> filter,
        UpdateDefinition<CallSessionDocument> update,
        CancellationToken ct)
    {
        return await _context.CallSessions.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<CallSessionDocument> { ReturnDocument = ReturnDocument.Before },
            cancellationToken: ct);
    }
}
