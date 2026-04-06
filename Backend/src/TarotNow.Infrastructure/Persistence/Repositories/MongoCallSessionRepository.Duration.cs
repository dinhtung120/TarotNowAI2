using MongoDB.Driver;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoCallSessionRepository
{
    private async Task UpdateDurationWhenEndedAsync(
        string id,
        CallSessionStatus newStatus,
        DateTime? endedAt,
        DateTime? startedAt,
        CancellationToken ct)
    {
        if (newStatus != CallSessionStatus.Ended || !endedAt.HasValue || !startedAt.HasValue)
        {
            return;
        }

        var duration = Math.Max(0, (int)(endedAt.Value - startedAt.Value).TotalSeconds);
        var durationUpdate = Builders<CallSessionDocument>.Update.Set(x => x.DurationSeconds, duration);
        await _context.CallSessions.UpdateOneAsync(x => x.Id == id, durationUpdate, cancellationToken: ct);
    }
}
