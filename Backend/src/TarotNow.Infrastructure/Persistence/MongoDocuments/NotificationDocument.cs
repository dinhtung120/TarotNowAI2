

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document thông báo in-app gửi tới người dùng.
public class NotificationDocument
{
    // ObjectId của thông báo.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User nhận thông báo.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Tiêu đề thông báo đa ngôn ngữ.
    [BsonElement("title")]
    public LocalizedText Title { get; set; } = new();

    // Nội dung thông báo đa ngôn ngữ.
    [BsonElement("body")]
    public LocalizedText Body { get; set; } = new();

    // Loại thông báo để client quyết định icon/điều hướng.
    [BsonElement("type")]
    public string Type { get; set; } = "system";

    // Cờ đã đọc/chưa đọc.
    [BsonElement("is_read")]
    public bool IsRead { get; set; } = false;

    // Metadata động cho deeplink/params bổ sung.
    [BsonElement("metadata")]
    [BsonIgnoreIfNull]
    public BsonDocument? Metadata { get; set; }

    // Thời điểm tạo thông báo.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// Cấu trúc text theo locale.
public class LocalizedText
{
    // Nội dung tiếng Việt.
    [BsonElement("vi")] public string Vi { get; set; } = string.Empty;
    // Nội dung tiếng Anh.
    [BsonElement("en")] public string En { get; set; } = string.Empty;
    // Nội dung tiếng Trung.
    [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}
