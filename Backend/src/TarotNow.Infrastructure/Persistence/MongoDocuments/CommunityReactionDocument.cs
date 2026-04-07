

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class CommunityReactionDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

        [BsonElement("post_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = string.Empty;

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
}
