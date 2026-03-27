using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private static bool IsDataUri(string value)
    {
        return value.StartsWith("data:", StringComparison.OrdinalIgnoreCase);
    }

    private static string? ParseMimeTypeFromDataUri(string dataUri)
    {
        if (IsDataUri(dataUri) == false)
        {
            return null;
        }

        var commaIndex = dataUri.IndexOf(',');
        var metadata = commaIndex >= 0
            ? dataUri[5..commaIndex]
            : dataUri[5..];

        if (string.IsNullOrWhiteSpace(metadata))
        {
            return null;
        }

        var semicolonIndex = metadata.IndexOf(';');
        var mimeType = semicolonIndex >= 0 ? metadata[..semicolonIndex] : metadata;
        return string.IsNullOrWhiteSpace(mimeType) ? null : mimeType;
    }

    private static long? TryEstimateDataUriBytes(string dataUri)
    {
        var commaIndex = dataUri.IndexOf(',');
        if (commaIndex < 0 || commaIndex == dataUri.Length - 1)
        {
            return null;
        }

        var payload = dataUri[(commaIndex + 1)..];
        if (payload.Length == 0)
        {
            return 0;
        }

        var padding = payload.EndsWith("==", StringComparison.Ordinal)
            ? 2
            : payload.EndsWith("=", StringComparison.Ordinal)
                ? 1
                : 0;

        return (long)((payload.Length * 3L) / 4L - padding);
    }

    private static void EnsureDataUriLimits(Uri mediaUri, MediaPayloadDto payload)
    {
        if (string.Equals(mediaUri.Scheme, "data", StringComparison.OrdinalIgnoreCase) == false)
        {
            return;
        }

        if (payload.SizeBytes is > 5_242_880)
        {
            throw new BadRequestException("Data URL media vượt quá 5MB.");
        }
    }
}
