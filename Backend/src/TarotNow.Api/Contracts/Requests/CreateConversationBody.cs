namespace TarotNow.Api.Contracts.Requests;

// Payload tạo hội thoại mới giữa user và reader.
public class CreateConversationBody
{
    // Reader mục tiêu mà người dùng muốn bắt đầu hội thoại.
    public Guid ReaderId { get; set; }

    // Số giờ SLA tùy chọn cho yêu cầu ưu tiên hoặc ngữ cảnh đặc biệt.
    public int? SlaHours { get; set; }
}
