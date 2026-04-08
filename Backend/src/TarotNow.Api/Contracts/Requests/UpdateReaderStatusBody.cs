namespace TarotNow.Api.Contracts.Requests;

// Payload cập nhật trạng thái hoạt động của reader.
public class UpdateReaderStatusBody
{
    // Trạng thái mục tiêu sau khi chuẩn hóa (online/offline/busy...).
    public string Status { get; set; } = string.Empty;
}
