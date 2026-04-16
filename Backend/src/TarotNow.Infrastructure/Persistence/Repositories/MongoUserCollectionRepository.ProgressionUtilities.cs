using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Partial helper utility cho progression và snapshot stats.
/// </summary>
public partial class MongoUserCollectionRepository
{
    /// <summary>
    /// Dựng snapshot trước/sau để trả về cho API.
    /// </summary>
    private static CardEnhancementStatSnapshot BuildStatSnapshot(UserCollectionDocument document)
    {
        return new CardEnhancementStatSnapshot
        {
            Level = document.Level,
            CurrentExp = Round2(document.Exp),
            ExpToNextLevel = Round2(document.ExpToNextLevel),
            BaseAtk = Round2(document.BaseAtk),
            BaseDef = Round2(document.BaseDef),
            BonusAtkPercent = Round2(document.BonusAtkPercent),
            BonusDefPercent = Round2(document.BonusDefPercent),
            TotalAtk = Round2(document.Atk),
            TotalDef = Round2(document.Def),
        };
    }

    private static decimal ResolveBaseStat(decimal currentBase, decimal legacyTotal, decimal fallback)
    {
        var sourceValue = currentBase <= 0m ? legacyTotal : currentBase;
        return Round2(sourceValue <= 0m ? fallback : sourceValue);
    }

    private static decimal ResolveExpToNextLevel(int level, decimal currentValue)
    {
        var resolved = currentValue <= 0m ? UserCollection.ResolveExpToNextLevel(level) : currentValue;
        return Round2(Math.Max(0m, resolved));
    }

    private static List<StatRollRecord> CloneStatHistory(List<StatRollRecord>? records)
    {
        return records?.Select(record => new StatRollRecord
        {
            Level = record.Level,
            AtkBonus = record.AtkBonus,
            DefBonus = record.DefBonus,
            RolledAt = record.RolledAt,
        }).ToList() ?? new List<StatRollRecord>();
    }

    private static decimal Round2(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
