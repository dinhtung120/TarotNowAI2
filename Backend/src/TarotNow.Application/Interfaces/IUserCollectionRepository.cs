using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract bộ sưu tập lá bài người dùng để quản lý tiến trình sở hữu và tăng cấp thẻ.
/// </summary>
public interface IUserCollectionRepository
{
    /// <summary>
    /// Tạo mới hoặc cập nhật thẻ trong bộ sưu tập khi người dùng nhận thêm kinh nghiệm.
    /// </summary>
    Task UpsertCardAsync(
        Guid userId,
        int cardId,
        decimal expToGain,
        string orientation = CardOrientation.Upright,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ bộ sưu tập của người dùng để hiển thị kho thẻ hiện có.
    /// </summary>
    Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra user có sở hữu lá bài hay chưa.
    /// </summary>
    Task<bool> ExistsAsync(Guid userId, int cardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Áp enhancement lên lá bài trong bộ sưu tập.
    /// </summary>
    Task<CardEnhancementApplyResult> ApplyEnhancementAsync(
        CardEnhancementApplyRequest request,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Request áp dụng enhancement cho một lá bài.
/// </summary>
public sealed class CardEnhancementApplyRequest
{
    /// <summary>
    /// Người dùng sở hữu lá bài.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Id lá bài mục tiêu.
    /// </summary>
    public int CardId { get; init; }

    /// <summary>
    /// Kiểu enhancement cần áp dụng.
    /// </summary>
    public string EnhancementType { get; init; } = string.Empty;

    /// <summary>
    /// Giá trị hiệu ứng tăng thêm.
    /// </summary>
    public decimal EffectValue { get; init; }

    /// <summary>
    /// Tỉ lệ thành công (0-100) cho item có xác suất.
    /// </summary>
    public decimal SuccessRatePercent { get; init; }
}

/// <summary>
/// Snapshot chỉ số card tại một thời điểm.
/// </summary>
public sealed class CardEnhancementStatSnapshot
{
    /// <summary>
    /// Level hiện tại của card.
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// EXP hiện tại trong level.
    /// </summary>
    public decimal CurrentExp { get; init; }

    /// <summary>
    /// EXP cần để lên level tiếp theo.
    /// </summary>
    public decimal ExpToNextLevel { get; init; }

    /// <summary>
    /// Base ATK.
    /// </summary>
    public decimal BaseAtk { get; init; }

    /// <summary>
    /// Base DEF.
    /// </summary>
    public decimal BaseDef { get; init; }

    /// <summary>
    /// Bonus % ATK.
    /// </summary>
    public decimal BonusAtkPercent { get; init; }

    /// <summary>
    /// Bonus % DEF.
    /// </summary>
    public decimal BonusDefPercent { get; init; }

    /// <summary>
    /// Tổng ATK hiển thị.
    /// </summary>
    public decimal TotalAtk { get; init; }

    /// <summary>
    /// Tổng DEF hiển thị.
    /// </summary>
    public decimal TotalDef { get; init; }
}

/// <summary>
/// Kết quả áp dụng enhancement cho card.
/// </summary>
public sealed class CardEnhancementApplyResult
{
    /// <summary>
    /// Cờ cho biết xử lý có thành công.
    /// </summary>
    public bool Succeeded { get; init; }

    /// <summary>
    /// Delta EXP được cộng.
    /// </summary>
    public decimal ExpDelta { get; init; }

    /// <summary>
    /// Delta tổng Attack được cộng.
    /// </summary>
    public decimal AttackDelta { get; init; }

    /// <summary>
    /// Delta tổng Defense được cộng.
    /// </summary>
    public decimal DefenseDelta { get; init; }

    /// <summary>
    /// Giá trị roll thực tế của effect (% hoặc EXP).
    /// </summary>
    public decimal RolledValue { get; init; }

    /// <summary>
    /// Cờ cho biết level upgrade có xảy ra hay không.
    /// </summary>
    public bool LevelUpgraded { get; init; }

    /// <summary>
    /// Snapshot trước khi áp dụng enhancement.
    /// </summary>
    public CardEnhancementStatSnapshot BeforeStats { get; init; } = new();

    /// <summary>
    /// Snapshot sau khi áp dụng enhancement.
    /// </summary>
    public CardEnhancementStatSnapshot AfterStats { get; init; } = new();
}
