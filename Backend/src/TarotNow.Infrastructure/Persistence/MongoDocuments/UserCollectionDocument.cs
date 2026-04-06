/*
 * FILE: UserCollectionDocument.cs
 * MỤC ĐÍCH: Schema cho collection "user_collections" (MongoDB).
 *   Bộ sưu tập lá bài cá nhân của mỗi User — hệ thống Gacha/Collection.
 *
 *   CÁCH HOẠT ĐỘNG:
 *   → User rút bài (draw) → nếu chưa có lá này → tạo document mới (copies=1, level=1)
 *   → Rút trùng lá đã có → EXP tăng, copies tăng → khi đủ EXP → level up
 *   → Level lá bài ảnh hưởng: số free follow-up slots khi dùng lá này để bói
 *
 *   LEVEL FORMULA: Mỗi 5 copies = +1 level (có thể thay đổi trong Phase Gacha).
 *   
 *   Unique index: (user_id, card_id) → đảm bảo 1 User chỉ có 1 record cho mỗi lá.
 *   1 User có tối đa 78 document (tổng số lá Tarot).
 *
 *   Tham chiếu: schema.md §2 (user_collections)
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// 1 lá bài trong bộ sưu tập cá nhân của User.
/// </summary>
public class UserCollectionDocument
{
    /// <summary>ObjectId tự sinh.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>UUID User sở hữu lá bài (từ PostgreSQL).</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>ID lá bài (0-77) — tham chiếu cards_catalog._id.</summary>
    [BsonElement("card_id")]
    public int CardId { get; set; }

    /// <summary>
    /// Cấp độ lá bài (1-20+). Level cao → nhiều free follow-up slots hơn.
    /// Formula: mỗi 5 copies = +1 level. Level mặc định = 1.
    /// </summary>
    [BsonElement("level")]
    public int Level { get; set; } = 1;

    /// <summary>EXP (kinh nghiệm) hiện tại — cộng dồn mỗi khi rút trùng lá này.</summary>
    [BsonElement("exp")]
    public long Exp { get; set; } = 0;

    /// <summary>
    /// Bậc Ascension — tính năng nâng cấp đặc biệt, unlock khi level ≥ 6 (Phase tương lai).
    /// Ascension tier 0 = chưa ascend.
    /// </summary>
    [BsonElement("ascension_tier")]
    public int AscensionTier { get; set; } = 0;

    /// <summary>
    /// Thống kê rút bài: bao nhiêu lần xuôi (upright), bao nhiêu lần ngược (reversed).
    /// Dùng cho analytics và hiển thị trên UI profile lá bài.
    /// </summary>
    [BsonElement("stats")]
    public DrawStats Stats { get; set; } = new();

    /// <summary>
    /// Tùy chỉnh cá nhân: đặt tên riêng (signature), thay skin. Tính năng tương lai.
    /// Null nếu chưa tùy chỉnh.
    /// </summary>
    [BsonElement("customization")]
    [BsonIgnoreIfNull]
    public CardCustomization? Customization { get; set; }

    /// <summary>Chỉ số Tấn công (Attack) của lá bài. Base = 10, tăng ngẫu nhiên mỗi level.</summary>
    [BsonElement("atk")]
    public int Atk { get; set; } = 10;

    /// <summary>Chỉ số Phòng thủ (Defense) của lá bài. Base = 10, tăng ngẫu nhiên mỗi level.</summary>
    [BsonElement("def")]
    public int Def { get; set; } = 10;

    /// <summary>Lịch sử roll stat mỗi lần level up — audit trail cho tính công bằng (fairness).</summary>
    [BsonElement("stat_history")]
    [BsonIgnoreIfNull]
    public List<StatRollRecord>? StatHistory { get; set; }

    /// <summary>Soft delete flag.</summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    /// <summary>Thời điểm lá bài được thêm vào bộ sưu tập (lần rút đầu tiên).</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Thời điểm cập nhật cuối (level up, thêm EXP, v.v.).</summary>
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Thời điểm rút lá bài này gần nhất.</summary>
    [BsonElement("last_drawn_at")]
    public DateTime LastDrawnAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Thống kê rút bài — tách xuôi/ngược cho analytics.
/// Giúp User biết: "Lá Death tôi rút 10 lần, 7 lần ngược" → thú vị cho trải nghiệm.
/// </summary>
public class DrawStats
{
    /// <summary>Số lần rút xuôi (upright).</summary>
    [BsonElement("times_drawn_upright")] public int TimesDrawnUpright { get; set; }
    /// <summary>Số lần rút ngược (reversed).</summary>
    [BsonElement("times_drawn_reversed")] public int TimesDrawnReversed { get; set; }
}

/// <summary>
/// Tùy chỉnh lá bài — Phase tương lai (Gacha cosmetics).
/// </summary>
public class CardCustomization
{
    /// <summary>Tên riêng User đặt cho lá bài (ví dụ: "Lá bài may mắn").</summary>
    [BsonElement("signature_name")][BsonIgnoreIfNull] public string? SignatureName { get; set; }
    /// <summary>ID skin đang dùng (tính năng cosmetic tương lai).</summary>
    [BsonElement("active_skin_id")][BsonIgnoreIfNull] public string? ActiveSkinId { get; set; }
}

/// <summary>
/// Ghi lại mỗi lần roll ATK/DEF khi level up — phục vụ audit và hiển thị UI theo yêu cầu của Gamification Phase.
/// Giúp người chơi biết được lá bài đã mạnh lên bao nhiêu qua mỗi cấp.
/// </summary>
public class StatRollRecord
{
    [BsonElement("level")] 
    public int Level { get; set; }
    
    [BsonElement("atk_bonus")] 
    public int AtkBonus { get; set; }
    
    [BsonElement("def_bonus")] 
    public int DefBonus { get; set; }
    
    [BsonElement("rolled_at")] 
    public DateTime RolledAt { get; set; }
}
