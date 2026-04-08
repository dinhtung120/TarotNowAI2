using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    /// <summary>
    /// Validate và chuẩn hóa media payload trong request gửi tin nhắn.
    /// Luồng xử lý: nếu không phải media thì bỏ qua; nếu thiếu payload thì fallback từ content; sau đó validate URL/mime/giới hạn và đồng bộ content.
    /// </summary>
    private static void ValidateMediaRequest(SendMessageCommand request)
    {
        if (IsMediaType(request.Type) == false)
        {
            return;
        }

        // Hỗ trợ tương thích request cũ chỉ truyền content mà chưa có MediaPayload.
        request.MediaPayload ??= BuildFallbackMediaPayload(request.Content);
        ValidateMediaPayload(request.MediaPayload, request.Type);

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            // Đảm bảo message có content tối thiểu để client hiển thị.
            request.Content = request.MediaPayload.Description ?? request.MediaPayload.Url;
        }
    }

    /// <summary>
    /// Xác định type hiện tại có thuộc nhóm media được hỗ trợ hay không.
    /// Luồng xử lý: so sánh không phân biệt hoa thường với image/voice.
    /// </summary>
    private static bool IsMediaType(string type)
    {
        return string.Equals(type, "image", StringComparison.OrdinalIgnoreCase)
            || string.Equals(type, "voice", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Dựng media payload fallback từ trường content.
    /// Luồng xử lý: ưu tiên parse data-uri; nếu không phải data-uri thì parse absolute URL và suy đoán mime theo path.
    /// </summary>
    private static MediaPayloadDto BuildFallbackMediaPayload(string content)
    {
        if (IsDataUri(content))
        {
            var dataUriMime = ParseMimeTypeFromDataUri(content);
            return new MediaPayloadDto
            {
                Url = content,
                MimeType = dataUriMime,
                SizeBytes = TryEstimateDataUriBytes(content),
                ProcessingStatus = "fallback_unvalidated"
            };
        }

        if (Uri.TryCreate(content, UriKind.Absolute, out var mediaUri) == false)
        {
            // Content không parse được thành media URL hợp lệ thì từ chối request.
            throw new BadRequestException("Media payload không hợp lệ.");
        }

        var guessedMime = GuessMimeTypeFromPath(mediaUri.AbsolutePath);
        return new MediaPayloadDto
        {
            Url = mediaUri.ToString(),
            MimeType = guessedMime,
            ProcessingStatus = "fallback_unvalidated"
        };
    }

    /// <summary>
    /// Validate chi tiết media payload và chuẩn hóa metadata.
    /// Luồng xử lý: validate URI, suy luận mime, kiểm tra mime/size/duration theo rule, rồi ghi lại mime/status chuẩn hóa.
    /// </summary>
    private static void ValidateMediaPayload(MediaPayloadDto payload, string type)
    {
        var mediaUri = ResolveAndValidateMediaUri(payload.Url);
        if (string.Equals(mediaUri.Scheme, "data", StringComparison.OrdinalIgnoreCase)
            && payload.SizeBytes is null)
        {
            // Ước lượng kích thước từ data-uri khi client chưa gửi size.
            payload.SizeBytes = TryEstimateDataUriBytes(payload.Url);
        }

        var mimeType = ResolveMimeType(payload, mediaUri);
        EnsureMimeTypeAllowed(type, mimeType);
        EnsureDataUriLimits(mediaUri, payload);
        EnsureMediaLimits(type, payload);

        payload.MimeType = mimeType;
        payload.ProcessingStatus ??= "validated";
    }

    /// <summary>
    /// Parse và validate URI của media payload.
    /// Luồng xử lý: data-uri được chấp nhận đặc biệt; các URI còn lại bắt buộc là absolute HTTP/HTTPS.
    /// </summary>
    private static Uri ResolveAndValidateMediaUri(string url)
    {
        if (IsDataUri(url))
        {
            // Dùng URI giả cho data scheme để thống nhất luồng xử lý phía sau.
            return new Uri("data:,");
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out var mediaUri) == false
            || (mediaUri.Scheme != Uri.UriSchemeHttp
                && mediaUri.Scheme != Uri.UriSchemeHttps))
        {
            // Chặn URL không an toàn/không hợp lệ ngay từ biên.
            throw new BadRequestException("Media URL không hợp lệ.");
        }

        return mediaUri;
    }

    /// <summary>
    /// Xác định mime type cuối cùng cho media payload.
    /// Luồng xử lý: lấy mime từ payload hoặc suy luận theo nguồn URL, chuẩn hóa chuỗi mime và kiểm tra rỗng.
    /// </summary>
    private static string ResolveMimeType(MediaPayloadDto payload, Uri mediaUri)
    {
        var mimeType = string.Equals(mediaUri.Scheme, "data", StringComparison.OrdinalIgnoreCase)
            ? payload.MimeType ?? ParseMimeTypeFromDataUri(payload.Url)
            : payload.MimeType ?? GuessMimeTypeFromPath(mediaUri.AbsolutePath);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            // Không xác định được mime thì không thể áp dụng whitelist media an toàn.
            throw new BadRequestException("Media mime type không hợp lệ.");
        }

        var normalizedMimeType = NormalizeMimeType(mimeType);
        if (string.IsNullOrWhiteSpace(normalizedMimeType))
        {
            // Mime rỗng sau normalize được xem là dữ liệu không hợp lệ.
            throw new BadRequestException("Media mime type không hợp lệ.");
        }

        return normalizedMimeType;
    }
}
