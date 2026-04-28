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

        public Dictionary<string, Dictionary<string, string>> Initial { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, Dictionary<string, string>> Followup { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public ReadingPromptContext Context { get; set; } = new();
    }

    private sealed class ReadingPromptContext
    {
        public Dictionary<string, string> DefaultQuestion { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public ReadingPromptOrientation Orientation { get; set; } = new();

        public Dictionary<string, string> UnknownCardLabel { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    private sealed class ReadingPromptOrientation
    {
        public Dictionary<string, string> Upright { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> Reversed { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
