/*
 * ===================================================================
 * FILE: AiProviderLogDocument.cs
 * NAMESPACE: TarotNow.Infrastructure.Persistence.MongoDocuments
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa "bản thiết kế" (schema) cho Collection "ai_provider_logs" trên MongoDB.
 *   Mỗi khi hệ thống gọi tới AI (ví dụ: OpenAI, Grok) để xin kết quả bói bài,
 *   hệ thống sẽ tạo 1 "tờ phiếu ghi chép" (document) theo cấu trúc này, lưu lại:
 *     - Ai gọi (UserId), gọi model nào (Model), tốn bao nhiêu token (Tokens),
 *       phản hồi mất bao lâu (LatencyMs), có lỗi không (ErrorCode), v.v.
 *
 *   TẠI SAO DÙNG MONGODB CHỨ KHÔNG LƯU VÀO POSTGRESQL?
 *   → Mỗi lần gọi AI = 1 dòng log. Số lượng log rất KHỔNG LỒ (hàng triệu dòng/tháng).
 *   → Cấu trúc log linh hoạt (có thể thêm trường mới mà không cần migration database).
 *   → MongoDB hỗ trợ TTL Index: tự động xóa log cũ hơn 90 ngày → tiết kiệm ổ cứng.
 *   → PostgreSQL không có tính năng tự xóa dữ liệu cũ kiểu này.
 *
 *   VÒNG ĐỜI CỦA 1 DOCUMENT:
 *   → Tạo ra khi bắt đầu gọi AI (status = "requested")
 *   → Cập nhật khi AI trả lời xong (status = "completed") hoặc lỗi (status = "failed")
 *   → Sau 90 ngày: MongoDB tự động xóa nhờ TTL Index trên trường created_at.
 * ===================================================================
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TarotNow.Infrastructure.Persistence.MongoDocuments;

/// <summary>
/// Lớp C# đại diện cho 1 Document (1 dòng dữ liệu) trong collection "ai_provider_logs" trên MongoDB.
/// Mỗi object của class này = 1 lần hệ thống gọi AI Provider (OpenAI, Grok, v.v.).
///
/// Tại sao tạo class riêng (POCO) thay vì dùng trực tiếp Entity bên Domain?
/// → Theo nguyên tắc Clean Architecture: Domain Layer không được phụ thuộc vào thư viện MongoDB.
///   Các attribute [BsonElement], [BsonId] thuộc về thư viện MongoDB.Driver, chỉ nên nằm ở Infrastructure.
/// </summary>
public class AiProviderLogDocument
{
    /// <summary>
    /// ID duy nhất của document trong MongoDB.
    /// [BsonId] = đánh dấu đây là trường khóa chính (_id) trong MongoDB.
    /// [BsonRepresentation(BsonType.ObjectId)] = khi lưu vào DB sẽ là kiểu ObjectId (chuỗi hex 24 ký tự),
    ///   nhưng trong code C# ta giữ kiểu string cho dễ xử lý.
    /// Mặc định tự sinh ID mới khi tạo object → không cần backend lo generate.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// Mã UUID của người dùng đã kích hoạt lần gọi AI này.
    /// UUID này trùng khớp với cột Id trong bảng "users" bên PostgreSQL.
    /// Lưu dạng string vì MongoDB không có kiểu UUID gốc (khác PostgreSQL).
    /// </summary>
    [BsonElement("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Tham chiếu tới phiên đọc bài (reading_sessions._id) trên MongoDB.
    /// Dạng ObjectId string. Có thể null nếu lần gọi AI này không liên quan tới phiên đọc bài nào
    /// (ví dụ: gọi AI cho mục đích khác).
    /// [BsonIgnoreIfNull] = nếu giá trị là null thì KHÔNG ghi trường này vào MongoDB → tiết kiệm dung lượng.
    /// </summary>
    [BsonElement("reading_ref")]
    [BsonIgnoreIfNull]
    public string? ReadingRef { get; set; }

    /// <summary>
    /// Tham chiếu tới bản ghi ai_requests.id bên PostgreSQL (UUID).
    /// Đây là cầu nối "Cross-Database" (liên kết giữa MongoDB và PostgreSQL).
    /// Khi cần đối chiếu: lấy AiRequestRef → tra bảng ai_requests bên PostgreSQL.
    /// </summary>
    [BsonElement("ai_request_ref")]
    [BsonIgnoreIfNull]
    public string? AiRequestRef { get; set; }

    /// <summary>
    /// Tên model AI đã được gọi. Ví dụ: "grok-3", "gpt-4o", "claude-3.5-sonnet".
    /// Giúp team theo dõi đang dùng model nào nhiều nhất, model nào hay lỗi.
    /// </summary>
    [BsonElement("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Thống kê số token đã tiêu thụ (đầu vào + đầu ra).
    /// Token là đơn vị tính tiền của các AI provider (càng nhiều token → càng tốn tiền).
    /// Dùng class con TokenUsage để gom gọn 2 trường input/output.
    /// </summary>
    [BsonElement("tokens")]
    public TokenUsage Tokens { get; set; } = new();

    /// <summary>
    /// Thời gian AI mất để phản hồi, tính bằng mili-giây (ms).
    /// Ví dụ: 1500 = 1.5 giây. Dùng để giám sát hiệu năng AI provider.
    /// Nếu latency quá cao → cần xem xét chuyển sang provider khác hoặc tối ưu prompt.
    /// </summary>
    [BsonElement("latency_ms")]
    public int LatencyMs { get; set; }

    /// <summary>
    /// Phiên bản của prompt template đang dùng (ví dụ: "v2.3").
    /// Khi thay đổi prompt, ta tăng version để có thể so sánh chất lượng kết quả giữa các phiên bản.
    /// </summary>
    [BsonElement("prompt_version")]
    [BsonIgnoreIfNull]
    public string? PromptVersion { get; set; }

    /// <summary>
    /// Phiên bản của content policy (chính sách nội dung) đang áp dụng.
    /// Content policy quyết định AI được/không được nói gì (ví dụ: không khuyên về y tế, tài chính).
    /// </summary>
    [BsonElement("policy_version")]
    [BsonIgnoreIfNull]
    public string? PolicyVersion { get; set; }

    /// <summary>
    /// Trạng thái của lần gọi AI:
    ///   - "requested": đã gửi yêu cầu, đang chờ AI trả lời
    ///   - "completed": AI đã trả lời thành công
    ///   - "failed": gọi AI thất bại (timeout, lỗi mạng, API key hết hạn, v.v.)
    /// Mặc định là "requested" khi mới tạo document.
    /// </summary>
    [BsonElement("status")]
    public string Status { get; set; } = "requested";

    /// <summary>
    /// Mã lỗi cụ thể nếu status = "failed" (ví dụ: "TIMEOUT", "RATE_LIMITED", "INVALID_API_KEY").
    /// Null nếu thành công. Giúp team debug và thiết lập alert khi có lỗi lặp lại nhiều.
    /// </summary>
    [BsonElement("error_code")]
    [BsonIgnoreIfNull]
    public string? ErrorCode { get; set; }

    /// <summary>
    /// OpenTelemetry Trace ID — mã theo dõi phân tán giữa các microservice.
    /// Khi có bug, dùng trace_id này paste vào tool Jaeger/Zipkin để xem toàn bộ chuỗi request
    /// đã đi qua những service nào, mất bao lâu ở mỗi bước.
    /// </summary>
    [BsonElement("trace_id")]
    [BsonIgnoreIfNull]
    public string? TraceId { get; set; }

    /// <summary>
    /// Thời điểm tạo log (UTC). Đây là trường được TTL Index nhắm tới:
    /// Sau 90 ngày kể từ thời điểm này, MongoDB sẽ TỰ ĐỘNG XÓA document này.
    /// Mặc định = DateTime.UtcNow (thời điểm hiện tại theo giờ UTC).
    /// </summary>
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Class con lưu thống kê token: bao nhiêu token đầu vào (prompt) và bao nhiêu token đầu ra (completion).
/// Tách riêng class vì trong MongoDB, trường "tokens" là 1 object lồng (nested object).
/// Ví dụ trong DB: { "tokens": { "in": 500, "out": 1200 } }
/// </summary>
public class TokenUsage
{
    /// <summary>Số token đầu vào (prompt gửi cho AI). [BsonElement("in")] = tên trường trong MongoDB là "in".</summary>
    [BsonElement("in")] public int InputTokens { get; set; }

    /// <summary>Số token đầu ra (câu trả lời AI trả về). [BsonElement("out")] = tên trường trong MongoDB là "out".</summary>
    [BsonElement("out")] public int OutputTokens { get; set; }
}
