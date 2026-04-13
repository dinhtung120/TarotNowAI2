namespace TarotNow.Application.Common;

// DTO metadata media đính kèm trong tin nhắn chat.
public class MediaPayloadDto
{
    // URL truy cập file media.
    public string Url { get; set; } = string.Empty;

    // Object key lưu trên R2 (dùng để đối chiếu bảo mật và cleanup).
    public string? ObjectKey { get; set; }

    // Upload token one-time do endpoint presign cấp.
    // Chỉ dùng để validate/consume, không persist vào DB.
    public string? UploadToken { get; set; }

    // MIME type để client render đúng loại media.
    public string? MimeType { get; set; }

    // Kích thước file theo byte.
    public long? SizeBytes { get; set; }

    // Thời lượng media theo mili giây (audio/video).
    public int? DurationMs { get; set; }

    // Chiều rộng media theo pixel.
    public int? Width { get; set; }

    // Chiều cao media theo pixel.
    public int? Height { get; set; }

    // URL thumbnail phục vụ hiển thị preview nhanh.
    public string? ThumbnailUrl { get; set; }

    // Mô tả ngắn cho media nếu có.
    public string? Description { get; set; }

    // Trạng thái xử lý media (upload/processing/ready...).
    public string? ProcessingStatus { get; set; }
}
