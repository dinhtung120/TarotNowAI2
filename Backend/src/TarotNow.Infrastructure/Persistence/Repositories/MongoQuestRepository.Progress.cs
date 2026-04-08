using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý tiến độ quest của người dùng.
public partial class MongoQuestRepository
{
    /// <summary>
    /// Lấy progress của một quest cụ thể.
    /// Luồng xử lý: filter theo userId-questCode-periodKey và map DTO nếu có.
    /// </summary>
    public async Task<QuestProgressDto?> GetProgressAsync(Guid userId, string questCode, string periodKey, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.QuestCode, questCode)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);

        var doc = await _context.QuestProgress.Find(filter).FirstOrDefaultAsync(ct);
        return doc != null ? MapProgress(doc) : null;
    }

    /// <summary>
    /// Lấy toàn bộ progress của user trong một period.
    /// Luồng xử lý: lọc theo user và periodKey, trả danh sách progress đã map DTO.
    /// </summary>
    public async Task<List<QuestProgressDto>> GetAllProgressAsync(Guid userId, string questType, string periodKey, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                   & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);
        // questType hiện không lọc ở tầng này vì progress chỉ lưu quest_code + period; mapping questType xử lý ở layer cao hơn.

        var docs = await _context.QuestProgress.Find(filter).ToListAsync(ct);
        return docs.Select(MapProgress).ToList();
    }

    /// <summary>
    /// Upsert và tăng tiến độ quest.
    /// Luồng xử lý: increment CurrentProgress, set defaults khi insert mới và cập nhật timestamp.
    /// </summary>
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
        // Dùng Inc để bảo đảm cộng dồn tiến độ an toàn khi có nhiều sự kiện đồng thời.

        await _context.QuestProgress.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, ct);
    }

    /// <summary>
    /// Đánh dấu quest progress đã claim.
    /// Luồng xử lý: set cờ IsClaimed, thời điểm ClaimedAt và UpdatedAt.
    /// </summary>
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
