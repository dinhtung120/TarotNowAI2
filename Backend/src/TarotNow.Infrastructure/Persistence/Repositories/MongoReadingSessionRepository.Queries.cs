using MongoDB.Driver;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoReadingSessionRepository
{
    public async Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var doc = await _mongoContext.ReadingSessions
            .Find(BuildIdFilter(id))
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : MapToEntity(doc);
    }

    public async Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var startOfDay = utcNow.Date;
        var endOfDay = startOfDay.AddDays(1);

        var count = await _mongoContext.ReadingSessions.CountDocumentsAsync(
            r => r.UserId == userId.ToString()
                && r.SpreadType == SpreadType.Daily1Card
                && r.CreatedAt >= startOfDay
                && r.CreatedAt < endOfDay,
            cancellationToken: cancellationToken);

        return count > 0;
    }

    public async Task<(ReadingSession ReadingSession, IEnumerable<AiRequest> AiRequests)?> GetSessionWithAiRequestsAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var session = await GetByIdAsync(sessionId, cancellationToken);
        if (session == null)
        {
            return null;
        }

        var aiRequests = _pgContext.AiRequests
            .Where(a => a.ReadingSessionRef == sessionId)
            .OrderBy(a => a.CreatedAt)
            .AsEnumerable();

        return (session, aiRequests);
    }
}
