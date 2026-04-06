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

public class MongoQuestRepository : IQuestRepository
{
    private readonly MongoDbContext _context;

    public MongoQuestRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuestDefinitionDto>> GetActiveQuestsAsync(string questType, CancellationToken ct)
    {
        var filter = Builders<QuestDefinitionDocument>.Filter.Eq(q => q.QuestType, questType)
                     & Builders<QuestDefinitionDocument>.Filter.Eq(q => q.IsActive, true);

        var docs = await _context.Quests.Find(filter).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    public async Task<QuestDefinitionDto?> GetQuestByCodeAsync(string questCode, CancellationToken ct)
    {
        var doc = await _context.Quests.Find(q => q.Code == questCode).FirstOrDefaultAsync(ct);
        return doc != null ? MapDefinition(doc) : null;
    }

    public async Task<List<QuestDefinitionDto>> GetAllQuestsAsync(CancellationToken ct)
    {
        var docs = await _context.Quests.Find(_ => true).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    public async Task UpsertQuestDefinitionAsync(QuestDefinitionDto quest, CancellationToken ct)
    {
        var filter = Builders<QuestDefinitionDocument>.Filter.Eq(q => q.Code, quest.Code);
        var update = Builders<QuestDefinitionDocument>.Update
            .Set(q => q.TitleVi, quest.TitleVi)
            .Set(q => q.TitleEn, quest.TitleEn)
            .Set(q => q.TitleZh, quest.TitleZh)
            .Set(q => q.DescriptionVi, quest.DescriptionVi)
            .Set(q => q.DescriptionEn, quest.DescriptionEn)
            .Set(q => q.DescriptionZh, quest.DescriptionZh)
            .Set(q => q.QuestType, quest.QuestType)
            .Set(q => q.TriggerEvent, quest.TriggerEvent)
            .Set(q => q.Target, quest.Target)
            .Set(q => q.Rewards, quest.Rewards.Select(r => new QuestRewardItem { Type = r.Type, Amount = r.Amount, TitleCode = r.TitleCode }).ToList())
            .Set(q => q.IsActive, quest.IsActive)
            .Set(q => q.UpdatedAt, DateTime.UtcNow)
            .SetOnInsert(q => q.CreatedAt, DateTime.UtcNow);

        await _context.Quests.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    public async Task DeleteQuestDefinitionAsync(string questCode, CancellationToken ct)
    {
        await _context.Quests.DeleteOneAsync(q => q.Code == questCode, ct);
    }

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
        // To query cross collection, we usually just fetch and match in service or double query, since MongoDB is not relational.
        // But the input gives periodKey directly (which implicitly means type), so we just query by UserId and PeriodKey
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                   & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);

        var docs = await _context.QuestProgress.Find(filter).ToListAsync(ct);
        return docs.Select(MapProgress).ToList();
    }

    public async Task UpsertProgressAsync(Guid userId, string questCode, string periodKey, int target, int incrementBy, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.QuestCode, questCode)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);

        var update = Builders<QuestProgressDocument>.Update
            .Inc(p => p.CurrentProgress, incrementBy)
            .SetOnInsert(p => p.Target, target)
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

    public async Task<bool> TryMarkClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.QuestCode, questCode)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.IsClaimed, false);

        var update = Builders<QuestProgressDocument>.Update
            .Set(p => p.IsClaimed, true)
            .Set(p => p.ClaimedAt, DateTime.UtcNow)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        var result = await _context.QuestProgress.UpdateOneAsync(filter, update, cancellationToken: ct);
        return result.ModifiedCount > 0;
    }

    private QuestDefinitionDto MapDefinition(QuestDefinitionDocument doc)
    {
        return new QuestDefinitionDto
        {
            Code = doc.Code,
            TitleVi = doc.TitleVi,
            TitleEn = doc.TitleEn,
            TitleZh = doc.TitleZh,
            DescriptionVi = doc.DescriptionVi,
            DescriptionEn = doc.DescriptionEn,
            DescriptionZh = doc.DescriptionZh,
            QuestType = doc.QuestType,
            TriggerEvent = doc.TriggerEvent,
            Target = doc.Target,
            IsActive = doc.IsActive,
            Rewards = doc.Rewards.Select(r => new QuestRewardItemDto { Type = r.Type, Amount = r.Amount, TitleCode = r.TitleCode }).ToList()
        };
    }

    private QuestProgressDto MapProgress(QuestProgressDocument doc)
    {
        return new QuestProgressDto
        {
            UserId = doc.UserId.ToString(),
            QuestCode = doc.QuestCode,
            PeriodKey = doc.PeriodKey,
            CurrentProgress = doc.CurrentProgress,
            Target = doc.Target,
            IsClaimed = doc.IsClaimed,
            ClaimedAt = doc.ClaimedAt
        };
    }
}
