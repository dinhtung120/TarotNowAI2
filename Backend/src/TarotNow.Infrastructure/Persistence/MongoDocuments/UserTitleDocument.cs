/*
 * FILE: UserTitleDocument.cs
 * MỤC ĐÍCH: Lưu trữ danh sách các danh hiệu mà user đang sở hữu.
 *   - Idempotency: Unique Index (UserId, TitleCode).
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Ghi nhận 1 Title được sở hữu bởi 1 User. (Bảng user_titles).
/// </summary>
public class UserTitleDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [BsonElement("title_code")]
    public string TitleCode { get; set; } = string.Empty;

    [BsonElement("granted_at")]
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}
