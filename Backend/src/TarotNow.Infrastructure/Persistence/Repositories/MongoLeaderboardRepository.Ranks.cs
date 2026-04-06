using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoLeaderboardRepository
{
    public async Task<LeaderboardEntryDto?> GetUserRankAsync(Guid userId, string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.UserId, userId)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        var userEntry = await _context.LeaderboardEntries.Find(filter).FirstOrDefaultAsync(ct);
        if (userEntry == null) return null;

        var higherScoresFilter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                               & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey)
                               & Builders<LeaderboardEntryDocument>.Filter.Gt(e => e.Score, userEntry.Score);

        var countHigher = await _context.LeaderboardEntries.CountDocumentsAsync(higherScoresFilter, cancellationToken: ct);
        return new LeaderboardEntryDto
        {
            UserId = userId.ToString(),
            Score = userEntry.Score,
            Rank = (int)(countHigher + 1)
        };
    }
}
