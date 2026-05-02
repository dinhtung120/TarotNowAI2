using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

[BsonIgnoreExtraElements]
/// <summary>
/// Document lưu đánh giá reader theo conversation completed.
/// </summary>
public class ConversationReviewDocument
{
    /// <summary>
    /// Định danh review.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// Định danh conversation được đánh giá.
    /// </summary>
    [BsonElement("conversation_id")]
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// Định danh user gửi đánh giá.
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Định danh reader nhận đánh giá.
    /// </summary>
    [BsonElement("reader_id")]
    public string ReaderId { get; set; } = string.Empty;

    /// <summary>
    /// Điểm sao user chấm.
    /// </summary>
    [BsonElement("rating")]
    public int Rating { get; set; }

    /// <summary>
    /// Nhận xét tùy chọn.
    /// </summary>
    [BsonElement("comment")]
    [BsonIgnoreIfNull]
    public string? Comment { get; set; }

    /// <summary>
    /// Thời điểm tạo review.
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Thời điểm cập nhật cuối.
    /// </summary>
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}
