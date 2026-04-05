/*
 * ===================================================================
 * FILE: CommunityPostDocument.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.MongoDocuments
 * ===================================================================
 * MỤC ĐÍCH:
 *   Schema cho collection "community_posts" trong MongoDB.
 *   Lưu trữ các bài luận/status chia sẻ trải nghiệm Tarot từ người dùng.
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Tài liệu (Document) đại diện cho một bài viết cộng đồng.
/// </summary>
public class CommunityPostDocument
{
    /// <summary>ObjectId tự sinh từ MongoDB.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>UUID của tác giả (Tách từ User JWT).</summary>
    [BsonElement("author_id")]
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>Tên hiển thị của tác giả để load feed cho nhanh, tránh join.</summary>
    [BsonElement("author_display_name")]
    public string AuthorDisplayName { get; set; } = string.Empty;

    /// <summary>Avatar của tác giả (có thể null).</summary>
    [BsonElement("author_avatar_url")]
    [BsonIgnoreIfNull]
    public string? AuthorAvatarUrl { get; set; }

    /// <summary>Nội dung chia sẻ (Rich text hoặc Markdown).</summary>
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>Tầm nhìn bài viết: public / friends_only / private.</summary>
    [BsonElement("visibility")]
    public string Visibility { get; set; } = string.Empty;

    /// <summary>
    /// Thống kê lượng tương tác theo loại (vd: { "like": 10, "love": 5 }).
    /// Lưu dạng từ điển chìa khóa - ổ khóa để dễ bề Update.
    /// </summary>
    [BsonElement("reactions_count")]
    public Dictionary<string, int> ReactionsCount { get; set; } = new();

    /// <summary>Tổng lượng biểu cảm đã có cho dễ sort.</summary>
    [BsonElement("total_reactions")]
    public int TotalReactions { get; set; } = 0;

    /// <summary>Số lượng bình luận.</summary>
    [BsonElement("comments_count")]
    public int CommentsCount { get; set; } = 0;

    /// <summary>Cờ lốc xoáy (Đã xóa nhưng vẫn giữ lại để Audit nếu cần).</summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    /// <summary>Thời gian xóa bài.</summary>
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    /// <summary>Người thực hiện xoay tay tiễn bài về cõi vĩnh hằng (Author hoặc Admin).</summary>
    [BsonElement("deleted_by")]
    [BsonIgnoreIfNull]
    public string? DeletedBy { get; set; }

    /// <summary>Thời điểm lên sóng.</summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>Thời điểm sửa đổi gần nhất.</summary>
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }
}
