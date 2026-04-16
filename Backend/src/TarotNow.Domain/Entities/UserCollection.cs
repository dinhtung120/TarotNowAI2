using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Entity bộ sưu tập thẻ của người dùng để theo dõi cấp độ và chỉ số chiến đấu.
/// </summary>
public class UserCollection
{
    /// <summary>
    /// Level tối đa của một lá bài.
    /// </summary>
    public const int MaxLevel = 100;

    /// <summary>
    /// EXP nền tảng cần để lên từ level 1 -> 2.
    /// </summary>
    public const decimal BaseExpToNextLevel = 100m;

    /// <summary>
    /// EXP cộng thêm cho mỗi level tiếp theo.
    /// </summary>
    public const decimal ExpIncreasePerLevel = 50m;

    /// <summary>
    /// ATK nền tảng ban đầu của card.
    /// </summary>
    public const decimal DefaultBaseAtk = 10m;

    /// <summary>
    /// DEF nền tảng ban đầu của card.
    /// </summary>
    public const decimal DefaultBaseDef = 10m;

    /// <summary>
    /// Chủ sở hữu thẻ.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Định danh lá bài.
    /// </summary>
    public int CardId { get; private set; }

    /// <summary>
    /// Cấp độ hiện tại của thẻ.
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// Số bản sao đã sở hữu (theo số lần bốc).
    /// </summary>
    public int Copies { get; private set; }

    /// <summary>
    /// EXP hiện tại trong level đang đứng.
    /// </summary>
    public decimal CurrentExp { get; private set; }

    /// <summary>
    /// EXP cần để lên level kế tiếp.
    /// </summary>
    public decimal ExpToNextLevel { get; private set; }

    /// <summary>
    /// ATK nền tảng tăng theo level.
    /// </summary>
    public decimal BaseAtk { get; private set; }

    /// <summary>
    /// DEF nền tảng tăng theo level.
    /// </summary>
    public decimal BaseDef { get; private set; }

    /// <summary>
    /// % bonus ATK do item booster.
    /// </summary>
    public decimal BonusAtkPercent { get; private set; }

    /// <summary>
    /// % bonus DEF do item booster.
    /// </summary>
    public decimal BonusDefPercent { get; private set; }

    /// <summary>
    /// Tổng ATK hiển thị (base + bonus).
    /// </summary>
    public decimal TotalAtk => CalculateTotalStat(BaseAtk, BonusAtkPercent);

    /// <summary>
    /// Tổng DEF hiển thị (base + bonus).
    /// </summary>
    public decimal TotalDef => CalculateTotalStat(BaseDef, BonusDefPercent);

    /// <summary>
    /// Alias giữ tương thích cho các client cũ.
    /// </summary>
    public decimal Atk => TotalAtk;

    /// <summary>
    /// Alias giữ tương thích cho các client cũ.
    /// </summary>
    public decimal Def => TotalDef;

    /// <summary>
    /// Alias giữ tương thích cho các client cũ.
    /// </summary>
    public decimal ExpGained => CurrentExp;

    /// <summary>
    /// Thời điểm gần nhất người dùng rút được thẻ này.
    /// </summary>
    public DateTime LastDrawnAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// </summary>
    protected UserCollection()
    {
    }

    /// <summary>
    /// Khởi tạo thẻ mới trong bộ sưu tập với chỉ số mặc định.
    /// </summary>
    public UserCollection(Guid userId, int cardId)
    {
        UserId = userId;
        CardId = cardId;
        Level = 1;
        Copies = 1;
        CurrentExp = 0m;
        ExpToNextLevel = ResolveExpToNextLevel(1);
        BaseAtk = DefaultBaseAtk;
        BaseDef = DefaultBaseDef;
        BonusAtkPercent = 0m;
        BonusDefPercent = 0m;
        LastDrawnAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Khôi phục UserCollection từ snapshot để tái tạo đúng state đã lưu.
    /// </summary>
    public static UserCollection Rehydrate(UserCollectionSnapshot snapshot)
    {
        return new UserCollection(snapshot.UserId, snapshot.CardId)
        {
            Level = Math.Clamp(snapshot.Level, 1, MaxLevel),
            Copies = Math.Max(snapshot.Copies, 1),
            CurrentExp = Round2(Math.Max(snapshot.CurrentExp, 0m)),
            ExpToNextLevel = Round2(Math.Max(snapshot.ExpToNextLevel, 0m)),
            BaseAtk = Round2(Math.Max(snapshot.BaseAtk, 0m)),
            BaseDef = Round2(Math.Max(snapshot.BaseDef, 0m)),
            BonusAtkPercent = Round2(Math.Max(snapshot.BonusAtkPercent, 0m)),
            BonusDefPercent = Round2(Math.Max(snapshot.BonusDefPercent, 0m)),
            LastDrawnAt = snapshot.LastDrawnAt,
        };
    }

    /// <summary>
    /// Trả khoảng bonus chỉ số hợp lệ theo cấp mới của thẻ.
    /// </summary>
    public static (int min, int max) GetStatBonusRange(int newLevel)
    {
        return (10, newLevel * 10);
    }

    /// <summary>
    /// Tính EXP cần cho level kế tiếp theo đường cong tuyến tính.
    /// </summary>
    public static decimal ResolveExpToNextLevel(int level)
    {
        var safeLevel = Math.Clamp(level, 1, MaxLevel);
        if (safeLevel >= MaxLevel)
        {
            return 0m;
        }

        return BaseExpToNextLevel + ((safeLevel - 1) * ExpIncreasePerLevel);
    }

    /// <summary>
    /// Tính tổng chỉ số sau khi áp dụng bonus phần trăm.
    /// </summary>
    public static decimal CalculateTotalStat(decimal baseValue, decimal bonusPercent)
    {
        var clampedBase = Math.Max(0m, baseValue);
        var clampedBonus = Math.Max(0m, bonusPercent);
        var total = clampedBase + (clampedBase * clampedBonus / 100m);
        return Round2(total);
    }

    private static decimal Round2(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}

/// <summary>
/// Snapshot bộ sưu tập thẻ để rehydrate domain entity.
/// </summary>
public sealed class UserCollectionSnapshot
{
    /// <summary>
    /// Định danh người dùng.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Định danh thẻ.
    /// </summary>
    public int CardId { get; init; }

    /// <summary>
    /// Cấp độ hiện tại.
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// Số bản sao hiện có.
    /// </summary>
    public int Copies { get; init; }

    /// <summary>
    /// EXP hiện tại.
    /// </summary>
    public decimal CurrentExp { get; init; }

    /// <summary>
    /// EXP cần để lên cấp tiếp theo.
    /// </summary>
    public decimal ExpToNextLevel { get; init; }

    /// <summary>
    /// ATK nền tảng.
    /// </summary>
    public decimal BaseAtk { get; init; }

    /// <summary>
    /// DEF nền tảng.
    /// </summary>
    public decimal BaseDef { get; init; }

    /// <summary>
    /// % bonus ATK.
    /// </summary>
    public decimal BonusAtkPercent { get; init; }

    /// <summary>
    /// % bonus DEF.
    /// </summary>
    public decimal BonusDefPercent { get; init; }

    /// <summary>
    /// Thời điểm rút gần nhất.
    /// </summary>
    public DateTime LastDrawnAt { get; init; }
}
