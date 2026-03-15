using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document cho collection "user_collections" — Bộ sưu tập lá bài của user.
///
/// Mỗi user có tối đa 78 document (1 per card). Khi rút bài trùng,
/// copies tăng + EXP cộng dồn → level up theo công thức (mỗi 5 copies = +1 level).
///
/// Unique index: (user_id, card_id) — đảm bảo 1 user chỉ có 1 record per card.
/// </summary>
public class UserCollectionDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>UUID user từ PostgreSQL — string dạng "550e8400-e29b-41d4-a716-446655440000".</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>ID lá bài (0-77) tham chiếu cards_catalog._id.</summary>
    [BsonElement("card_id")]
    public int CardId { get; set; }

    /// <summary>
    /// Cấp lá (1-20+). Quyết định số free follow-up slots khi rút lá này.
    /// Formula hiện tại: mỗi 5 copies = +1 level (có thể tùy biến Phase Gacha).
    /// </summary>
    [BsonElement("level")]
    public int Level { get; set; } = 1;

    /// <summary>EXP hiện tại — cộng dồn mỗi khi rút trùng.</summary>
    [BsonElement("exp")]
    public long Exp { get; set; } = 0;

    /// <summary>Bậc Ascension — unlock ở level 6+ (Phase tương lai).</summary>
    [BsonElement("ascension_tier")]
    public int AscensionTier { get; set; } = 0;

    /// <summary>Thống kê rút bài: số lần xuôi/ngược.</summary>
    [BsonElement("stats")]
    public DrawStats Stats { get; set; } = new();

    /// <summary>Tùy chỉnh cá nhân: tên signature, skin.</summary>
    [BsonElement("customization")]
    [BsonIgnoreIfNull]
    public CardCustomization? Customization { get; set; }

    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("last_drawn_at")]
    public DateTime LastDrawnAt { get; set; } = DateTime.UtcNow;
}

/// <summary>Thống kê rút bài — phân biệt xuôi/ngược cho analytics.</summary>
public class DrawStats
{
    [BsonElement("times_drawn_upright")] public int TimesDrawnUpright { get; set; }
    [BsonElement("times_drawn_reversed")] public int TimesDrawnReversed { get; set; }
}

/// <summary>Tùy chỉnh lá bài — Phase tương lai.</summary>
public class CardCustomization
{
    [BsonElement("signature_name")][BsonIgnoreIfNull] public string? SignatureName { get; set; }
    [BsonElement("active_skin_id")][BsonIgnoreIfNull] public string? ActiveSkinId { get; set; }
}
