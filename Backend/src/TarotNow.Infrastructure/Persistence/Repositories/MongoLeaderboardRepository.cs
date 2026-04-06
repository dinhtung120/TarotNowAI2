using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class MongoLeaderboardRepository : ILeaderboardRepository
{
    private readonly MongoDbContext _context;

    public MongoLeaderboardRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<LeaderboardSnapshotDto?> GetSnapshotAsync(string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardSnapshotDocument>.Filter.Eq(s => s.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardSnapshotDocument>.Filter.Eq(s => s.PeriodKey, periodKey);

        var doc = await _context.LeaderboardSnapshots.Find(filter).FirstOrDefaultAsync(ct);
        if (doc == null) return null;

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

        await _context.LeaderboardSnapshots.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    public async Task IncrementScoreAsync(Guid userId, string scoreTrack, string periodKey, long points, CancellationToken ct)
    {
        /*
         * [FIX-GAMIFICATION]: Sử dụng Upsert (Update or Insert) cẩn thận.
         * Khi sử dụng IsUpsert = true, chúng ta cần dùng SetOnInsert để đảm bảo các trường lọc (Filter)
         * được lưu vào tài liệu mới nếu nó chưa tồn tại. Nếu không, tài liệu mới sẽ bị trống các trường này.
         */
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.UserId, userId)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        var update = Builders<LeaderboardEntryDocument>.Update
            .Inc(e => e.Score, points)
            .Set(e => e.UpdatedAt, DateTime.UtcNow)
            // Đảm bảo các trường định danh được thiết lập khi chèn mới (Upsert)
            .SetOnInsert(e => e.UserId, userId)
            .SetOnInsert(e => e.ScoreTrack, scoreTrack)
            .SetOnInsert(e => e.PeriodKey, periodKey);

        await _context.LeaderboardEntries.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    public async Task<List<LeaderboardEntryDto>> GetTopEntriesAsync(string scoreTrack, string periodKey, int limit, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        var docs = await _context.LeaderboardEntries
            .Find(filter)
            .SortByDescending(e => e.Score)
            .Limit(limit)
            .ToListAsync(ct);

        // Map to DTO and assign Rank
        var result = new List<LeaderboardEntryDto>();
        for (int i = 0; i < docs.Count; i++)
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

    public async Task<LeaderboardEntryDto?> GetUserRankAsync(Guid userId, string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.UserId, userId)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);

        var userEntry = await _context.LeaderboardEntries.Find(filter).FirstOrDefaultAsync(ct);
        if (userEntry == null) return null;

        // Calculate rank by counting how many people have a strictly higher score
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

    public async Task ResetScoreTrackAsync(string scoreTrack, string periodKey, CancellationToken ct)
    {
        var filter = Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.ScoreTrack, scoreTrack)
                   & Builders<LeaderboardEntryDocument>.Filter.Eq(e => e.PeriodKey, periodKey);
                   
        await _context.LeaderboardEntries.DeleteManyAsync(filter, ct);
    }
}
