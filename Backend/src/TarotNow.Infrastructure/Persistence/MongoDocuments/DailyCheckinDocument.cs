using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document chuyên biệt chứa mốc sự kiện người chơi nhận Gold mỗi ngày qua tính năng Daily Check-in.
/// Được cấu hình index ở tầng Context (UserId + BusinessDate) để đảm bảo 1 ngày người dùng chỉ thu hoạch 1 lần duy nhất (idempotent).
/// </summary>
public class DailyCheckinDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Guid chuyển thành String dễ xử lý, tham chiếu tới User.
    [BsonElement("userId")]
    public string UserId { get; set; } = string.Empty;

    // Ngày nghiệp vụ (UTC Date), khoá chốt cho unique constraint. 
    // MongoDB Serialize DateOnly qua String "yyyy-MM-dd" để an toàn tìm kiếm.
    [BsonElement("businessDate")]
    public string BusinessDate { get; set; } = string.Empty;

    // Lượng Vàng đã phân phát tự động dội vào ví.
    [BsonElement("goldReward")]
    public long GoldReward { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
