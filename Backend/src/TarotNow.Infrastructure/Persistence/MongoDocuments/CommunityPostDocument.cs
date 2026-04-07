

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public class CommunityPostDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

        [BsonElement("author_id")]
    public string AuthorId { get; set; } = string.Empty;

        [BsonElement("author_display_name")]
    public string AuthorDisplayName { get; set; } = string.Empty;

        [BsonElement("author_avatar_url")]
    [BsonIgnoreIfNull]
    public string? AuthorAvatarUrl { get; set; }

        [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

        [BsonElement("visibility")]
    public string Visibility { get; set; } = string.Empty;

        [BsonElement("reactions_count")]
    public Dictionary<string, int> ReactionsCount { get; set; } = new();

        [BsonElement("total_reactions")]
    public int TotalReactions { get; set; } = 0;

        [BsonElement("comments_count")]
    public int CommentsCount { get; set; } = 0;

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

        [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

        [BsonElement("deleted_by")]
    [BsonIgnoreIfNull]
    public string? DeletedBy { get; set; }

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}
