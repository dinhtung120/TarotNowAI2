using System.Collections.Generic;

namespace TarotNow.Application.Common;

// Kết quả chuẩn cho một hành động trong workflow hội thoại.
public class ConversationActionResult
{
    // Cờ biểu thị hành động có xử lý thành công hay không.
    public bool Success { get; set; } = true;

    // Trạng thái workflow sau khi thực hiện hành động.
    public string Status { get; set; } = string.Empty;

    // Lý do thất bại hoặc ghi chú bổ sung nếu có.
    public string? Reason { get; set; }

    // Metadata mở rộng để trả thêm dữ liệu theo từng kịch bản nghiệp vụ.
    public Dictionary<string, object>? Metadata { get; set; }
}

// Kết quả hành động phản hồi hoàn tất hội thoại.
public class ConversationCompleteRespondResult : ConversationActionResult
{
    // Cờ thể hiện người nhận có chấp nhận hoàn tất hay không.
    public bool Accepted { get; set; }
}

// Kết quả khi tạo yêu cầu thêm tiền trong hội thoại.
public class ConversationAddMoneyRequestResult
{
    // Cờ báo thao tác tạo yêu cầu thành công.
    public bool Success { get; set; } = true;

    // Định danh tin nhắn chứa yêu cầu thêm tiền.
    public string MessageId { get; set; } = string.Empty;
}

// Kết quả khi phản hồi yêu cầu thêm tiền.
public class ConversationAddMoneyRespondResult
{
    // Cờ báo thao tác phản hồi thành công.
    public bool Success { get; set; } = true;

    // Cờ cho biết yêu cầu thêm tiền được chấp nhận hay bị từ chối.
    public bool Accepted { get; set; }

    // Định danh item tài chính phát sinh nếu chấp nhận.
    public Guid? ItemId { get; set; }

    // Định danh tin nhắn phản hồi trong hội thoại.
    public string MessageId { get; set; } = string.Empty;
}

// Kết quả gửi đánh giá reader sau khi hoàn thành hội thoại.
public class ConversationReviewSubmitResult
{
    // Cờ báo thao tác gửi đánh giá thành công.
    public bool Success { get; set; } = true;

    // Định danh conversation được đánh giá.
    public string ConversationId { get; set; } = string.Empty;

    // Định danh reader nhận đánh giá.
    public string ReaderId { get; set; } = string.Empty;

    // Điểm sao đã gửi.
    public int Rating { get; set; }

    // Comment tùy chọn đi kèm.
    public string? Comment { get; set; }

    // Thời điểm đánh giá được lưu.
    public DateTime CreatedAt { get; set; }
}
