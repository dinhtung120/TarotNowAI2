using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class CallSessionV2Repository
{
    public async Task<CallSessionV2Dto?> TryPatchAsync(string id, CallSessionV2Patch patch, CancellationToken ct = default)
    {
        var filter = BuildPatchFilter(id, patch.ExpectedPreviousStatuses);
        var update = BuildPatchUpdate(patch);

        var options = new FindOneAndUpdateOptions<CallSessionV2Document>
        {
            ReturnDocument = ReturnDocument.After,
        };

        var updated = await _context.CallSessionsV2.FindOneAndUpdateAsync(filter, update, options, ct);
        return updated == null ? null : ToDto(updated);
    }

    private static FilterDefinition<CallSessionV2Document> BuildPatchFilter(
        string id,
        IReadOnlyCollection<string>? expectedStatuses)
    {
        var filter = Builders<CallSessionV2Document>.Filter.Eq(x => x.Id, id);
        if (expectedStatuses == null || expectedStatuses.Count == 0)
        {
            return filter;
        }

        return filter & Builders<CallSessionV2Document>.Filter.In(x => x.Status, expectedStatuses);
    }

    private static UpdateDefinition<CallSessionV2Document> BuildPatchUpdate(CallSessionV2Patch patch)
    {
        var update = Builders<CallSessionV2Document>.Update
            .Set(x => x.Status, patch.NewStatus)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        update = ApplyOptionalUpdates(update, patch);
        return update;
    }

    private static UpdateDefinition<CallSessionV2Document> ApplyOptionalUpdates(
        UpdateDefinition<CallSessionV2Document> update,
        CallSessionV2Patch patch)
    {
        if (patch.AcceptedAt.HasValue) update = update.Set(x => x.AcceptedAt, patch.AcceptedAt.Value);
        if (patch.ConnectedAt.HasValue) update = update.Set(x => x.ConnectedAt, patch.ConnectedAt.Value);
        if (patch.EndedAt.HasValue) update = update.Set(x => x.EndedAt, patch.EndedAt.Value);
        if (patch.InitiatorJoinedAt.HasValue) update = update.Set(x => x.InitiatorJoinedAt, patch.InitiatorJoinedAt.Value);
        if (patch.CalleeJoinedAt.HasValue) update = update.Set(x => x.CalleeJoinedAt, patch.CalleeJoinedAt.Value);
        if (patch.IsLogCreated.HasValue) update = update.Set(x => x.IsLogCreated, patch.IsLogCreated.Value);

        return string.IsNullOrWhiteSpace(patch.EndReason)
            ? update
            : update.Set(x => x.EndReason, patch.EndReason);
    }
}
