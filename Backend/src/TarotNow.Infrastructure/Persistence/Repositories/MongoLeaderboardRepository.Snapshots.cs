using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý snapshot leaderboard.
public partial class MongoLeaderboardRepository
{
    /// <summary>
    /// Lấy snapshot leaderboard theo track-period.
    /// Luồng xử lý: query document snapshot và map về DTO đầy đủ danh sách entries.
    /// </summary>
    public async Task<LeaderboardSnapshotDto?> GetSnapshotAsync(string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardSnapshotDocument>.Filter.Eq(s => s.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardSnapshotDocument>.Filter.Eq(s => s.PeriodKey, periodKey);

        var doc = await _context.LeaderboardSnapshots.Find(filter).FirstOrDefaultAsync(ct);
        if (doc == null) return null;
        // Không có snapshot nghĩa là kỳ này chưa được chốt.

        return new LeaderboardSnapshotDto
        {
            ScoreTrack = doc.ScoreTrack,
            PeriodKey = doc.PeriodKey,
            TotalParticipants = doc.TotalParticipants,
            CreatedAt = doc.CreatedAt,
            Entries = doc.Entries.Select(e => new LeaderboardEntryDto
            {
                UserId = e.UserId.ToString(),
                DisplayName = e.DisplayName,
                Avatar = e.Avatar,
                ActiveTitle = e.ActiveTitle,
                Score = e.Score,
                Rank = e.Rank
            }).ToList()
        };
    }

    /// <summary>
    /// Upsert snapshot leaderboard.
    /// Luồng xử lý: dựng document từ DTO, update participants/entries, chỉ set CreatedAt khi insert mới.
    /// </summary>
    public async Task UpsertSnapshotAsync(LeaderboardSnapshotDto snapshot, CancellationToken ct)
    {
        var filter = Builders<LeaderboardSnapshotDocument>.Filter.Eq(s => s.ScoreTrack, snapshot.ScoreTrack)
                   & Builders<LeaderboardSnapshotDocument>.Filter.Eq(s => s.PeriodKey, snapshot.PeriodKey);

        var document = new LeaderboardSnapshotDocument
        {
            ScoreTrack = snapshot.ScoreTrack,
            PeriodKey = snapshot.PeriodKey,
            TotalParticipants = snapshot.TotalParticipants,
            CreatedAt = DateTime.UtcNow,
            Entries = snapshot.Entries.Select(e => new LeaderboardSnapshotEntry
            {
                UserId = Guid.Parse(e.UserId),
                DisplayName = e.DisplayName,
                Avatar = e.Avatar,
                ActiveTitle = e.ActiveTitle,
                Score = e.Score,
                Rank = e.Rank
            }).ToList()
        };

        var update = Builders<LeaderboardSnapshotDocument>.Update
            .Set(s => s.TotalParticipants, document.TotalParticipants)
            .Set(s => s.Entries, document.Entries)
            .SetOnInsert(s => s.CreatedAt, document.CreatedAt);
        // Giữ CreatedAt ổn định sau lần insert đầu để bảo toàn thời điểm chốt snapshot.

        await _context.LeaderboardSnapshots.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }
}
