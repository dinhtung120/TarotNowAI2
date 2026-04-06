using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoQuestRepository
{
    public async Task<QuestProgressDto?> GetProgressAsync(Guid userId, string questCode, string periodKey, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.QuestCode, questCode)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);

        var doc = await _context.QuestProgress.Find(filter).FirstOrDefaultAsync(ct);
        return doc != null ? MapProgress(doc) : null;
    }

    public async Task<List<QuestProgressDto>> GetAllProgressAsync(Guid userId, string questType, string periodKey, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                   & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);
        var docs = await _context.QuestProgress.Find(filter).ToListAsync(ct);
        return docs.Select(MapProgress).ToList();
    }

    public async Task UpsertProgressAsync(QuestProgressUpsertRequest request, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, request.UserId)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.QuestCode, request.QuestCode)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, request.PeriodKey);

        var update = Builders<QuestProgressDocument>.Update
            .Inc(p => p.CurrentProgress, request.IncrementBy)
            .SetOnInsert(p => p.Target, request.Target)
            .SetOnInsert(p => p.IsClaimed, false)
            .Set(p => p.UpdatedAt, DateTime.UtcNow)
            .SetOnInsert(p => p.CreatedAt, DateTime.UtcNow);

        await _context.QuestProgress.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    public async Task MarkClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.QuestCode, questCode)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);

        var update = Builders<QuestProgressDocument>.Update
            .Set(p => p.IsClaimed, true)
            .Set(p => p.ClaimedAt, DateTime.UtcNow)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        await _context.QuestProgress.UpdateOneAsync(filter, update, cancellationToken: ct);
    }
}
