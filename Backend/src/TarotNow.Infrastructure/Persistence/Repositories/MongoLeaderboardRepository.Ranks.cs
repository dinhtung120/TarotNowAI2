using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý truy vấn thứ hạng cá nhân.
public partial class MongoLeaderboardRepository
{
    /// <summary>
    /// Lấy thứ hạng của user trong một track-period.
    /// Luồng xử lý: lấy entry của user, đếm số entry có score cao hơn rồi suy ra rank.
    /// </summary>
    public async Task<LeaderboardEntryDto?> GetUserRankAsync(Guid userId, string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.UserId, userId)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        var userEntry = await _context.LeaderboardEntries.Find(filter).FirstOrDefaultAsync(ct);
        if (userEntry == null) return null;
        // Edge case: user chưa có điểm trong kỳ thì không có rank.

        var higherScoresFilter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                               & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey)
                               & Builders<LeaderboardEntryDocument>.Filter.Gt(e => e.Score, userEntry.Score);
        // Rank tính theo số lượng score cao hơn + 1.

        var countHigher = await _context.LeaderboardEntries.CountDocumentsAsync(higherScoresFilter, cancellationToken: ct);
        return new LeaderboardEntryDto
        {
            UserId = userId.ToString(),
            Score = userEntry.Score,
            Rank = (int)(countHigher + 1)
        };
    }
}
