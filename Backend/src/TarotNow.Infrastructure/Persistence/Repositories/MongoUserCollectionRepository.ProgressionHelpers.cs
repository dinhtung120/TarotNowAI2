using System.Security.Cryptography;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial helper xử lý progression EXP/level và snapshot stats.
/// </summary>
public partial class MongoUserCollectionRepository
{
    /// <summary>
    /// Đồng bộ dữ liệu cũ để phù hợp model mới base/bonus.
    /// </summary>
    private static void NormalizeAndHydrateLegacyFields(UserCollectionDocument document)
    {
        document.Level = Math.Clamp(document.Level, 1, UserCollection.MaxLevel);
        document.Exp = Round2(Math.Max(document.Exp, 0m));
        document.ExpToNextLevel = ResolveExpToNextLevel(document.Level, document.ExpToNextLevel);
        document.BaseAtk = ResolveBaseStat(document.BaseAtk, document.Atk, UserCollection.DefaultBaseAtk);
        document.BaseDef = ResolveBaseStat(document.BaseDef, document.Def, UserCollection.DefaultBaseDef);
        document.BonusAtkPercent = Round2(Math.Max(document.BonusAtkPercent, 0m));
        document.BonusDefPercent = Round2(Math.Max(document.BonusDefPercent, 0m));
        document.Stats ??= new DrawStats();
        document.StatHistory ??= new List<StatRollRecord>();
        document.IsDeleted = false;
        RecalculateTotalStats(document);
        
        // [Self-Healing] Kiểm tra và tự động nâng cấp nếu đã tích đủ EXP nhưng chưa được xử lý.
        while (CanLevelUp(document))
        {
            LevelUpDocument(document, DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Clone document để mutate an toàn trước khi replace.
    /// </summary>
    private static UserCollectionDocument CloneDocument(UserCollectionDocument document)
    {
        return new UserCollectionDocument
        {
            Id = document.Id,
            UserId = document.UserId,
            CardId = document.CardId,
            Level = document.Level,
            Exp = document.Exp,
            ExpToNextLevel = document.ExpToNextLevel,
            AscensionTier = document.AscensionTier,
            Stats = new DrawStats
            {
                TimesDrawnUpright = document.Stats?.TimesDrawnUpright ?? 0,
                TimesDrawnReversed = document.Stats?.TimesDrawnReversed ?? 0,
            },
            Customization = document.Customization is null ? null : new CardCustomization
            {
                SignatureName = document.Customization.SignatureName,
                ActiveSkinId = document.Customization.ActiveSkinId,
            },
            BaseAtk = document.BaseAtk,
            BaseDef = document.BaseDef,
            BonusAtkPercent = document.BonusAtkPercent,
            BonusDefPercent = document.BonusDefPercent,
            Atk = document.Atk,
            Def = document.Def,
            StatHistory = CloneStatHistory(document.StatHistory),
            IsDeleted = document.IsDeleted,
            DeletedAt = document.DeletedAt,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt,
            LastDrawnAt = document.LastDrawnAt,
        };
    }

    /// <summary>
    /// Áp EXP gain và tự động level up theo EXP curve.
    /// </summary>
    private static void ApplyExpAndProgression(UserCollectionDocument document, decimal expGain, DateTime nowUtc)
    {
        if (expGain < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(expGain), "expGain must be greater than or equal to 0.");
        }

        NormalizeAndHydrateLegacyFields(document);
        document.Exp = Round2(document.Exp + expGain);

        while (CanLevelUp(document))
        {
            LevelUpDocument(document, nowUtc);
        }

        if (document.Level >= UserCollection.MaxLevel)
        {
            document.Level = UserCollection.MaxLevel;
            document.Exp = 0m;
            document.ExpToNextLevel = 0m;
        }

        RecalculateTotalStats(document);
        document.LastDrawnAt = nowUtc;
        document.UpdatedAt = nowUtc;
    }

    /// <summary>
    /// Tính lại total ATK/DEF từ base + bonus%.
    /// </summary>
    private static void RecalculateTotalStats(UserCollectionDocument document)
    {
        document.BaseAtk = Round2(Math.Max(document.BaseAtk, 0m));
        document.BaseDef = Round2(Math.Max(document.BaseDef, 0m));
        document.BonusAtkPercent = Round2(Math.Max(document.BonusAtkPercent, 0m));
        document.BonusDefPercent = Round2(Math.Max(document.BonusDefPercent, 0m));
        document.Atk = UserCollection.CalculateTotalStat(document.BaseAtk, document.BonusAtkPercent);
        document.Def = UserCollection.CalculateTotalStat(document.BaseDef, document.BonusDefPercent);
    }

    private static bool CanLevelUp(UserCollectionDocument document)
    {
        return document.Level < UserCollection.MaxLevel
               && document.ExpToNextLevel > 0m
               && document.Exp >= document.ExpToNextLevel;
    }

    private static void LevelUpDocument(UserCollectionDocument document, DateTime nowUtc)
    {
        document.Exp = Round2(document.Exp - document.ExpToNextLevel);
        document.Level += 1;

        var (atkBonus, defBonus) = RollBaseStatBonuses(document.Level);
        document.BaseAtk = Round2(document.BaseAtk + atkBonus);
        document.BaseDef = Round2(document.BaseDef + defBonus);
        document.StatHistory!.Add(new StatRollRecord
        {
            Level = document.Level,
            AtkBonus = atkBonus,
            DefBonus = defBonus,
            RolledAt = nowUtc,
        });

        document.ExpToNextLevel = UserCollection.ResolveExpToNextLevel(document.Level);
    }

    private static (decimal atkBonus, decimal defBonus) RollBaseStatBonuses(int targetLevel)
    {
        var (minBonus, maxBonus) = UserCollection.GetStatBonusRange(targetLevel);
        var atkBonus = RandomNumberGenerator.GetInt32(minBonus, maxBonus + 1);
        var defBonus = RandomNumberGenerator.GetInt32(minBonus, maxBonus + 1);
        return (atkBonus, defBonus);
    }

}
