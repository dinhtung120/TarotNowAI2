using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigProjectionService
{
    private async Task ProjectGamificationAsync(
        IReadOnlyDictionary<string, SnapshotItem> configs,
        CancellationToken cancellationToken)
    {
        await ProjectQuestDefinitionsAsync(configs, cancellationToken);
        await ProjectAchievementDefinitionsAsync(configs, cancellationToken);
        await ProjectTitleDefinitionsAsync(configs, cancellationToken);
    }

    private async Task ProjectQuestDefinitionsAsync(
        IReadOnlyDictionary<string, SnapshotItem> configs,
        CancellationToken cancellationToken)
    {
        if (!configs.TryGetValue("gamification.quests", out var item))
        {
            return;
        }

        var definitions = DeserializeList<QuestDefinitionConfig>(item.Value, "gamification.quests");
        if (definitions.Count == 0)
        {
            return;
        }

        var nowUtc = DateTime.UtcNow;
        var configuredCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var quest in definitions)
        {
            var code = NormalizeRequired(quest.Code, nameof(quest.Code));
            configuredCodes.Add(code);
            await UpsertQuestDefinitionAsync(code, quest, nowUtc, cancellationToken);
        }

        await DeactivateMissingQuestsAsync(configuredCodes, nowUtc, cancellationToken);
    }

    private async Task UpsertQuestDefinitionAsync(
        string code,
        QuestDefinitionConfig quest,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var filter = Builders<QuestDefinitionDocument>.Filter.Eq(x => x.Code, code);
        var update = BuildQuestDefinitionUpdate(quest, nowUtc);

        await _mongoDbContext.Quests.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken);
    }

    private static UpdateDefinition<QuestDefinitionDocument> BuildQuestDefinitionUpdate(
        QuestDefinitionConfig quest,
        DateTime nowUtc)
    {
        return Builders<QuestDefinitionDocument>.Update
            .Set(x => x.TitleVi, quest.TitleVi?.Trim() ?? string.Empty)
            .Set(x => x.TitleEn, quest.TitleEn?.Trim() ?? string.Empty)
            .Set(x => x.TitleZh, quest.TitleZh?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionVi, quest.DescriptionVi?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionEn, quest.DescriptionEn?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionZh, quest.DescriptionZh?.Trim() ?? string.Empty)
            .Set(x => x.QuestType, quest.QuestType?.Trim().ToLowerInvariant() ?? "daily")
            .Set(x => x.TriggerEvent, quest.TriggerEvent?.Trim() ?? string.Empty)
            .Set(x => x.Target, quest.Target)
            .Set(x => x.Rewards, BuildQuestRewards(quest.Rewards))
            .Set(x => x.IsActive, quest.IsActive)
            .Set(x => x.UpdatedAt, nowUtc)
            .SetOnInsert(x => x.CreatedAt, nowUtc);
    }

    private static List<QuestRewardItem> BuildQuestRewards(IEnumerable<QuestRewardConfig>? rewards)
    {
        return (rewards ?? [])
            .Select(r => new QuestRewardItem
            {
                Type = r.Type?.Trim() ?? string.Empty,
                Amount = r.Amount,
                TitleCode = string.IsNullOrWhiteSpace(r.TitleCode) ? null : r.TitleCode.Trim()
            })
            .ToList();
    }

    private async Task DeactivateMissingQuestsAsync(
        HashSet<string> configuredCodes,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var deactivateFilter = Builders<QuestDefinitionDocument>.Filter.And(
            Builders<QuestDefinitionDocument>.Filter.Nin(x => x.Code, configuredCodes),
            Builders<QuestDefinitionDocument>.Filter.Eq(x => x.IsActive, true));
        var deactivateUpdate = Builders<QuestDefinitionDocument>.Update
            .Set(x => x.IsActive, false)
            .Set(x => x.UpdatedAt, nowUtc);

        await _mongoDbContext.Quests.UpdateManyAsync(
            deactivateFilter,
            deactivateUpdate,
            cancellationToken: cancellationToken);
    }
}
