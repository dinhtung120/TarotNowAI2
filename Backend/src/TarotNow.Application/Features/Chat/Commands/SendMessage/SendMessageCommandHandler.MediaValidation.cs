using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    /// <summary>
    /// Validate và chuẩn hóa media payload trong request gửi tin nhắn.
    /// Luồng xử lý: chỉ chạy với image/voice, bắt buộc payload hợp lệ và đồng bộ content fallback.
    /// </summary>
    private static void ValidateMediaRequest(SendMessageCommand request)
    {
        if (IsMediaType(request.Type) == false)
        {
            return;
        }

        if (request.MediaPayload is null)
        {
            throw new BadRequestException("Thiếu media payload cho tin nhắn media.");
        }

        ValidateMediaPayload(request.MediaPayload, request.Type);

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            request.Content = string.Equals(request.Type, "voice", StringComparison.OrdinalIgnoreCase)
                ? "[voice]"
                : "[image]";
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
    /// Validate chi tiết media payload và chuẩn hóa metadata.
    /// Luồng xử lý: validate URL/mime/size/duration theo rule và ghi lại mime/status chuẩn hóa.
    /// </summary>
    private static void ValidateMediaPayload(MediaPayloadDto payload, string type)
    {
        var mediaUri = ResolveAndValidateMediaUri(payload.Url);
        var mimeType = ResolveMimeType(payload, mediaUri);

        EnsureMimeTypeAllowed(type, mimeType);
        EnsureMediaLimits(type, payload);

        payload.MimeType = mimeType;
        payload.ProcessingStatus ??= "validated";
    }

    /// <summary>
    /// Parse và validate URI của media payload.
    /// Luồng xử lý: bắt buộc absolute HTTP/HTTPS và loại bỏ data-uri flow cũ.
    /// </summary>
    private static Uri ResolveAndValidateMediaUri(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var mediaUri) == false
            || (mediaUri.Scheme != Uri.UriSchemeHttp
                && mediaUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new BadRequestException("Media URL không hợp lệ.");
        }

        return mediaUri;
    }

    /// <summary>
    /// Xác định mime type cuối cùng cho media payload.
    /// Luồng xử lý: lấy mime từ payload hoặc suy luận theo đường dẫn URL, sau đó normalize.
    /// </summary>
    private static string ResolveMimeType(MediaPayloadDto payload, Uri mediaUri)
    {
        var mimeType = payload.MimeType ?? GuessMimeTypeFromPath(mediaUri.AbsolutePath);
        if (string.IsNullOrWhiteSpace(mimeType))
        {
            throw new BadRequestException("Media mime type không hợp lệ.");
        }

        var normalizedMimeType = NormalizeMimeType(mimeType);
        if (string.IsNullOrWhiteSpace(normalizedMimeType))
        {
            throw new BadRequestException("Media mime type không hợp lệ.");
        }

        return normalizedMimeType;
    }
}
