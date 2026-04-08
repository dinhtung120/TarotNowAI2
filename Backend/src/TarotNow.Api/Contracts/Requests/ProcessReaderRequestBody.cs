namespace TarotNow.Api.Contracts.Requests;

// Payload admin xử lý đơn đăng ký reader.
public class ProcessReaderRequestBody
{
    // Định danh đơn đăng ký reader cần xử lý.
    public string RequestId { get; set; } = string.Empty;

    // Hành động xử lý (approve/reject) theo rule nghiệp vụ.
    public string Action { get; set; } = string.Empty;

    // Ghi chú của admin để phục vụ audit và phản hồi nội bộ.
    public string? AdminNote { get; set; }
}
