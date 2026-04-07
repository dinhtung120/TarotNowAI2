

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
public class CardCatalogDocument
{
        [BsonId]
    public int Id { get; set; }

        [BsonElement("code")]
    public string Code { get; set; } = string.Empty;

        [BsonElement("name")]
    public LocalizedName Name { get; set; } = new();

        [BsonElement("arcana")]
    public string Arcana { get; set; } = string.Empty;

        [BsonElement("suit")]
    [BsonIgnoreIfNull]
    public string? Suit { get; set; }

        [BsonElement("number")]
    public int Number { get; set; }

        [BsonElement("element")]
    public string Element { get; set; } = string.Empty;

        [BsonElement("image_url")]
    [BsonIgnoreIfNull]
    public string? ImageUrl { get; set; }

        [BsonElement("meanings")]
    public CardMeanings Meanings { get; set; } = new();

        [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

[BsonIgnoreExtraElements]
public class LocalizedName
{
        [BsonElement("vi")] public string Vi { get; set; } = string.Empty;

        [BsonElement("en")] public string En { get; set; } = string.Empty;

        [BsonElement("zh")] public string Zh { get; set; } = string.Empty;
}

[BsonIgnoreExtraElements]
public class CardMeanings
{
        [BsonElement("upright")] public MeaningDetail Upright { get; set; } = new();

        [BsonElement("reversed")] public MeaningDetail Reversed { get; set; } = new();
}

[BsonIgnoreExtraElements]
public class MeaningDetail
{
        [BsonElement("keywords")] public List<string> Keywords { get; set; } = new();

        [BsonElement("description")] public string Description { get; set; } = string.Empty;
}
