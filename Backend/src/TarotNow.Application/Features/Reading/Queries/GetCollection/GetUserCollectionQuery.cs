using MediatR;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

/// <summary>
/// Query lấy bộ sưu tập thẻ của người dùng.
/// </summary>
public sealed class GetUserCollectionQuery : IRequest<List<UserCollectionDto>>
{
    /// <summary>
    /// Định danh user cần lấy collection.
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// DTO thông tin một lá thẻ trong bộ sưu tập người dùng.
/// </summary>
public sealed class UserCollectionDto
{
    /// <summary>
    /// Định danh lá bài.
    /// </summary>
    public int CardId { get; set; }

    /// <summary>
    /// Cấp hiện tại của lá bài trong collection.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// EXP hiện tại trong level.
    /// </summary>
    public decimal CurrentExp { get; set; }

    /// <summary>
    /// EXP cần để lên level kế tiếp.
    /// </summary>
    public decimal ExpToNextLevel { get; set; }

    /// <summary>
    /// Thời điểm gần nhất lá bài được rút.
    /// </summary>
    public DateTime LastDrawnAt { get; set; }

    /// <summary>
    /// Số bản sao lá bài user đang sở hữu.
    /// </summary>
    public int Copies { get; set; }

    /// <summary>
    /// ATK nền tảng.
    /// </summary>
    public decimal BaseAtk { get; set; }

    /// <summary>
    /// DEF nền tảng.
    /// </summary>
    public decimal BaseDef { get; set; }

    /// <summary>
    /// Bonus % ATK.
    /// </summary>
    public decimal BonusAtkPercent { get; set; }

    /// <summary>
    /// Bonus % DEF.
    /// </summary>
    public decimal BonusDefPercent { get; set; }

    /// <summary>
    /// Tổng ATK hiển thị.
    /// </summary>
    public decimal TotalAtk { get; set; }

    /// <summary>
    /// Tổng DEF hiển thị.
    /// </summary>
    public decimal TotalDef { get; set; }

    /// <summary>
    /// Alias tương thích client cũ.
    /// </summary>
    public decimal Atk { get; set; }

    /// <summary>
    /// Alias tương thích client cũ.
    /// </summary>
    public decimal Def { get; set; }

    /// <summary>
    /// Alias tương thích client cũ.
    /// </summary>
    public decimal ExpGained { get; set; }
}
