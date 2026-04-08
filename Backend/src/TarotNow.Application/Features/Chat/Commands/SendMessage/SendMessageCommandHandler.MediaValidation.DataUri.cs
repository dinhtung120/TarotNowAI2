using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    /// <summary>
    /// Kiểm tra chuỗi có phải data-uri hay không.
    /// Luồng xử lý: so sánh tiền tố "data:" không phân biệt hoa thường.
    /// </summary>
    private static bool IsDataUri(string value)
    {
        return value.StartsWith("data:", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Parse mime type từ data-uri.
    /// Luồng xử lý: tách phần metadata trước dấu phẩy, lấy phần mime trước dấu chấm phẩy, trả null khi không xác định được.
    /// </summary>
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

    /// <summary>
    /// Ước lượng số byte của payload trong data-uri base64.
    /// Luồng xử lý: tách phần payload, tính theo công thức base64 và trừ padding nếu có.
    /// </summary>
    private static long? TryEstimateDataUriBytes(string dataUri)
    {
        var commaIndex = dataUri.IndexOf(',');
        if (commaIndex < 0 || commaIndex == dataUri.Length - 1)
        {
            // Data-uri thiếu payload hợp lệ nên không thể ước lượng kích thước.
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

    /// <summary>
    /// Kiểm tra giới hạn dung lượng cho media kiểu data-uri.
    /// Luồng xử lý: chỉ áp dụng với scheme data và chặn payload vượt quá 5MB.
    /// </summary>
    private static void EnsureDataUriLimits(Uri mediaUri, MediaPayloadDto payload)
    {
        if (string.Equals(mediaUri.Scheme, "data", StringComparison.OrdinalIgnoreCase) == false)
        {
            return;
        }

        if (payload.SizeBytes is > 5_242_880)
        {
            // Chặn data-uri quá lớn để bảo vệ bộ nhớ và thời gian xử lý.
            throw new BadRequestException("Data URL media vượt quá 5MB.");
        }
    }
}
