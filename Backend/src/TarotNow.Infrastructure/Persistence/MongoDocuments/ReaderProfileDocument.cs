

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
public class ReaderProfileDocument
{
        [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

        [BsonElement("status")]
    public string Status { get; set; } = "offline";

        [BsonElement("pricing")]
    public ReaderPricing Pricing { get; set; } = new();

        [BsonElement("bio")]
    public LocalizedText Bio { get; set; } = new();

        [BsonElement("specialties")]
    public List<string> Specialties { get; set; } = new();

        [BsonElement("stats")]
    public ReaderStats Stats { get; set; } = new();

        [BsonElement("badges")]
    public List<string> Badges { get; set; } = new();

        [BsonElement("title_ref")]
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TitleRef { get; set; }

        [BsonElement("display_name")]
    public string DisplayName { get; set; } = string.Empty;

        [BsonElement("avatar_url")]
    [BsonIgnoreIfNull]
    public string? AvatarUrl { get; set; }

        [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}

[BsonIgnoreExtraElements]
public class ReaderPricing
{
        [BsonElement("diamond_per_question")]
    public long DiamondPerQuestion { get; set; } = 5;
}

[BsonIgnoreExtraElements]
public class ReaderStats
{
        [BsonElement("avg_rating")]
    public double AvgRating { get; set; } = 0;

        [BsonElement("total_reviews")]
    public int TotalReviews { get; set; } = 0;
}
