using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

public partial class ChatMessageDocument
{
    [BsonElement("media_payload")]
    [BsonIgnoreIfNull]
    public ChatMediaPayload? MediaPayload { get; set; }
}

public class ChatMediaPayload
{
    [BsonElement("url")]
    public string Url { get; set; } = string.Empty;

    [BsonElement("mime_type")]
    public string? MimeType { get; set; }

    [BsonElement("size_bytes")]
    public long? SizeBytes { get; set; }

    [BsonElement("duration_ms")]
    public int? DurationMs { get; set; }

    [BsonElement("width")]
    public int? Width { get; set; }

    [BsonElement("height")]
    public int? Height { get; set; }

    [BsonElement("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("processing_status")]
    public string? ProcessingStatus { get; set; }
}
