using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// MongoDB Document cho collection "notifications" — Thông báo in-app.
///
/// TTL 30 ngày — MongoDB tự động xóa document khi created_at quá 30 ngày.
/// TTL index: created_at + expireAfterSeconds: 2,592,000 (xem init.js dòng 190).
///
/// Hỗ trợ đa ngôn ngữ (vi/en/zh) cho title và body.
/// </summary>
public class NotificationDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>UUID user nhận thông báo.</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>Tiêu đề đa ngôn ngữ.</summary>
    [BsonElement("title")]
    public LocalizedText Title { get; set; } = new();

    /// <summary>Nội dung đa ngôn ngữ.</summary>
    [BsonElement("body")]
    public LocalizedText Body { get; set; } = new();

    /// <summary>
    /// Loại thông báo: quest, system, streak, escrow, ...
    /// Dùng để filter và icon trên UI.
    /// </summary>
    [BsonElement("type")]
    public string Type { get; set; } = "system";

    /// <summary>Đã đọc hay chưa — dùng cho badge count trên UI.</summary>
    [BsonElement("is_read")]
    public bool IsRead { get; set; } = false;

    /// <summary>
    /// Dữ liệu bổ sung — VD: deep link URL, quest_code, reading_session_id.
    /// Flexible object cho mỗi loại notification khác nhau.
    /// </summary>
    [BsonElement("metadata")]
    [BsonIgnoreIfNull]
    public BsonDocument? Metadata { get; set; }

    /// <summary>
    /// Thời điểm tạo — TTL index dựa trên field này (30 ngày auto-delete).
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>Text đa ngôn ngữ — vi/en/zh với fallback chain.</summary>
public class LocalizedText
{
    [BsonElement("vi")] public string Vi { get; set; } = string.Empty;
    [BsonElement("en")] public string En { get; set; } = string.Empty;
    [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}
