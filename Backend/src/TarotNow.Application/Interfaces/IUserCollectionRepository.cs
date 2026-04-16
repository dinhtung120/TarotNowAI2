

using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract bộ sưu tập lá bài người dùng để quản lý tiến trình sở hữu và tăng cấp thẻ.
/// </summary>
public interface IUserCollectionRepository
{
    /// <summary>
    /// Tạo mới hoặc cập nhật thẻ trong bộ sưu tập khi người dùng nhận thêm kinh nghiệm.
    /// Luồng xử lý: định vị thẻ theo userId/cardId, cộng expToGain và lưu trạng thái mới.
    /// </summary>
    Task UpsertCardAsync(
        Guid userId,
        int cardId,
        decimal expToGain,
        string orientation = CardOrientation.Upright,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ bộ sưu tập của người dùng để hiển thị kho thẻ hiện có.
    /// Luồng xử lý: lọc theo userId và trả danh sách UserCollection tương ứng.
    /// </summary>
    Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra user có sở hữu lá bài hay chưa.
    /// Luồng xử lý: lọc theo userId/cardId và trả cờ tồn tại.
    /// </summary>
    Task<bool> ExistsAsync(Guid userId, int cardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Áp enhancement lên lá bài trong bộ sưu tập.
    /// Luồng xử lý: xác định kiểu enhancement, cập nhật chỉ số tương ứng và trả delta kết quả.
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
    /// Delta Attack được cộng.
    /// </summary>
    public decimal AttackDelta { get; init; }

    /// <summary>
    /// Delta Defense được cộng.
    /// </summary>
    public decimal DefenseDelta { get; init; }

    /// <summary>
    /// Cờ cho biết level upgrade thành công.
    /// </summary>
    public bool LevelUpgraded { get; init; }
}
