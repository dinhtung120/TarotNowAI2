using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document cho collection "cards_catalog" — 78 lá bài Tarot.
///
/// Tại sao tạo POCO riêng thay vì dùng Domain Entity?
/// → Clean Architecture: Domain Entity không nên phụ thuộc vào MongoDB attributes.
///   Document model là "persistence concern" thuộc Infrastructure layer.
///   Khi cần, dùng mapper để chuyển đổi giữa Document ↔ Domain Entity.
///
/// _id là Int32 (0-77) thay vì ObjectId mặc định — trùng với seed_cards.js.
/// </summary>
public class CardCatalogDocument
{
    /// <summary>
    /// ID cố định (0-77) — map trực tiếp với deck Tarot.
    /// Major Arcana: 0-21, Minor Arcana: 22-77.
    /// </summary>
    [BsonId]
    public int Id { get; set; }

    /// <summary>
    /// Mã ổn định dùng làm key logic (VD: "the_fool", "ace_of_wands").
    /// Unique index trong MongoDB.
    /// </summary>
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Tên đa ngôn ngữ — i18n spec yêu cầu vi/en/zh với fallback chain.
    /// </summary>
    [BsonElement("name")]
    public LocalizedName Name { get; set; } = new();

    /// <summary>major hoặc minor — phân biệt 2 nhóm bài lớn.</summary>
    [BsonElement("arcana")]
    public string Arcana { get; set; } = string.Empty;

    /// <summary>
    /// wands/cups/swords/pentacles hoặc null (Major Arcana không có suit).
    /// </summary>
    [BsonElement("suit")]
    [BsonIgnoreIfNull]
    public string? Suit { get; set; }

    /// <summary>Số thứ tự trong bộ.</summary>
    [BsonElement("number")]
    public int Number { get; set; }

    /// <summary>Nguyên tố: fire/water/air/earth.</summary>
    [BsonElement("element")]
    public string Element { get; set; } = string.Empty;

    /// <summary>Ý nghĩa xuôi (upright) và ngược (reversed).</summary>
    [BsonElement("meanings")]
    public CardMeanings Meanings { get; set; } = new();

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Tên bài đa ngôn ngữ — 3 locale bắt buộc theo spec i18n.</summary>
public class LocalizedName
{
    [BsonElement("vi")] public string Vi { get; set; } = string.Empty;
    [BsonElement("en")] public string En { get; set; } = string.Empty;
    [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}

/// <summary>Ý nghĩa lá bài — mỗi hướng có keywords + description.</summary>
public class CardMeanings
{
    [BsonElement("upright")] public MeaningDetail Upright { get; set; } = new();
    [BsonElement("reversed")] public MeaningDetail Reversed { get; set; } = new();
}

/// <summary>Chi tiết ý nghĩa: danh sách từ khóa + mô tả đầy đủ.</summary>
public class MeaningDetail
{
    [BsonElement("keywords")] public List<string> Keywords { get; set; } = new();
    [BsonElement("description")] public string Description { get; set; } = string.Empty;
}
