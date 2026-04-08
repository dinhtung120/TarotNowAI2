using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý claim quest và mapping DTO/document.
public partial class MongoQuestRepository
{
    /// <summary>
    /// Đánh dấu quest đã claim theo cơ chế an toàn một lần.
    /// Luồng xử lý: chỉ update khi IsClaimed=false, set claimed timestamps và trả true nếu cập nhật thành công.
    /// </summary>
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
        // ModifiedCount=0 nghĩa là quest đã được claim trước đó hoặc không tồn tại record.
    }

    /// <summary>
    /// Hoàn tác trạng thái đã claim của quest progress.
    /// Luồng xử lý: reset IsClaimed/ClaimedAt và cập nhật UpdatedAt phục vụ rollback nghiệp vụ.
    /// </summary>
    public async Task RevertClaimedAsync(Guid userId, string questCode, string periodKey, CancellationToken ct)
    {
        var filter = Builders<QuestProgressDocument>.Filter.Eq(p => p.UserId, userId)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.QuestCode, questCode)
                     & Builders<QuestProgressDocument>.Filter.Eq(p => p.PeriodKey, periodKey);

        var update = Builders<QuestProgressDocument>.Update
            .Set(p => p.IsClaimed, false)
            .Set(p => p.ClaimedAt, null)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        await _context.QuestProgress.UpdateOneAsync(filter, update, cancellationToken: ct);
    }

    /// <summary>
    /// Map quest definition document sang DTO.
    /// Luồng xử lý: ánh xạ đầy đủ metadata quest và danh sách rewards.
    /// </summary>
    private static QuestDefinitionDto MapDefinition(QuestDefinitionDocument doc)
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

    /// <summary>
    /// Map quest progress document sang DTO.
    /// Luồng xử lý: chuyển định danh user về string và giữ trạng thái claim hiện tại.
    /// </summary>
    private static QuestProgressDto MapProgress(QuestProgressDocument doc)
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
