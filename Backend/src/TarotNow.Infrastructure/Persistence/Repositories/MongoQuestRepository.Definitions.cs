using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý quest definitions.
public partial class MongoQuestRepository
{
    /// <summary>
    /// Lấy danh sách quest active theo loại.
    /// Luồng xử lý: filter quest_type + is_active=true rồi map DTO.
    /// </summary>
    public async Task<List<QuestDefinitionDto>> GetActiveQuestsAsync(string questType, CancellationToken ct)
    {
        var filter = Builders<QuestDefinitionDocument>.Filter.Eq(q => q.QuestType, questType)
                     & Builders<QuestDefinitionDocument>.Filter.Eq(q => q.IsActive, true);
        var docs = await _context.Quests.Find(filter).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    /// <summary>
    /// Lấy quest definition theo code.
    /// Luồng xử lý: query code duy nhất và map DTO nếu tồn tại.
    /// </summary>
    public async Task<QuestDefinitionDto?> GetQuestByCodeAsync(string questCode, CancellationToken ct)
    {
        var doc = await _context.Quests.Find(q => q.Code == questCode).FirstOrDefaultAsync(ct);
        return doc != null ? MapDefinition(doc) : null;
    }

    /// <summary>
    /// Lấy toàn bộ quest definitions.
    /// Luồng xử lý: đọc tất cả documents và map sang DTO.
    /// </summary>
    public async Task<List<QuestDefinitionDto>> GetAllQuestsAsync(CancellationToken ct)
    {
        var docs = await _context.Quests.Find(_ => true).ToListAsync(ct);
        return docs.Select(MapDefinition).ToList();
    }

    /// <summary>
    /// Upsert quest definition theo code.
    /// Luồng xử lý: cập nhật metadata quest/rewards và chỉ set CreatedAt ở lần insert đầu.
    /// </summary>
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
        // SetOnInsert giữ lịch sử thời điểm khởi tạo definition ổn định qua các lần chỉnh sửa.

        await _context.Quests.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    /// <summary>
    /// Xóa quest definition theo code.
    /// Luồng xử lý: delete một document khớp code.
    /// </summary>
    public async Task DeleteQuestDefinitionAsync(string questCode, CancellationToken ct)
    {
        await _context.Quests.DeleteOneAsync(q => q.Code == questCode, ct);
    }
}
