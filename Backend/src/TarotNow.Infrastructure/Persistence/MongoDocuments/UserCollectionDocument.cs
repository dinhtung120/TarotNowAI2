using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Document thẻ bài người dùng sở hữu trong bộ sưu tập.
/// </summary>
public class UserCollectionDocument
{
    /// <summary>
    /// ObjectId của bản ghi collection.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// User sở hữu thẻ bài.
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Id card trong catalog.
    /// </summary>
    [BsonElement("card_id")]
    public int CardId { get; set; }

    /// <summary>
    /// Cấp hiện tại của card.
    /// </summary>
    [BsonElement("level")]
    public int Level { get; set; } = 1;

    /// <summary>
    /// EXP hiện tại trong level đang đứng.
    /// </summary>
    [BsonElement("exp")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Exp { get; set; } = 0m;

    /// <summary>
    /// EXP cần để lên level kế tiếp.
    /// </summary>
    [BsonElement("exp_to_next_level")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal ExpToNextLevel { get; set; } = UserCollection.ResolveExpToNextLevel(1);

    /// <summary>
    /// Bậc ascension của card.
    /// </summary>
    [BsonElement("ascension_tier")]
    public int AscensionTier { get; set; } = 0;

    /// <summary>
    /// Thống kê số lần rút upright/reversed.
    /// </summary>
    [BsonElement("stats")]
    public DrawStats Stats { get; set; } = new();

    /// <summary>
    /// Tùy biến giao diện của card (nếu có).
    /// </summary>
    [BsonElement("customization")]
    [BsonIgnoreIfNull]
    public CardCustomization? Customization { get; set; }

    /// <summary>
    /// ATK nền tảng tăng theo level.
    /// </summary>
    [BsonElement("base_atk")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BaseAtk { get; set; } = UserCollection.DefaultBaseAtk;

    /// <summary>
    /// DEF nền tảng tăng theo level.
    /// </summary>
    [BsonElement("base_def")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BaseDef { get; set; } = UserCollection.DefaultBaseDef;

    /// <summary>
    /// Bonus % ATK cộng dồn từ item booster.
    /// </summary>
    [BsonElement("bonus_atk_percent")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BonusAtkPercent { get; set; } = 0m;

    /// <summary>
    /// Bonus % DEF cộng dồn từ item booster.
    /// </summary>
    [BsonElement("bonus_def_percent")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BonusDefPercent { get; set; } = 0m;

    /// <summary>
    /// Tổng ATK hiển thị (base + bonus) để tối ưu truy vấn cũ.
    /// </summary>
    [BsonElement("atk")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Atk { get; set; } = UserCollection.DefaultBaseAtk;

    /// <summary>
    /// Tổng DEF hiển thị (base + bonus) để tối ưu truy vấn cũ.
    /// </summary>
    [BsonElement("def")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Def { get; set; } = UserCollection.DefaultBaseDef;

    /// <summary>
    /// Lịch sử roll chỉ số để audit progression.
    /// </summary>
    [BsonElement("stat_history")]
    [BsonIgnoreIfNull]
    public List<StatRollRecord>? StatHistory { get; set; }

    /// <summary>
    /// Soft-delete flag của bản ghi collection.
    /// </summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Mốc xóa mềm.
    /// </summary>
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Thời điểm tạo bản ghi.
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Thời điểm cập nhật gần nhất.
    /// </summary>
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Lần gần nhất card được rút trong session.
    /// </summary>
    [BsonElement("last_drawn_at")]
    public DateTime LastDrawnAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Thống kê tần suất card xuất hiện.
/// </summary>
public class DrawStats
{
    /// <summary>
    /// Số lần card xuất hiện ở trạng thái upright.
    /// </summary>
    [BsonElement("times_drawn_upright")]
    public int TimesDrawnUpright { get; set; }

    /// <summary>
    /// Số lần card xuất hiện ở trạng thái reversed.
    /// </summary>
    [BsonElement("times_drawn_reversed")]
    public int TimesDrawnReversed { get; set; }
}

/// <summary>
/// Cấu hình tùy biến hiển thị card.
/// </summary>
public class CardCustomization
{
    /// <summary>
    /// Tên ký hiệu cá nhân hiển thị trên card.
    /// </summary>
    [BsonElement("signature_name")]
    [BsonIgnoreIfNull]
    public string? SignatureName { get; set; }

    /// <summary>
    /// Skin đang được áp dụng cho card.
    /// </summary>
    [BsonElement("active_skin_id")]
    [BsonIgnoreIfNull]
    public string? ActiveSkinId { get; set; }
}

/// <summary>
/// Bản ghi lịch sử roll chỉ số theo từng cấp.
/// </summary>
public class StatRollRecord
{
    /// <summary>
    /// Cấp card tại thời điểm roll.
    /// </summary>
    [BsonElement("level")]
    public int Level { get; set; }

    /// <summary>
    /// Điểm cộng base ATK nhận được.
    /// </summary>
    [BsonElement("atk_bonus")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AtkBonus { get; set; }

    /// <summary>
    /// Điểm cộng base DEF nhận được.
    /// </summary>
    [BsonElement("def_bonus")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal DefBonus { get; set; }

    /// <summary>
    /// Mốc thời gian roll.
    /// </summary>
    [BsonElement("rolled_at")]
    public DateTime RolledAt { get; set; }
}
