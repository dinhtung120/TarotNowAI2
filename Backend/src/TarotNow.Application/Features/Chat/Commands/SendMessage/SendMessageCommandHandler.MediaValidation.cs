using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private static void ValidateMediaRequest(SendMessageCommand request)
    {
        if (IsMediaType(request.Type) == false)
        {
            return;
        }

        request.MediaPayload ??= BuildFallbackMediaPayload(request.Content);
        ValidateMediaPayload(request.MediaPayload, request.Type);

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            request.Content = request.MediaPayload.Description ?? request.MediaPayload.Url;
        }
    }

    private async Task ProcessMediaRequestAsync(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (IsMediaType(request.Type) == false || request.MediaPayload == null) return;

        if (IsDataUri(request.MediaPayload.Url))
        {
            var commaIndex = request.MediaPayload.Url.IndexOf(',');
            if (commaIndex >= 0)
            {
                var base64Data = request.MediaPayload.Url.Substring(commaIndex + 1);
                var mediaBytes = Convert.FromBase64String(base64Data);
                
                byte[] processedBytes;
                string newMimeType;

                if (string.Equals(request.Type, "image", StringComparison.OrdinalIgnoreCase))
                {
                    (processedBytes, newMimeType) = await _mediaProcessor.ProcessAndCompressImageAsync(mediaBytes, cancellationToken);
                }
                else
                {
                    var ext = GetExtensionFromMime(request.MediaPayload.MimeType) ?? ".webm";
                    (processedBytes, newMimeType) = await _mediaProcessor.ProcessAndCompressVoiceAsync(mediaBytes, ext, cancellationToken);
                }

                var processedBase64 = Convert.ToBase64String(processedBytes);
                request.MediaPayload.Url = $"data:{newMimeType};base64,{processedBase64}";
                request.MediaPayload.MimeType = newMimeType;
                request.MediaPayload.SizeBytes = processedBytes.Length;
                request.MediaPayload.ProcessingStatus = "compressed";
                
                // Cập nhật lại Content nếu đó là DataUri root
                if (IsDataUri(request.Content))
                {
                    request.Content = request.MediaPayload.Url;
                }
            }
        }
    }

    private static bool IsMediaType(string type)
    {
        return string.Equals(type, "image", StringComparison.OrdinalIgnoreCase)
            || string.Equals(type, "voice", StringComparison.OrdinalIgnoreCase);
    }

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

    private static void ValidateMediaPayload(MediaPayloadDto payload, string type)
    {
        var mediaUri = ResolveAndValidateMediaUri(payload.Url);
        if (string.Equals(mediaUri.Scheme, "data", StringComparison.OrdinalIgnoreCase)
            && payload.SizeBytes is null)
        {
            payload.SizeBytes = TryEstimateDataUriBytes(payload.Url);
        }

        var mimeType = ResolveMimeType(payload, mediaUri);
        EnsureMimeTypeAllowed(type, mimeType);
        EnsureDataUriLimits(mediaUri, payload);
        EnsureMediaLimits(type, payload);

        payload.MimeType = mimeType;
        payload.ProcessingStatus ??= "validated";
    }

    private static Uri ResolveAndValidateMediaUri(string url)
    {
        if (IsDataUri(url))
        {
            return new Uri("data:,");
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out var mediaUri) == false
            || (mediaUri.Scheme != Uri.UriSchemeHttp
                && mediaUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new BadRequestException("Media URL không hợp lệ.");
        }

        return mediaUri;
    }

    private static string ResolveMimeType(MediaPayloadDto payload, Uri mediaUri)
    {
        var mimeType = string.Equals(mediaUri.Scheme, "data", StringComparison.OrdinalIgnoreCase)
            ? payload.MimeType ?? ParseMimeTypeFromDataUri(payload.Url)
            : payload.MimeType ?? GuessMimeTypeFromPath(mediaUri.AbsolutePath);

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
