using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Partial mở rộng message với payload media.
public partial class ChatMessageDocument
{
    // Metadata media đính kèm, chỉ có khi message type là media.
    [BsonElement("media_payload")]
    [BsonIgnoreIfNull]
    public ChatMediaPayload? MediaPayload { get; set; }
}

// Payload thông tin media phục vụ render và xử lý async.
public class ChatMediaPayload
{
    // URL file media gốc.
    [BsonElement("url")]
    public string Url { get; set; } = string.Empty;

    // MIME type của media.
    [BsonElement("mime_type")]
    public string? MimeType { get; set; }

    // Kích thước file theo byte.
    [BsonElement("size_bytes")]
    public long? SizeBytes { get; set; }

    // Thời lượng media (video/audio), đơn vị ms.
    [BsonElement("duration_ms")]
    public int? DurationMs { get; set; }

    // Chiều rộng media (px).
    [BsonElement("width")]
    public int? Width { get; set; }

    // Chiều cao media (px).
    [BsonElement("height")]
    public int? Height { get; set; }

    // URL thumbnail để tải nhanh trong chat list.
    [BsonElement("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    // Caption/miêu tả người dùng nhập.
    [BsonElement("description")]
    public string? Description { get; set; }

    // Trạng thái xử lý media (pending/ready/failed...).
    [BsonElement("processing_status")]
    public string? ProcessingStatus { get; set; }
}
