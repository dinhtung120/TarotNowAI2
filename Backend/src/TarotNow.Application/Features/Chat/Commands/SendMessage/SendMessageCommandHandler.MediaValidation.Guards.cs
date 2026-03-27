using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private static void EnsureMimeTypeAllowed(string type, string mimeType)
    {
        if (string.Equals(type, "image", StringComparison.OrdinalIgnoreCase)
            && AllowedImageMimeTypes.Contains(mimeType) == false)
        {
            throw new BadRequestException("Image mime type chưa được hỗ trợ.");
        }

        if (string.Equals(type, "voice", StringComparison.OrdinalIgnoreCase)
            && AllowedVoiceMimeTypes.Contains(mimeType) == false)
        {
            throw new BadRequestException("Voice mime type chưa được hỗ trợ.");
        }
    }

    private static void EnsureMediaLimits(string type, MediaPayloadDto payload)
    {
        if (payload.SizeBytes is > 52_428_800)
        {
            throw new BadRequestException("Kích thước media vượt quá 50MB.");
        }

        if (string.Equals(type, "voice", StringComparison.OrdinalIgnoreCase)
            && payload.DurationMs is <= 0 or > 600_000)
        {
            throw new BadRequestException("Thời lượng voice phải trong khoảng 1ms đến 600000ms.");
        }
    }
}
