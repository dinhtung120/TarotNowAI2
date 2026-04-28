using System.Text.Json;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class ReadingPromptService
{
    private sealed class CachedCatalog
    {
        public CachedCatalog(DateTime updatedAt, string rawValue, ReadingPromptCatalog catalog)
        {
            UpdatedAt = updatedAt;
            RawValue = rawValue;
            Catalog = catalog;
        }

        public DateTime UpdatedAt { get; }

        public string RawValue { get; }

        public ReadingPromptCatalog Catalog { get; }
    }

    private sealed class ReadingPromptCatalog
    {
        public string DefaultLocale { get; set; } = "vi";

        public Dictionary<string, string> System { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, JsonElement> Initial { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, JsonElement> Followup { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public ReadingPromptContext Context { get; set; } = new();
    }

    private sealed class ReadingPromptContext
    {
        public JsonElement DefaultQuestion { get; set; }

        public ReadingPromptOrientation Orientation { get; set; } = new();

        public JsonElement UnknownCardLabel { get; set; }
    }

    private sealed class ReadingPromptOrientation
    {
        public JsonElement Upright { get; set; }

        public JsonElement Reversed { get; set; }
    }
}
