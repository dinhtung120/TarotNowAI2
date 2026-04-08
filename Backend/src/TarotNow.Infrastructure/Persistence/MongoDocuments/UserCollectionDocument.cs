

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document thẻ bài người dùng sở hữu trong bộ sưu tập.
public class UserCollectionDocument
{
    // ObjectId của bản ghi collection.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User sở hữu thẻ bài.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Id card trong catalog.
    [BsonElement("card_id")]
    public int CardId { get; set; }

    // Cấp hiện tại của card.
    [BsonElement("level")]
    public int Level { get; set; } = 1;

    // Điểm kinh nghiệm hiện có của card.
    [BsonElement("exp")]
    public long Exp { get; set; } = 0;

    // Bậc ascension của card.
    [BsonElement("ascension_tier")]
    public int AscensionTier { get; set; } = 0;

    // Thống kê số lần rút upright/reversed.
    [BsonElement("stats")]
    public DrawStats Stats { get; set; } = new();

    // Tùy biến giao diện của card (nếu có).
    [BsonElement("customization")]
    [BsonIgnoreIfNull]
    public CardCustomization? Customization { get; set; }

    // Chỉ số tấn công đã tính sau nâng cấp.
    [BsonElement("atk")]
    public int Atk { get; set; } = 10;

    // Chỉ số phòng thủ đã tính sau nâng cấp.
    [BsonElement("def")]
    public int Def { get; set; } = 10;

    // Lịch sử roll chỉ số để audit progression.
    [BsonElement("stat_history")]
    [BsonIgnoreIfNull]
    public List<StatRollRecord>? StatHistory { get; set; }

    // Soft-delete flag của bản ghi collection.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc xóa mềm.
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    // Thời điểm tạo bản ghi.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật gần nhất.
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Lần gần nhất card được rút trong session.
    [BsonElement("last_drawn_at")]
    public DateTime LastDrawnAt { get; set; } = DateTime.UtcNow;
}

// Thống kê tần suất card xuất hiện.
public class DrawStats
{
    // Số lần card xuất hiện ở trạng thái upright.
    [BsonElement("times_drawn_upright")] public int TimesDrawnUpright { get; set; }
    // Số lần card xuất hiện ở trạng thái reversed.
    [BsonElement("times_drawn_reversed")] public int TimesDrawnReversed { get; set; }
}

// Cấu hình tùy biến hiển thị card.
public class CardCustomization
{
    // Tên ký hiệu cá nhân hiển thị trên card.
    [BsonElement("signature_name")][BsonIgnoreIfNull] public string? SignatureName { get; set; }
    // Skin đang được áp dụng cho card.
    [BsonElement("active_skin_id")][BsonIgnoreIfNull] public string? ActiveSkinId { get; set; }
}

// Bản ghi lịch sử roll chỉ số theo từng cấp.
public class StatRollRecord
{
    // Cấp card tại thời điểm roll.
    [BsonElement("level")]
    public int Level { get; set; }

    // Điểm cộng attack nhận được.
    [BsonElement("atk_bonus")]
    public int AtkBonus { get; set; }

    // Điểm cộng defense nhận được.
    [BsonElement("def_bonus")]
    public int DefBonus { get; set; }

    // Mốc thời gian roll.
    [BsonElement("rolled_at")]
    public DateTime RolledAt { get; set; }
}
