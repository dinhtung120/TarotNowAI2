namespace TarotNow.Api.Contracts.Requests;

// Payload tạo báo cáo vi phạm/sự cố từ người dùng.
public class CreateReportBody
{
    // Loại đối tượng bị báo cáo (post, comment, user, message...).
    public string TargetType { get; set; } = string.Empty;

    // Định danh đối tượng bị báo cáo theo loại mục tiêu tương ứng.
    public string TargetId { get; set; } = string.Empty;

    // Mã hội thoại liên quan khi báo cáo phát sinh trong phiên chat.
    public string? ConversationRef { get; set; }

    // Lý do báo cáo do người dùng cung cấp.
    public string Reason { get; set; } = string.Empty;
}
