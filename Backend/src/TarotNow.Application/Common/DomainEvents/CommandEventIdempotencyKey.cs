using System.Security.Cryptography;
using System.Text;

namespace TarotNow.Application.Common.DomainEvents;

// Chuẩn hóa key idempotency cho command-requested domain events theo định dạng chung.
public static class CommandEventIdempotencyKey
{
    private const int MaxEventNameLength = 64;
    private const int MaxRawKeyLength = 80;

    public static string Build(string eventName, string? rawKey)
    {
        var normalizedEvent = NormalizeSegment(eventName, MaxEventNameLength);
        var normalizedKey = NormalizeKey(rawKey);
        return $"cmd:{normalizedEvent}:{normalizedKey}";
    }

    private static string NormalizeKey(string? rawKey)
    {
        if (string.IsNullOrWhiteSpace(rawKey))
        {
            return "missing";
        }

        var trimmed = rawKey.Trim().ToLowerInvariant();
        if (trimmed.Length <= MaxRawKeyLength)
        {
            return trimmed;
        }

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(trimmed));
        return $"sha256-{Convert.ToHexString(hash).ToLowerInvariant()}";
    }

    private static string NormalizeSegment(string raw, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return "unknown";
        }

        var builder = new StringBuilder(raw.Length);
        foreach (var ch in raw.Trim().ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(ch) || ch is '-' or '_')
            {
                builder.Append(ch);
            }
            else
            {
                builder.Append('-');
            }
        }

        var normalized = builder.ToString().Trim('-');
        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "unknown";
        }

        return normalized.Length <= maxLength ? normalized : normalized[..maxLength];
    }
}
