namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    /// <summary>
    /// Tiền xử lý media request trước khi lưu message.
    /// Luồng xử lý: chỉ xử lý khi type là media và payload là data-uri, sau đó nén/chuyển đổi để giảm kích thước.
    /// </summary>
    private async Task ProcessMediaRequestAsync(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (IsMediaType(request.Type) == false || request.MediaPayload == null)
        {
            // Không phải media hoặc thiếu payload thì không cần tiền xử lý.
            return;
        }

        if (IsDataUri(request.MediaPayload.Url) == false)
        {
            // URL ngoài (http/https) không nén tại đây, giữ nguyên để backend khác xử lý nếu cần.
            return;
        }

        await CompressDataUriPayloadAsync(request, cancellationToken);
    }

    /// <summary>
    /// Nén/chuyển đổi payload data-uri và cập nhật metadata media.
    /// Luồng xử lý: tách base64, decode bytes, xử lý theo loại media, rồi ghi lại URL data-uri mới cùng mime/size/status.
    /// </summary>
    private async Task CompressDataUriPayloadAsync(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var commaIndex = request.MediaPayload!.Url.IndexOf(',');
        if (commaIndex < 0)
        {
            // Data-uri không có phần phân cách metadata/payload thì bỏ qua để tránh lỗi parse.
            return;
        }

        var base64Data = request.MediaPayload.Url[(commaIndex + 1)..];
        var mediaBytes = Convert.FromBase64String(base64Data);
        var (processedBytes, mimeType) = await ProcessMediaBytesAsync(request, mediaBytes, cancellationToken);
        var processedBase64 = Convert.ToBase64String(processedBytes);

        request.MediaPayload.Url = $"data:{mimeType};base64,{processedBase64}";
        request.MediaPayload.MimeType = mimeType;
        request.MediaPayload.SizeBytes = processedBytes.Length;
        request.MediaPayload.ProcessingStatus = "compressed";

        if (IsDataUri(request.Content))
        {
            // Đồng bộ content với URL media mới khi content ban đầu cũng là data-uri.
            request.Content = request.MediaPayload.Url;
        }
    }

    /// <summary>
    /// Xử lý bytes media theo loại request (image hoặc voice).
    /// Luồng xử lý: image đi qua pipeline nén ảnh; voice đi qua pipeline nén âm thanh với extension suy ra từ mime.
    /// </summary>
    private async Task<(byte[] Bytes, string MimeType)> ProcessMediaBytesAsync(
        SendMessageCommand request,
        byte[] mediaBytes,
        CancellationToken cancellationToken)
    {
        if (string.Equals(request.Type, "image", StringComparison.OrdinalIgnoreCase))
        {
            // Nhánh ảnh: chuẩn hóa + nén theo pipeline image processor.
            return await _mediaProcessor.ProcessAndCompressImageAsync(mediaBytes, cancellationToken);
        }

        var extension = GetExtensionFromMime(request.MediaPayload!.MimeType) ?? ".webm";
        // Nhánh voice: fallback extension .webm khi mime không map được để đảm bảo pipeline không vỡ.
        return await _mediaProcessor.ProcessAndCompressVoiceAsync(mediaBytes, extension, cancellationToken);
    }
}
