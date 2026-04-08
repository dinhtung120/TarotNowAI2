

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document bài viết cộng đồng.
public class CommunityPostDocument
{
    // ObjectId của bài viết.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // User tác giả bài viết.
    [BsonElement("author_id")]
    public string AuthorId { get; set; } = string.Empty;

    // Tên hiển thị snapshot của tác giả khi đăng bài.
    [BsonElement("author_display_name")]
    public string AuthorDisplayName { get; set; } = string.Empty;

    // Avatar snapshot để hiển thị feed nhanh.
    [BsonElement("author_avatar_url")]
    [BsonIgnoreIfNull]
    public string? AuthorAvatarUrl { get; set; }

    // Nội dung bài viết.
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    // Quyền hiển thị bài (public/private...).
    [BsonElement("visibility")]
    public string Visibility { get; set; } = string.Empty;

    // Số lượng reaction theo từng loại để render tức thì.
    [BsonElement("reactions_count")]
    public Dictionary<string, int> ReactionsCount { get; set; } = new();

    // Tổng số reaction của bài.
    [BsonElement("total_reactions")]
    public int TotalReactions { get; set; } = 0;

    // Tổng số bình luận của bài.
    [BsonElement("comments_count")]
    public int CommentsCount { get; set; } = 0;

    // Soft-delete flag cho bài viết.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc thời gian bài bị xóa mềm.
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    // Tác nhân thực hiện xóa.
    [BsonElement("deleted_by")]
    [BsonIgnoreIfNull]
    public string? DeletedBy { get; set; }

    // Thời điểm tạo bài viết.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật gần nhất.
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}
