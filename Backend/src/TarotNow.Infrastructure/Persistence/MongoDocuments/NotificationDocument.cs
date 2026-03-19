/*
 * FILE: NotificationDocument.cs
 * MỤC ĐÍCH: Schema cho collection "notifications" (MongoDB). TTL 30 ngày tự xóa.
 *   Hỗ trợ đa ngôn ngữ (vi/en/zh) cho title và body.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// 1 thông báo in-app trong collection "notifications". TTL 30 ngày tự xóa bởi MongoDB.
/// </summary>
public class NotificationDocument
{
    /// <summary>ID duy nhất (ObjectId tự sinh).</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>UUID User nhận thông báo (từ PostgreSQL users table).</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>Tiêu đề đa ngôn ngữ (vi/en/zh). Hiển thị dạng bold trên UI.</summary>
    [BsonElement("title")]
    public LocalizedText Title { get; set; } = new();

    /// <summary>Nội dung chi tiết đa ngôn ngữ. Hiển thị bên dưới tiêu đề.</summary>
    [BsonElement("body")]
    public LocalizedText Body { get; set; } = new();

    /// <summary>
    /// Loại thông báo: "quest" (nhiệm vụ), "system" (hệ thống), "streak" (chuỗi), "escrow" (tài chính).
    /// Quyết định icon và màu trên UI.
    /// </summary>
    [BsonElement("type")]
    public string Type { get; set; } = "system";

    /// <summary>Đã đọc chưa — dùng tính badge count (số đỏ) trên icon chuông.</summary>
    [BsonElement("is_read")]
    public bool IsRead { get; set; } = false;

    /// <summary>
    /// Dữ liệu bổ sung dạng JSON tự do (khác nhau tùy loại thông báo).
    /// Ví dụ: { "quest_code": "daily_reading", "deep_link": "/history/abc" }.
    /// Dùng BsonDocument vì mỗi loại notification có metadata khác nhau.
    /// </summary>
    [BsonElement("metadata")]
    [BsonIgnoreIfNull]
    public BsonDocument? Metadata { get; set; }

    /// <summary>
    /// Thời điểm tạo (UTC). TTL Index nhắm vào trường này: sau 30 ngày → MongoDB TỰ ĐỘNG XÓA.
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Văn bản đa ngôn ngữ (vi/en/zh). Fallback: locale → en nếu locale rỗng.
/// </summary>
public class LocalizedText
{
    [BsonElement("vi")] public string Vi { get; set; } = string.Empty;
    [BsonElement("en")] public string En { get; set; } = string.Empty;
    [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}
