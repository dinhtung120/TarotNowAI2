using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document ghi nhận điểm danh hằng ngày của người dùng.
public class DailyCheckinDocument
{
    // ObjectId của bản ghi check-in.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // User thực hiện check-in.
    [BsonElement("userId")]
    public string UserId { get; set; } = string.Empty;

    // Khóa ngày nghiệp vụ (yyyyMMdd theo timezone chuẩn hệ thống).
    [BsonElement("businessDate")]
    public string BusinessDate { get; set; } = string.Empty;

    // Số vàng thưởng từ lần check-in này.
    [BsonElement("goldReward")]
    public long GoldReward { get; set; }

    // Thời điểm tạo bản ghi check-in.
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
