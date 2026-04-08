

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document danh hiệu đã được cấp cho người dùng.
public class UserTitleDocument
{
    // ObjectId của bản ghi cấp danh hiệu.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // User nhận danh hiệu.
    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    // Mã danh hiệu đã cấp.
    [BsonElement("title_code")]
    public string TitleCode { get; set; } = string.Empty;

    // Mốc thời gian cấp danh hiệu.
    [BsonElement("granted_at")]
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}
