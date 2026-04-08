

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
// Document log từng lượt quay gacha.
public class GachaLogDocument
{
    // ObjectId của bản ghi quay thưởng.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // User thực hiện lượt quay.
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    // Banner đã quay.
    [BsonElement("banner_code")]
    public string BannerCode { get; set; } = string.Empty;

    // Rarity nhận được.
    [BsonElement("rarity")]
    public string Rarity { get; set; } = string.Empty;

    // Loại phần thưởng trả về.
    [BsonElement("reward_type")]
    public string RewardType { get; set; } = string.Empty;

    // Giá trị phần thưởng (item id/title code/amount...).
    [BsonElement("reward_value")]
    public string RewardValue { get; set; } = string.Empty;

    // Số diamond đã tiêu cho lượt quay.
    [BsonElement("spent_diamond")]
    public long SpentDiamond { get; set; }

    // Đánh dấu lượt quay có kích hoạt pity hay không.
    [BsonElement("was_pity")]
    public bool WasPity { get; set; }

    // Seed RNG lưu cho mục đích audit (nếu policy cho phép lưu).
    [BsonElement("rng_seed")]
    public string? RngSeed { get; set; }

    // Thời điểm tạo log, chuẩn UTC để TTL hoạt động chính xác.
    [BsonElement("created_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }
}
