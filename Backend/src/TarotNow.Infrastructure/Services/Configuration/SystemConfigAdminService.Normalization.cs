using System.Text.Encodings.Web;
using System.Text.Json;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigAdminService
{
    private static readonly JsonSerializerOptions CanonicalJsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static string ResolveDescription(string? requestedDescription, string? existingDescription, string fallbackDescription)
    {
        if (!string.IsNullOrWhiteSpace(requestedDescription))
        {
            return requestedDescription.Trim();
        }

        if (!string.IsNullOrWhiteSpace(existingDescription))
        {
            return existingDescription.Trim();
        }

        return fallbackDescription;
    }

    private static string NormalizeValue(string value, string valueKind)
    {
        if (string.Equals(valueKind, "json", StringComparison.Ordinal))
        {
            return CanonicalizeJson(value);
        }

        return value.Trim();
    }

    private static string CanonicalizeJson(string rawValue)
    {
        using var document = JsonDocument.Parse(rawValue);
        return JsonSerializer.Serialize(document.RootElement, CanonicalJsonOptions);
    }
}
