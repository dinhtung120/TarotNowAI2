namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    // Tập mime ảnh được hệ thống cho phép nhận và xử lý.
    private static readonly HashSet<string> AllowedImageMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/avif",
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    // Tập mime âm thanh được hệ thống cho phép cho message voice.
    private static readonly HashSet<string> AllowedVoiceMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "audio/ogg",
        "audio/opus",
        "audio/mpeg",
        "audio/wav",
        "audio/webm",
        "audio/mp4"
    };

    // Bảng ánh xạ extension -> mime để suy đoán định dạng khi payload thiếu mime.
    private static readonly IReadOnlyDictionary<string, string> MimeByExtension = new Dictionary<string, string>
    {
        [".avif"] = "image/avif",
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".png"] = "image/png",
        [".webp"] = "image/webp",
        [".opus"] = "audio/opus",
        [".ogg"] = "audio/ogg",
        [".mp3"] = "audio/mpeg",
        [".wav"] = "audio/wav",
        [".webm"] = "audio/webm",
        [".m4a"] = "audio/mp4"
    };

    // Bảng ánh xạ mime -> extension ưu tiên, dùng khi cần xuất file từ mime.
    private static readonly IReadOnlyDictionary<string, string> ExtensionByMime = MimeByExtension
        .GroupBy(kvp => kvp.Value)
        .ToDictionary(g => g.Key, g => g.First().Key, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Lấy extension tương ứng từ mime type.
    /// Luồng xử lý: chuẩn hóa mime rồi tra bảng ExtensionByMime; trả null khi mime rỗng hoặc không hỗ trợ.
    /// </summary>
    private static string? GetExtensionFromMime(string? mimeType)
    {
        if (string.IsNullOrWhiteSpace(mimeType))
        {
            return null;
        }

        var normalized = NormalizeMimeType(mimeType);
        return ExtensionByMime.TryGetValue(normalized, out var ext) ? ext : null;
    }

    /// <summary>
    /// Suy đoán mime type từ phần mở rộng của đường dẫn media.
    /// Luồng xử lý: lấy extension đã normalize lower-case rồi tra bảng MimeByExtension.
    /// </summary>
    private static string? GuessMimeTypeFromPath(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        return MimeByExtension.TryGetValue(extension, out var mimeType) ? mimeType : null;
    }

    /// <summary>
    /// Chuẩn hóa mime type về dạng lowercase, bỏ tham số phụ.
    /// Luồng xử lý: trim chuỗi, cắt phần sau dấu ';' (nếu có), rồi trim/lowercase để so sánh ổn định.
    /// </summary>
    private static string NormalizeMimeType(string mimeType)
    {
        var trimmed = mimeType.Trim();
        if (trimmed.Length == 0)
        {
            return trimmed;
        }

        var semicolonIndex = trimmed.IndexOf(';');
        if (semicolonIndex >= 0)
        {
            trimmed = trimmed[..semicolonIndex];
        }

        return trimmed.Trim().ToLowerInvariant();
    }
}
