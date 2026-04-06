using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoLeaderboardRepository
{
    public async Task IncrementScoreAsync(Guid userId, string scoreTrack, string periodKey, long points, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.UserId, userId)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        var update = Builders<LeaderboardEntryDocument>.Update
            .Inc(e => e.Score, points)
            .Set(e => e.UpdatedAt, DateTime.UtcNow)
            .SetOnInsert(e => e.UserId, userId)
            .SetOnInsert(e => e.ScoreTrack, scoreTrack)
            .SetOnInsert(e => e.PeriodKey, periodKey);

        await _context.LeaderboardEntries.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    public async Task<List<LeaderboardEntryDto>> GetTopEntriesAsync(string scoreTrack, string periodKey, int limit, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        var docs = await _context.LeaderboardEntries.Find(filter).SortByDescending(e => e.Score).Limit(limit).ToListAsync(ct);
        var result = new List<LeaderboardEntryDto>(docs.Count);
        for (var i = 0; i < docs.Count; i++)
        {
            result.Add(new LeaderboardEntryDto
            {
                UserId = docs[i].UserId.ToString(),
                Score = docs[i].Score,
                Rank = i + 1
            });
        }

        return result;
    }

    public async Task ResetScoreTrackAsync(string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        await _context.LeaderboardEntries.DeleteManyAsync(filter, ct);
    }
}
