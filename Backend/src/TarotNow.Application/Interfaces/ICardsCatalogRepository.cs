namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository interface cho cards_catalog (78 lá bài Tarot).
/// 
/// Tại sao cần interface riêng?
/// → cards_catalog là dữ liệu tĩnh trong MongoDB. Trước đây chưa có
///   repository vì data chỉ tồn tại trong seed script. Nay cần đọc
///   từ backend để phục vụ: rút bài random, hiển thị chi tiết lá bài,
///   và tính EXP khi rút trùng.
/// </summary>
public interface ICardsCatalogRepository
{
    /// <summary>Lấy 1 lá bài theo ID (0-77).</summary>
    Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Lấy 1 lá bài theo code (VD: "the_fool").</summary>
    Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Lấy toàn bộ 78 lá — dùng cho cache hoặc random draw.</summary>
    Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO nhẹ cho cards_catalog — tách biệt khỏi MongoDB document model.
/// Application layer chỉ nhận DTO, không biết MongoDB tồn tại.
/// </summary>
public class CardCatalogDto
{
    /// <summary>ID cố định (0-77).</summary>
    public int Id { get; set; }

    /// <summary>Mã ổn định: the_fool, ace_of_wands, ...</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Tên tiếng Việt.</summary>
    public string NameVi { get; set; } = string.Empty;

    /// <summary>Tên tiếng Anh.</summary>
    public string NameEn { get; set; } = string.Empty;

    /// <summary>Tên tiếng Trung.</summary>
    public string NameZh { get; set; } = string.Empty;

    /// <summary>major hoặc minor.</summary>
    public string Arcana { get; set; } = string.Empty;

    /// <summary>wands/cups/swords/pentacles hoặc null.</summary>
    public string? Suit { get; set; }

    /// <summary>Nguyên tố: fire/water/air/earth.</summary>
    public string Element { get; set; } = string.Empty;

    /// <summary>Số thứ tự trong bộ.</summary>
    public int Number { get; set; }
}
