
using System;

namespace TarotNow.Domain.Entities;

// Entity bộ sưu tập thẻ của người dùng để theo dõi bản sao, cấp độ và chỉ số chiến đấu.
public class UserCollection
{
    // Chủ sở hữu thẻ.
    public Guid UserId { get; private set; }

    // Định danh lá bài.
    public int CardId { get; private set; }

    // Cấp độ hiện tại của thẻ.
    public int Level { get; private set; }

    // Số bản sao đã sở hữu.
    public int Copies { get; private set; }

    // Tổng EXP cộng dồn cho thẻ.
    public long ExpGained { get; private set; }

    // Thời điểm gần nhất người dùng rút được thẻ này.
    public DateTime LastDrawnAt { get; private set; }

    // Chỉ số tấn công của thẻ.
    public int Atk { get; private set; }

    // Chỉ số phòng thủ của thẻ.
    public int Def { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khôi phục entity từ dữ liệu lưu trữ.
    /// </summary>
    protected UserCollection() { }

    /// <summary>
    /// Khởi tạo thẻ mới trong bộ sưu tập với chỉ số và cấp mặc định.
    /// Luồng xử lý: gán user/card, set level/copies ban đầu và thiết lập chỉ số nền.
    /// </summary>
    public UserCollection(Guid userId, int cardId)
    {
        UserId = userId;
        CardId = cardId;
        Level = 1;
        Copies = 1;
        ExpGained = 0;
        LastDrawnAt = DateTime.UtcNow;
        Atk = 10;
        Def = 10;
    }

    /// <summary>
    /// Khôi phục UserCollection từ snapshot để tái tạo đúng state đã lưu.
    /// Luồng xử lý: tạo entity nền rồi gán lại toàn bộ thuộc tính từ snapshot.
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
            Def = snapshot.Def
        };
    }

    /// <summary>
    /// Thêm một bản sao thẻ và cộng EXP tương ứng sau mỗi lần rút trúng.
    /// Luồng xử lý: tăng copies/exp, cập nhật LastDrawnAt và tăng level mỗi mốc 5 bản sao.
    /// </summary>
    public void AddCopy(long expToGain)
    {
        Copies += 1;
        ExpGained += expToGain;
        LastDrawnAt = DateTime.UtcNow;
        // Cập nhật state chính của thẻ ngay sau khi nhận thêm bản sao.

        if (Copies % 5 == 0)
        {
            Level += 1;
            // Business rule: cứ mỗi 5 bản sao thì tăng 1 cấp thẻ.
        }
    }

    /// <summary>
    /// Áp chỉ số thưởng khi thẻ lên cấp.
    /// Luồng xử lý: cộng trực tiếp atkBonus/defBonus vào chỉ số hiện tại.
    /// </summary>
    public void ApplyLevelUpStats(int atkBonus, int defBonus)
    {
        Atk += atkBonus;
        Def += defBonus;
        // Đồng bộ chỉ số chiến đấu sau khi hoàn tất xử lý level-up.
    }

    /// <summary>
    /// Trả khoảng bonus chỉ số hợp lệ theo cấp mới của thẻ.
    /// Luồng xử lý: giữ min cố định và tăng max tuyến tính theo newLevel.
    /// </summary>
    public static (int min, int max) GetStatBonusRange(int newLevel)
    {
        return (10, newLevel * 10);
    }
}

// Snapshot bộ sưu tập thẻ để rehydrate domain entity.
public sealed class UserCollectionSnapshot
{
    // Định danh người dùng.
    public Guid UserId { get; init; }

    // Định danh thẻ.
    public int CardId { get; init; }

    // Cấp độ hiện tại.
    public int Level { get; init; }

    // Số bản sao hiện có.
    public int Copies { get; init; }

    // EXP tích lũy.
    public long ExpGained { get; init; }

    // Thời điểm rút gần nhất.
    public DateTime LastDrawnAt { get; init; }

    // Chỉ số tấn công.
    public int Atk { get; init; }

    // Chỉ số phòng thủ.
    public int Def { get; init; }
}
