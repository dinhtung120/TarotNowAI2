using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document bình luận trong module cộng đồng.
public class CommunityCommentDocument
{
    // ObjectId của bình luận.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Bài viết cha của bình luận.
    [BsonElement("post_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = string.Empty;

    // User tác giả bình luận.
    [BsonElement("author_id")]
    public string AuthorId { get; set; } = string.Empty;

    // Display name tại thời điểm ghi nhận để tránh lệch khi profile đổi tên.
    [BsonElement("author_display_name")]
    public string AuthorDisplayName { get; set; } = string.Empty;

    // Avatar snapshot phục vụ render feed nhanh.
    [BsonElement("author_avatar_url")]
    [BsonIgnoreIfNull]
    public string? AuthorAvatarUrl { get; set; }

    // Nội dung bình luận.
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    // Soft-delete flag.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc thời gian bị xóa mềm.
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    // Tác nhân xóa bình luận (admin/system/user).
    [BsonElement("deleted_by")]
    [BsonIgnoreIfNull]
    public string? DeletedBy { get; set; }

    // Thời điểm tạo bình luận.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
}
