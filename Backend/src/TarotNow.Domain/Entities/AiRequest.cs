
namespace TarotNow.Domain.Entities;

// Entity lưu vòng đời một yêu cầu AI để theo dõi trạng thái, chi phí và truy vết kỹ thuật.
public class AiRequest
{
    // Định danh yêu cầu AI.
    public Guid Id { get; set; } = Guid.NewGuid();

    // Định danh người dùng tạo yêu cầu.
    public Guid UserId { get; set; }

    // Mã tham chiếu phiên đọc bài chứa yêu cầu này.
    public Guid ReadingSessionRef { get; set; }

    // Số thứ tự follow-up trong cùng phiên; null khi là lượt đầu.
    public short? FollowupSequence { get; set; }

    // Trạng thái xử lý yêu cầu AI theo lifecycle chuẩn.
    public string Status { get; set; } = Enums.AiRequestStatus.Requested;

    // Mốc thời gian token đầu tiên được trả về.
    public DateTimeOffset? FirstTokenAt { get; set; }

    // Mốc hệ thống nhận marker hoàn tất sinh nội dung.
    public DateTimeOffset? CompletionMarkerAt { get; set; }

    // Lý do kết thúc do provider hoặc pipeline trả về.
    public string? FinishReason { get; set; }

    // Số lần retry đã thực hiện cho yêu cầu.
    public short RetryCount { get; set; }

    // Phiên bản prompt áp dụng tại thời điểm gọi AI.
    public string? PromptVersion { get; set; }

    // Phiên bản policy/guardrail đi kèm request.
    public string? PolicyVersion { get; set; }

    // Correlation id để nối log xuyên dịch vụ.
    public Guid? CorrelationId { get; set; }

    // Trace id phục vụ quan sát distributed tracing.
    public string? TraceId { get; set; }

    // Chi phí Gold đã thu cho yêu cầu.
    public long ChargeGold { get; set; }

    // Chi phí Diamond đã thu cho yêu cầu.
    public long ChargeDiamond { get; set; }

    // Locale yêu cầu từ client.
    public string? RequestedLocale { get; set; }

    // Locale thực tế hệ thống trả về sau xử lý fallback.
    public string? ReturnedLocale { get; set; }

    // Lý do fallback ngôn ngữ hoặc mô hình (nếu có).
    public string? FallbackReason { get; set; }

    // Khóa idempotency để chống xử lý trùng.
    public string? IdempotencyKey { get; set; }

    // Thời điểm tạo yêu cầu.
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Thời điểm cập nhật gần nhất.
    public DateTimeOffset? UpdatedAt { get; set; }

    // Navigation tới user sở hữu yêu cầu.
    public User User { get; set; } = null!;
}
