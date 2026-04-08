using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý thao tác điểm leaderboard.
public partial class MongoLeaderboardRepository
{
    /// <summary>
    /// Cộng điểm cho user theo track và period.
    /// Luồng xử lý: upsert bản ghi leaderboard entry, tăng score atomically và cập nhật updated_at.
    /// </summary>
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
        // SetOnInsert bảo toàn khóa nghiệp vụ khi entry được tạo lần đầu.

        await _context.LeaderboardEntries.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    /// <summary>
    /// Lấy top entries theo track và period.
    /// Luồng xử lý: sort score giảm dần, giới hạn số lượng và gán rank tuần tự theo thứ tự trả về.
    /// </summary>
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
        // Rank hiện tính theo vị trí list, phù hợp UI top-N realtime.

        return result;
    }

    /// <summary>
    /// Xóa toàn bộ điểm của một track-period.
    /// Luồng xử lý: delete many theo scoreTrack + periodKey để reset kỳ xếp hạng.
    /// </summary>
    public async Task ResetScoreTrackAsync(string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        await _context.LeaderboardEntries.DeleteManyAsync(filter, ct);
    }
}
