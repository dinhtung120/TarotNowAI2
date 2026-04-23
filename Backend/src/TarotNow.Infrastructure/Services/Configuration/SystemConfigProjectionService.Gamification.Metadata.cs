using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigProjectionService
{
    private async Task ProjectAchievementDefinitionsAsync(
        IReadOnlyDictionary<string, SnapshotItem> configs,
        CancellationToken cancellationToken)
    {
        if (!configs.TryGetValue("gamification.achievements", out var item))
        {
            return;
        }

        var definitions = DeserializeList<AchievementDefinitionConfig>(item.Value, "gamification.achievements");
        if (definitions.Count == 0)
        {
            return;
        }

        var nowUtc = DateTime.UtcNow;
        var configuredCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var achievement in definitions)
        {
            var code = NormalizeRequired(achievement.Code, nameof(achievement.Code));
            configuredCodes.Add(code);
            await UpsertAchievementDefinitionAsync(code, achievement, nowUtc, cancellationToken);
        }

        await DeactivateMissingAchievementsAsync(configuredCodes, cancellationToken);
    }

    private async Task UpsertAchievementDefinitionAsync(
        string code,
        AchievementDefinitionConfig achievement,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var filter = Builders<AchievementDefinitionDocument>.Filter.Eq(x => x.Code, code);
        var update = Builders<AchievementDefinitionDocument>.Update
            .Set(x => x.TitleVi, achievement.TitleVi?.Trim() ?? string.Empty)
            .Set(x => x.TitleEn, achievement.TitleEn?.Trim() ?? string.Empty)
            .Set(x => x.TitleZh, achievement.TitleZh?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionVi, achievement.DescriptionVi?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionEn, achievement.DescriptionEn?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionZh, achievement.DescriptionZh?.Trim() ?? string.Empty)
            .Set(x => x.Icon, string.IsNullOrWhiteSpace(achievement.Icon) ? null : achievement.Icon.Trim())
            .Set(x => x.GrantsTitleCode, string.IsNullOrWhiteSpace(achievement.GrantsTitleCode) ? null : achievement.GrantsTitleCode.Trim())
            .Set(x => x.IsActive, achievement.IsActive)
            .SetOnInsert(x => x.CreatedAt, nowUtc);

        await _mongoDbContext.Achievements.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken);
    }

    private async Task DeactivateMissingAchievementsAsync(
        HashSet<string> configuredCodes,
        CancellationToken cancellationToken)
    {
        var deactivateFilter = Builders<AchievementDefinitionDocument>.Filter.And(
            Builders<AchievementDefinitionDocument>.Filter.Nin(x => x.Code, configuredCodes),
            Builders<AchievementDefinitionDocument>.Filter.Eq(x => x.IsActive, true));
        var deactivateUpdate = Builders<AchievementDefinitionDocument>.Update
            .Set(x => x.IsActive, false);

        await _mongoDbContext.Achievements.UpdateManyAsync(
            deactivateFilter,
            deactivateUpdate,
            cancellationToken: cancellationToken);
    }

    private async Task ProjectTitleDefinitionsAsync(
        IReadOnlyDictionary<string, SnapshotItem> configs,
        CancellationToken cancellationToken)
    {
        if (!configs.TryGetValue("gamification.titles", out var item))
        {
            return;
        }

        var definitions = DeserializeList<TitleDefinitionConfig>(item.Value, "gamification.titles");
        if (definitions.Count == 0)
        {
            return;
        }

        var nowUtc = DateTime.UtcNow;
        var configuredCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var title in definitions)
        {
            var code = NormalizeRequired(title.Code, nameof(title.Code));
            configuredCodes.Add(code);
            await UpsertTitleDefinitionAsync(code, title, nowUtc, cancellationToken);
        }

        await DeactivateMissingTitlesAsync(configuredCodes, cancellationToken);
    }

    private async Task UpsertTitleDefinitionAsync(
        string code,
        TitleDefinitionConfig title,
        DateTime nowUtc,
        CancellationToken cancellationToken)
    {
        var filter = Builders<TitleDefinitionDocument>.Filter.Eq(x => x.Code, code);
        var update = Builders<TitleDefinitionDocument>.Update
            .Set(x => x.NameVi, title.NameVi?.Trim() ?? string.Empty)
            .Set(x => x.NameEn, title.NameEn?.Trim() ?? string.Empty)
            .Set(x => x.NameZh, title.NameZh?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionVi, title.DescriptionVi?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionEn, title.DescriptionEn?.Trim() ?? string.Empty)
            .Set(x => x.DescriptionZh, title.DescriptionZh?.Trim() ?? string.Empty)
            .Set(x => x.Rarity, string.IsNullOrWhiteSpace(title.Rarity) ? "Common" : title.Rarity.Trim())
            .Set(x => x.IsActive, title.IsActive)
            .SetOnInsert(x => x.CreatedAt, nowUtc);

        await _mongoDbContext.Titles.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken);
    }

    private async Task DeactivateMissingTitlesAsync(
        HashSet<string> configuredCodes,
        CancellationToken cancellationToken)
    {
        var deactivateFilter = Builders<TitleDefinitionDocument>.Filter.And(
            Builders<TitleDefinitionDocument>.Filter.Nin(x => x.Code, configuredCodes),
            Builders<TitleDefinitionDocument>.Filter.Eq(x => x.IsActive, true));
        var deactivateUpdate = Builders<TitleDefinitionDocument>.Update
            .Set(x => x.IsActive, false);

        await _mongoDbContext.Titles.UpdateManyAsync(
            deactivateFilter,
            deactivateUpdate,
            cancellationToken: cancellationToken);
    }
}
