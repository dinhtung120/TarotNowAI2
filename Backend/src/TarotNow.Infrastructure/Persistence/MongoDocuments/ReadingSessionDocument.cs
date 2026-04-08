

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

// Document phiên đọc bài tarot được lưu trên Mongo.
public class ReadingSessionDocument
{
    // Khóa phiên đọc (ObjectId) dùng cho truy vết nghiệp vụ.
    [BsonId]
    public object Id { get; set; } = ObjectId.GenerateNewId();

    // User sở hữu phiên đọc.
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    // Loại phiên đọc (personal/love/career...).
    [BsonElement("session_type")]
    public string SessionType { get; set; } = "personal";

    // Kiểu trải bài đã chọn.
    [BsonElement("spread_type")]
    public string SpreadType { get; set; } = string.Empty;

    // Câu hỏi gốc người dùng cung cấp.
    [BsonElement("question")]
    [BsonIgnoreIfNull]
    public string? Question { get; set; }

    // Danh sách lá bài đã rút cho phiên đọc.
    [BsonElement("drawn_cards")]
    public List<DrawnCard> DrawnCards { get; set; } = new();

    // Trạng thái xử lý AI của phiên đọc.
    [BsonElement("ai_status")]
    public string AiStatus { get; set; } = "pending";

    // Kết quả trả lời từ AI (nếu đã hoàn tất).
    [BsonElement("ai_result")]
    [BsonIgnoreIfNull]
    public AiResult? AiResult { get; set; }

    // Thông tin locale request/response để theo dõi fallback ngôn ngữ.
    [BsonElement("locale_info")]
    [BsonIgnoreIfNull]
    public LocaleInfo? LocaleInfo { get; set; }

    // Danh sách câu hỏi follow-up trong cùng phiên.
    [BsonElement("followups")]
    public List<FollowupEntry> Followups { get; set; } = new();

    // Thông tin chi phí phiên đọc.
    [BsonElement("cost")]
    [BsonIgnoreIfNull]
    public SessionCost? Cost { get; set; }

    // Thông tin hoàn tiền nếu có lỗi xử lý.
    [BsonElement("refund")]
    [BsonIgnoreIfNull]
    public RefundInfo? Refund { get; set; }

    // Soft-delete flag.
    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    // Mốc thời gian xóa mềm.
    [BsonElement("deleted_at")]
    [BsonIgnoreIfNull]
    public DateTime? DeletedAt { get; set; }

    // Thời điểm tạo phiên đọc.
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật phiên đọc gần nhất.
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

// Thông tin một lá bài đã rút trong spread.
public class DrawnCard
{
    // Id lá bài trong catalog.
    [BsonElement("card_id")] public int CardId { get; set; }
    // Vị trí lá bài trong spread.
    [BsonElement("position")] public int Position { get; set; }
    // Cờ lá bài bị ngược.
    [BsonElement("is_reversed")] public bool IsReversed { get; set; }
    // Level lá bài tại thời điểm đọc để cố định ngữ cảnh diễn giải.
    [BsonElement("card_level_at_reading")] public int CardLevelAtReading { get; set; } = 1;
}

// Kết quả diễn giải từ AI.
public class AiResult
{
    // Nội dung diễn giải chính.
    [BsonElement("summary")] public string Summary { get; set; } = string.Empty;
    // Bản tóm tắt rút gọn cho chia sẻ.
    [BsonElement("share_summary")][BsonIgnoreIfNull] public string? ShareSummary { get; set; }
    // Câu hỏi follow-up được AI gợi ý.
    [BsonElement("suggested_followup")][BsonIgnoreIfNull] public string? SuggestedFollowup { get; set; }
}

// Thông tin locale của request/response AI.
public class LocaleInfo
{
    // Locale người dùng yêu cầu.
    [BsonElement("requested_locale")] public string RequestedLocale { get; set; } = "vi";
    // Locale AI thực tế trả về.
    [BsonElement("returned_locale")] public string ReturnedLocale { get; set; } = "vi";
    // Lý do fallback locale nếu có.
    [BsonElement("fallback_reason")][BsonIgnoreIfNull] public string? FallbackReason { get; set; }
}

// Một câu hỏi follow-up phát sinh trong phiên đọc.
public class FollowupEntry
{
    // Thứ tự follow-up trong phiên.
    [BsonElement("sequence")] public int Sequence { get; set; }
    // Nội dung câu hỏi follow-up.
    [BsonElement("question")] public string Question { get; set; } = string.Empty;
    // Câu trả lời follow-up từ AI.
    [BsonElement("answer")] public string Answer { get; set; } = string.Empty;
    // Chi phí diamond đã thu.
    [BsonElement("cost_diamond")] public long CostDiamond { get; set; }
    // Chi phí gold đã thu (nếu áp dụng).
    [BsonElement("cost_gold")] public long CostGold { get; set; } = 0;
    // Đánh dấu follow-up miễn phí theo cấp user.
    [BsonElement("is_free_by_level")] public bool IsFreeByLevel { get; set; }
    // Thời điểm tạo follow-up entry.
    [BsonElement("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// Tổng quan chi phí của phiên đọc.
public class SessionCost
{
    // Đơn vị tiền tệ của khoản phí.
    [BsonElement("currency")] public string Currency { get; set; } = string.Empty;
    // Giá trị phí đã thu.
    [BsonElement("amount")] public long Amount { get; set; }
}

// Thông tin hoàn tiền khi phiên gặp lỗi.
public class RefundInfo
{
    // Cờ đã hoàn tiền hay chưa.
    [BsonElement("refunded")] public bool Refunded { get; set; }
    // Tham chiếu giao dịch ví hoàn tiền.
    [BsonElement("wallet_tx_ref")][BsonIgnoreIfNull] public string? WalletTxRef { get; set; }
    // Mốc thời gian hoàn tiền.
    [BsonElement("refunded_at")][BsonIgnoreIfNull] public DateTime? RefundedAt { get; set; }
}
