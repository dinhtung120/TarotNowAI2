namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    private static readonly HashSet<string> AllowedImageMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/avif",
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    private static readonly HashSet<string> AllowedVoiceMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "audio/ogg",
        "audio/opus",
        "audio/mpeg",
        "audio/wav",
        "audio/webm",
        "audio/mp4"
    };

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

    private static readonly IReadOnlyDictionary<string, string> ExtensionByMime = MimeByExtension
        .GroupBy(kvp => kvp.Value)
        .ToDictionary(g => g.Key, g => g.First().Key, StringComparer.OrdinalIgnoreCase);

    private static string? GetExtensionFromMime(string? mimeType)
    {
        if (string.IsNullOrWhiteSpace(mimeType)) return null;
        var normalized = NormalizeMimeType(mimeType);
        return ExtensionByMime.TryGetValue(normalized, out var ext) ? ext : null;
    }

    private static string? GuessMimeTypeFromPath(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        return MimeByExtension.TryGetValue(extension, out var mimeType) ? mimeType : null;
    }

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
