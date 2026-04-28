using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using System.Text.Json;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class ReadingPromptService
{
    private static string ResolvePromptTemplate(
        Dictionary<string, JsonElement> promptMap,
        string? spreadType,
        string locale,
        string defaultLocale)
    {
        var normalizedSpread = string.IsNullOrWhiteSpace(spreadType)
            ? "default"
            : spreadType.Trim().ToLowerInvariant();

        if (promptMap.TryGetValue(normalizedSpread, out var spreadTemplate))
        {
            return ResolvePromptValue(spreadTemplate, locale, defaultLocale, string.Empty);
        }

        if (promptMap.TryGetValue("default", out var defaultTemplate))
        {
            return ResolvePromptValue(defaultTemplate, locale, defaultLocale, string.Empty);
        }

        return string.Empty;
    }

    private static string ResolvePromptValue(
        JsonElement promptValue,
        string locale,
        string defaultLocale,
        string fallback)
    {
        if (promptValue.ValueKind == JsonValueKind.String)
        {
            var value = promptValue.GetString();
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }

        if (promptValue.ValueKind != JsonValueKind.Object)
        {
            return fallback;
        }

        foreach (var candidate in BuildLocaleCandidates(locale, defaultLocale))
        {
            if (TryReadLocaleFromObject(promptValue, candidate, out var localized))
            {
                return localized;
            }
        }

        return fallback;
    }

    private static string ResolveLocalizedText(
        Dictionary<string, string> localizedMap,
        string locale,
        string defaultLocale,
        string fallback)
    {
        foreach (var candidate in BuildLocaleCandidates(locale, defaultLocale))
        {
            if (localizedMap.TryGetValue(candidate, out var localized)
                && string.IsNullOrWhiteSpace(localized) == false)
            {
                return localized.Trim();
            }
        }

        return fallback;
    }

    private static string BuildDrawnCardsContext(
        string? cardsDrawn,
        string locale,
        string defaultLocale,
        ReadingPromptContext context)
    {
        var cards = ReadingDrawnCardCodec.Parse(cardsDrawn);
        var unknownCardLabel = ResolvePromptValue(context.UnknownCardLabel, locale, defaultLocale, "Unknown Card");
        var uprightLabel = ResolvePromptValue(context.Orientation.Upright, locale, defaultLocale, "Upright");
        var reversedLabel = ResolvePromptValue(context.Orientation.Reversed, locale, defaultLocale, "Reversed");

        return string.Join(", ", cards.Select(card =>
            FormatCardContextItem(card, unknownCardLabel, uprightLabel, reversedLabel)));
    }

    private static string FormatCardContextItem(
        ReadingDrawnCard card,
        string unknownCardLabel,
        string uprightLabel,
        string reversedLabel)
    {
        var cardName = ResolveCardName(card.CardId, unknownCardLabel);
        var orientationLabel = ResolveOrientationLabel(card.Orientation, uprightLabel, reversedLabel);
        return $"[Position {card.Position + 1}: {cardName} ({orientationLabel})]";
    }

    private static string ResolveCardName(int cardId, string unknownCardLabel)
    {
        return cardId >= 0 && cardId < CardNames.Length
            ? CardNames[cardId]
            : unknownCardLabel;
    }

    private static string ResolveOrientationLabel(string orientation, string uprightLabel, string reversedLabel)
    {
        return string.Equals(orientation, CardOrientation.Reversed, StringComparison.Ordinal)
            ? reversedLabel
            : uprightLabel;
    }

    private static string RenderTemplate(
        string template,
        string question,
        string spreadType,
        string cardsContext,
        string? followupQuestion)
    {
        return template
            .Replace("{{question}}", question, StringComparison.Ordinal)
            .Replace("{{spread_type}}", spreadType, StringComparison.Ordinal)
            .Replace("{{cards_context}}", cardsContext, StringComparison.Ordinal)
            .Replace("{{followup_question}}", followupQuestion?.Trim() ?? string.Empty, StringComparison.Ordinal);
    }

    private static string NormalizeLanguage(string? language, string fallback)
    {
        var requested = NormalizeLocaleCode(language);
        if (IsSupportedLocale(requested))
        {
            return requested;
        }

        var normalizedFallback = NormalizeLocaleCode(fallback);
        return IsSupportedLocale(normalizedFallback) ? normalizedFallback : "vi";
    }

    private static IEnumerable<string> BuildLocaleCandidates(string locale, string defaultLocale)
    {
        yield return locale;
        yield return defaultLocale;
        yield return "vi";
        yield return "en";
        yield return "zh";
    }

    private static string NormalizeLocaleCode(string? locale)
    {
        return string.IsNullOrWhiteSpace(locale)
            ? string.Empty
            : locale.Trim().ToLowerInvariant();
    }

    private static bool IsSupportedLocale(string locale)
    {
        return locale is "vi" or "en" or "zh";
    }

    private static bool TryReadLocaleFromObject(
        JsonElement localeObject,
        string locale,
        out string localized)
    {
        if (localeObject.TryGetProperty(locale, out var value)
            && value.ValueKind == JsonValueKind.String
            && string.IsNullOrWhiteSpace(value.GetString()) == false)
        {
            localized = value.GetString()!.Trim();
            return true;
        }

        foreach (var property in localeObject.EnumerateObject())
        {
            if (!string.Equals(property.Name, locale, StringComparison.OrdinalIgnoreCase)
                || property.Value.ValueKind != JsonValueKind.String
                || string.IsNullOrWhiteSpace(property.Value.GetString()))
            {
                continue;
            }

            localized = property.Value.GetString()!.Trim();
            return true;
        }

        localized = string.Empty;
        return false;
    }
}
