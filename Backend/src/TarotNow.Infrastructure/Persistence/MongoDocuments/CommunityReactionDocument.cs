

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document reaction của người dùng trên bài viết cộng đồng.
public class CommunityReactionDocument
{
    // ObjectId của reaction.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Bài viết được react.
    [BsonElement("post_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = string.Empty;

    // User thực hiện reaction.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Loại reaction (like/love...).
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    // Thời điểm tạo reaction.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
}
