/*
 * ===================================================================
 * FILE: CommunityReactionDocument.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.MongoDocuments
 * ===================================================================
 * MỤC ĐÍCH:
 *   Schema cho collection "community_reactions" trong MongoDB.
 *   Lưu vết từng lần vớt cảm xúc (react) của một user cho một bài viết.
 *   Phải có ràng buộc Unique Index để bảo đảm tính chất Idempotent (Chống spam thả reaction).
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Tài liệu (Document) đại diện cho một cú thả cảm xúc của một user trên một bài viết.
/// </summary>
public class CommunityReactionDocument
{
    /// <summary>Vạn vật sinh ra đều từ Cát Bụi, từ ID tự cấp của Mongo.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Liên kết huyết thống với CommunityPost (ObjectId).</summary>
    [BsonElement("post_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = string.Empty;

    /// <summary>Người đã phát tâm thả thính (UUID từ Identity Provider).</summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>Loại tình cảm: like / love / haha / sad / insightful.</summary>
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Khoảnh khắc đong đầy cõi tạm.</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
}
