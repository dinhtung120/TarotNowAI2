namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private async Task ProcessMediaRequestAsync(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (IsMediaType(request.Type) == false || request.MediaPayload == null)
        {
            return;
        }

        if (IsDataUri(request.MediaPayload.Url) == false)
        {
            return;
        }

        await CompressDataUriPayloadAsync(request, cancellationToken);
    }

    private async Task CompressDataUriPayloadAsync(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var commaIndex = request.MediaPayload!.Url.IndexOf(',');
        if (commaIndex < 0)
        {
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
            request.Content = request.MediaPayload.Url;
        }
    }

    private async Task<(byte[] Bytes, string MimeType)> ProcessMediaBytesAsync(
        SendMessageCommand request,
        byte[] mediaBytes,
        CancellationToken cancellationToken)
    {
        if (string.Equals(request.Type, "image", StringComparison.OrdinalIgnoreCase))
        {
            return await _mediaProcessor.ProcessAndCompressImageAsync(mediaBytes, cancellationToken);
        }

        var extension = GetExtensionFromMime(request.MediaPayload!.MimeType) ?? ".webm";
        return await _mediaProcessor.ProcessAndCompressVoiceAsync(mediaBytes, extension, cancellationToken);
    }
}
