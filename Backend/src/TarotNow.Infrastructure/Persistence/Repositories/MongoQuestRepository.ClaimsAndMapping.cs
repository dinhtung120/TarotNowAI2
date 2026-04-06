using MongoDB.Driver;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoQuestRepository
{
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
