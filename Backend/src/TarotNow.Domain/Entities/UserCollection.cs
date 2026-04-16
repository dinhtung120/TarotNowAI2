using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Entity bộ sưu tập thẻ của người dùng để theo dõi bản sao, cấp độ và chỉ số chiến đấu.
/// </summary>
public class UserCollection
{
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
    /// Số bản sao đã sở hữu.
    /// </summary>
    public int Copies { get; private set; }

    /// <summary>
    /// Tổng EXP cộng dồn cho thẻ.
    /// </summary>
    public decimal ExpGained { get; private set; }

    /// <summary>
    /// Thời điểm gần nhất người dùng rút được thẻ này.
    /// </summary>
    public DateTime LastDrawnAt { get; private set; }

    /// <summary>
    /// Chỉ số tấn công của thẻ.
    /// </summary>
    public decimal Atk { get; private set; }

    /// <summary>
    /// Chỉ số phòng thủ của thẻ.
    /// </summary>
    public decimal Def { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// </summary>
    protected UserCollection()
    {
    }

    /// <summary>
    /// Khởi tạo thẻ mới trong bộ sưu tập với chỉ số và cấp mặc định.
    /// </summary>
    public UserCollection(Guid userId, int cardId)
    {
        UserId = userId;
        CardId = cardId;
        Level = 1;
        Copies = 1;
        ExpGained = 0m;
        LastDrawnAt = DateTime.UtcNow;
        Atk = 10m;
        Def = 10m;
    }

    /// <summary>
    /// Khôi phục UserCollection từ snapshot để tái tạo đúng state đã lưu.
    /// </summary>
    public static UserCollection Rehydrate(UserCollectionSnapshot snapshot)
    {
        return new UserCollection(snapshot.UserId, snapshot.CardId)
        {
            Level = snapshot.Level,
            Copies = snapshot.Copies,
            ExpGained = snapshot.ExpGained,
            LastDrawnAt = snapshot.LastDrawnAt,
            Atk = snapshot.Atk,
            Def = snapshot.Def,
        };
    }

    /// <summary>
    /// Thêm một bản sao thẻ và cộng EXP tương ứng sau mỗi lần rút trúng.
    /// </summary>
    public void AddCopy(decimal expToGain)
    {
        Copies += 1;
        ExpGained = Round2(ExpGained + expToGain);
        LastDrawnAt = DateTime.UtcNow;

        if (Copies % 5 == 0)
        {
            Level += 1;
        }
    }

    /// <summary>
    /// Áp chỉ số thưởng khi thẻ lên cấp.
    /// </summary>
    public void ApplyLevelUpStats(decimal atkBonus, decimal defBonus)
    {
        Atk = Round2(Atk + atkBonus);
        Def = Round2(Def + defBonus);
    }

    /// <summary>
    /// Cộng thêm EXP trực tiếp cho thẻ từ item enhancer.
    /// </summary>
    public void AddExp(decimal expAmount)
    {
        if (expAmount <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(expAmount), "expAmount must be > 0.");
        }

        ExpGained = Round2(ExpGained + expAmount);
        LastDrawnAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cộng chỉ số tấn công trực tiếp từ item booster.
    /// </summary>
    public void IncreaseAttack(decimal amount)
    {
        if (amount <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "amount must be > 0.");
        }

        Atk = Round2(Atk + amount);
        LastDrawnAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cộng chỉ số phòng thủ trực tiếp từ item booster.
    /// </summary>
    public void IncreaseDefense(decimal amount)
    {
        if (amount <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "amount must be > 0.");
        }

        Def = Round2(Def + amount);
        LastDrawnAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Nâng cấp cấp độ thẻ khi có bonus được tính ở tầng hạ tầng.
    /// </summary>
    public void ApplyLevelUpgrade(decimal atkBonus, decimal defBonus)
    {
        if (atkBonus <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(atkBonus), "atkBonus must be > 0.");
        }

        if (defBonus <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(defBonus), "defBonus must be > 0.");
        }

        Level += 1;
        ApplyLevelUpStats(atkBonus, defBonus);
        LastDrawnAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Trả khoảng bonus chỉ số hợp lệ theo cấp mới của thẻ.
    /// </summary>
    public static (int min, int max) GetStatBonusRange(int newLevel)
    {
        return (10, newLevel * 10);
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
    /// EXP tích lũy.
    /// </summary>
    public decimal ExpGained { get; init; }

    /// <summary>
    /// Thời điểm rút gần nhất.
    /// </summary>
    public DateTime LastDrawnAt { get; init; }

    /// <summary>
    /// Chỉ số tấn công.
    /// </summary>
    public decimal Atk { get; init; }

    /// <summary>
    /// Chỉ số phòng thủ.
    /// </summary>
    public decimal Def { get; init; }
}
