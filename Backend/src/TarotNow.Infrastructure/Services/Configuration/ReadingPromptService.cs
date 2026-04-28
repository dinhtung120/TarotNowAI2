using System.Text.Json;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.SystemConfigs;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services.Configuration;

/// <summary>
/// Service resolve prompt reading/follow-up từ system config runtime.
/// </summary>
public sealed partial class ReadingPromptService : IReadingPromptService
{
    private const string PromptConfigKey = "ai.reading.prompts";

    private static readonly string[] CardNames =
    {
        "The Fool", "The Magician", "The High Priestess", "The Empress", "The Emperor", "The Hierophant", "The Lovers", "The Chariot", "Strength", "The Hermit", "Wheel of Fortune", "Justice", "The Hanged Man", "Death", "Temperance", "The Devil", "The Tower", "The Star", "The Moon", "The Sun", "Judgement", "The World",
        "Ace of Wands", "Two of Wands", "Three of Wands", "Four of Wands", "Five of Wands", "Six of Wands", "Seven of Wands", "Eight of Wands", "Nine of Wands", "Ten of Wands", "Page of Wands", "Knight of Wands", "Queen of Wands", "King of Wands",
        "Ace of Cups", "Two of Cups", "Three of Cups", "Four of Cups", "Five of Cups", "Six of Cups", "Seven of Cups", "Eight of Cups", "Nine of Cups", "Ten of Cups", "Page of Cups", "Knight of Cups", "Queen of Cups", "King of Cups",
        "Ace of Swords", "Two of Swords", "Three of Swords", "Four of Swords", "Five of Swords", "Six of Swords", "Seven of Swords", "Eight of Swords", "Nine of Swords", "Ten of Swords", "Page of Swords", "Knight of Swords", "Queen of Swords", "King of Swords",
        "Ace of Pentacles", "Two of Pentacles", "Three of Pentacles", "Four of Pentacles", "Five of Pentacles", "Six of Pentacles", "Seven of Pentacles", "Eight of Pentacles", "Nine of Pentacles", "Ten of Pentacles", "Page of Pentacles", "Knight of Pentacles", "Queen of Pentacles", "King of Pentacles"
    };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly ReadingPromptCatalog FallbackCatalog = BuildFallbackCatalog();

    private readonly SystemConfigSnapshotStore _snapshotStore;
    private readonly ILogger<ReadingPromptService> _logger;
    private readonly object _sync = new();

    private CachedCatalog? _cachedCatalog;

    public ReadingPromptService(
        SystemConfigSnapshotStore snapshotStore,
        ILogger<ReadingPromptService> logger)
    {
        _snapshotStore = snapshotStore;
        _logger = logger;
    }

    /// <inheritdoc />
    public ReadingPromptBundle Build(ReadingSession session, string? followupQuestion, string language)
    {
        ArgumentNullException.ThrowIfNull(session);

        var catalog = ResolveCatalog();
        var resolvedLanguage = NormalizeLanguage(language, catalog.DefaultLocale);
        var defaultLocale = NormalizeLanguage(catalog.DefaultLocale, "vi");

        var systemPrompt = ResolveLocalizedText(
            catalog.System,
            resolvedLanguage,
            defaultLocale,
            "You are a mystical, wise, and empathetic Tarot Reader.");
        var promptTemplate = string.IsNullOrWhiteSpace(followupQuestion)
            ? ResolvePromptTemplate(catalog.Initial, session.SpreadType, resolvedLanguage, defaultLocale)
            : ResolvePromptTemplate(catalog.Followup, session.SpreadType, resolvedLanguage, defaultLocale);
        var defaultQuestion = ResolveLocalizedText(
            catalog.Context.DefaultQuestion,
            resolvedLanguage,
            defaultLocale,
            "A general reading about my current life.");
        var questionContext = string.IsNullOrWhiteSpace(session.Question)
            ? defaultQuestion
            : session.Question.Trim();
        var cardsContext = BuildDrawnCardsContext(session.CardsDrawn, resolvedLanguage, defaultLocale, catalog.Context);
        var userPrompt = RenderTemplate(
            promptTemplate,
            questionContext,
            session.SpreadType,
            cardsContext,
            followupQuestion);

        return new ReadingPromptBundle
        {
            SystemPrompt = systemPrompt,
            UserPrompt = userPrompt,
            ResolvedLanguage = resolvedLanguage
        };
    }

    private ReadingPromptCatalog ResolveCatalog()
    {
        var normalizedKey = SystemConfigRegistry.NormalizeKey(PromptConfigKey);
        if (_snapshotStore.TryGet(normalizedKey, out var item) == false
            || string.IsNullOrWhiteSpace(item.Value))
        {
            return FallbackCatalog;
        }

        lock (_sync)
        {
            if (_cachedCatalog is not null
                && _cachedCatalog.UpdatedAt == item.UpdatedAt
                && string.Equals(_cachedCatalog.RawValue, item.Value, StringComparison.Ordinal))
            {
                return _cachedCatalog.Catalog;
            }

            return ParseAndCacheCatalog(item.Value, item.UpdatedAt, normalizedKey);
        }
    }

    private ReadingPromptCatalog ParseAndCacheCatalog(string rawValue, DateTime updatedAt, string normalizedKey)
    {
        try
        {
            var parsed = JsonSerializer.Deserialize<ReadingPromptCatalog>(rawValue, JsonOptions);
            if (parsed is null)
            {
                return FallbackCatalog;
            }

            var normalized = NormalizeCatalog(parsed);
            _cachedCatalog = new CachedCatalog(updatedAt, rawValue, normalized);
            return normalized;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                exception,
                "Failed to parse reading prompt catalog from system config key {Key}. Falling back to in-memory default.",
                normalizedKey);
            return FallbackCatalog;
        }
    }
}
