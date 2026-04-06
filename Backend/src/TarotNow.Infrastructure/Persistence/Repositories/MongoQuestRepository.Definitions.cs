using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoQuestRepository
{
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
}
