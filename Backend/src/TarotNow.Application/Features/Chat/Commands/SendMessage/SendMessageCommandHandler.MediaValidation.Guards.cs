using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    /// <summary>
    /// Kiểm tra mime type có thuộc whitelist theo loại media hay không.
    /// Luồng xử lý: với image kiểm tra tập mime ảnh; với voice kiểm tra tập mime âm thanh.
    /// </summary>
    private static void EnsureMimeTypeAllowed(string type, string mimeType)
    {
        if (string.Equals(type, "image", StringComparison.OrdinalIgnoreCase)
            && AllowedImageMimeTypes.Contains(mimeType) == false)
        {
            // Chặn mime ảnh ngoài whitelist để giảm rủi ro bảo mật/xử lý sai định dạng.
            throw new BadRequestException("Image mime type chưa được hỗ trợ.");
        }

        if (string.Equals(type, "voice", StringComparison.OrdinalIgnoreCase)
            && AllowedVoiceMimeTypes.Contains(mimeType) == false)
        {
            // Chặn mime voice ngoài whitelist để giữ pipeline xử lý ổn định.
            throw new BadRequestException("Voice mime type chưa được hỗ trợ.");
        }
    }

    /// <summary>
    /// Kiểm tra giới hạn kích thước và thời lượng media.
    /// Luồng xử lý: áp dụng trần 50MB cho mọi media; với voice bắt buộc duration trong ngưỡng 1ms-600000ms.
    /// </summary>
    private static void EnsureMediaLimits(string type, MediaPayloadDto payload)
    {
        if (payload.SizeBytes is > 52_428_800)
        {
            // Giới hạn dung lượng để tránh upload quá lớn ảnh hưởng hiệu năng hệ thống.
            throw new BadRequestException("Kích thước media vượt quá 50MB.");
        }

        if (string.Equals(type, "voice", StringComparison.OrdinalIgnoreCase)
            && payload.DurationMs is <= 0 or > 600_000)
        {
            // Business rule: voice phải có thời lượng hợp lệ để client/player xử lý đúng.
            throw new BadRequestException("Thời lượng voice phải trong khoảng 1ms đến 600000ms.");
        }
    }
}
