using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Document yêu cầu đăng ký Reader của người dùng.
/// </summary>
public class ReaderRequestDocument
{
    /// <summary>
    /// ObjectId của yêu cầu.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// User gửi yêu cầu.
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái duyệt yêu cầu (pending/approved/rejected).
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    /// <summary>
    /// Lời giới thiệu Reader.
    /// </summary>
    [BsonElement("bio")]
    public string Bio { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách chuyên môn Reader đăng ký.
    /// </summary>
    [BsonElement("specialties")]
    public List<string> Specialties { get; set; } = [];

    /// <summary>
    /// Số năm kinh nghiệm đăng ký.
    /// </summary>
    [BsonElement("years_of_experience")]
    public int YearsOfExperience { get; set; }

    /// <summary>
    /// Link Facebook.
    /// </summary>
    [BsonElement("facebook_url")]
    [BsonIgnoreIfNull]
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// Link Instagram.
    /// </summary>
    [BsonElement("instagram_url")]
    [BsonIgnoreIfNull]
    public string? InstagramUrl { get; set; }

    /// <summary>
    /// Link TikTok.
    /// </summary>
    [BsonElement("tiktok_url")]
    [BsonIgnoreIfNull]
    public string? TikTokUrl { get; set; }

    /// <summary>
    /// Giá diamond cho mỗi câu hỏi.
    /// </summary>
    [BsonElement("diamond_per_question")]
    public long DiamondPerQuestion { get; set; }

    /// <summary>
    /// Danh sách tài liệu minh chứng.
    /// </summary>
    [BsonElement("proof_documents")]
    public List<string> ProofDocuments { get; set; } = [];

    /// <summary>
    /// Ghi chú xử lý từ admin (nếu có).
    /// </summary>
    [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

    /// <summary>
    /// Admin đã duyệt yêu cầu.
    /// </summary>
    [BsonElement("reviewed_by")]
    [BsonIgnoreIfNull]
    public string? ReviewedBy { get; set; }

    /// <summary>
    /// Mốc thời gian duyệt.
    /// </summary>
    [BsonElement("reviewed_at")]
    [BsonIgnoreIfNull]
    public DateTime? ReviewedAt { get; set; }

    /// <summary>
    /// Soft-delete flag.
    /// </summary>
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Mốc thời gian xóa mềm.
    /// </summary>
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Thời điểm tạo yêu cầu.
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Thời điểm cập nhật gần nhất.
    /// </summary>
    [BsonElement("updated_at")]
    [BsonIgnoreIfNull]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Lịch sử duyệt yêu cầu Reader theo từng lần xử lý.
    /// </summary>
    [BsonElement("review_history")]
    public List<ReaderRequestReviewHistoryEntryDocument> ReviewHistory { get; set; } = [];

    /// <summary>
    /// Version optimistic concurrency cho các thao tác review/update.
    /// </summary>
    [BsonElement("version")]
    public long Version { get; set; } = 1;
}

/// <summary>
/// Bản ghi lịch sử duyệt đơn Reader trong Mongo.
/// </summary>
public class ReaderRequestReviewHistoryEntryDocument
{
    /// <summary>
    /// Hành động admin thực hiện.
    /// </summary>
    [BsonElement("action")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái đơn sau khi xử lý.
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Định danh admin xử lý.
    /// </summary>
    [BsonElement("reviewed_by")]
    public string ReviewedBy { get; set; } = string.Empty;

    /// <summary>
    /// Ghi chú quyết định của admin.
    /// </summary>
    [BsonElement("admin_note")]
    [BsonIgnoreIfNull]
    public string? AdminNote { get; set; }

    /// <summary>
    /// Mốc thời gian xử lý.
    /// </summary>
    [BsonElement("reviewed_at")]
    public DateTime ReviewedAt { get; set; }
}
