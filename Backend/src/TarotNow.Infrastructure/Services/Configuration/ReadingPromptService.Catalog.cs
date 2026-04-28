using System.Text.Json;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class ReadingPromptService
{
    private static ReadingPromptCatalog NormalizeCatalog(ReadingPromptCatalog catalog)
    {
        return new ReadingPromptCatalog
        {
            DefaultLocale = NormalizeLanguage(catalog.DefaultLocale, "vi"),
            System = NormalizeNestedMap(catalog.System),
            Initial = NormalizePromptTemplates(catalog.Initial),
            Followup = NormalizePromptTemplates(catalog.Followup),
            Context = new ReadingPromptContext
            {
                DefaultQuestion = NormalizePromptValue(catalog.Context.DefaultQuestion),
                Orientation = new ReadingPromptOrientation
                {
                    Upright = NormalizePromptValue(catalog.Context.Orientation.Upright),
                    Reversed = NormalizePromptValue(catalog.Context.Orientation.Reversed)
                },
                UnknownCardLabel = NormalizePromptValue(catalog.Context.UnknownCardLabel)
            }
        };
    }

    private static Dictionary<string, JsonElement> NormalizePromptTemplates(Dictionary<string, JsonElement> templates)
    {
        var result = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in templates)
        {
            var key = pair.Key?.Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            result[key.ToLowerInvariant()] = NormalizePromptValue(pair.Value);
        }

        return result;
    }

    private static JsonElement NormalizePromptValue(JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.String)
        {
            var normalized = value.GetString()?.Trim();
            return JsonSerializer.SerializeToElement(normalized ?? string.Empty);
        }

        if (value.ValueKind != JsonValueKind.Object)
        {
            return default;
        }

        var normalizedMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in value.EnumerateObject())
        {
            if (property.Value.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            var propertyName = property.Name.Trim().ToLowerInvariant();
            var propertyValue = property.Value.GetString()?.Trim();
            if (string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrWhiteSpace(propertyValue))
            {
                continue;
            }

            normalizedMap[propertyName] = propertyValue;
        }

        return JsonSerializer.SerializeToElement(normalizedMap);
    }

    private static Dictionary<string, string> NormalizeNestedMap(Dictionary<string, string> map)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in map)
        {
            var key = pair.Key?.Trim();
            var value = pair.Value?.Trim();
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            result[key.ToLowerInvariant()] = value;
        }

        return result;
    }

    private static ReadingPromptCatalog BuildFallbackCatalog()
    {
        return new ReadingPromptCatalog
        {
            DefaultLocale = "vi",
            System = BuildFallbackSystemPrompts(),
            Initial = BuildFallbackInitialPrompts(),
            Followup = BuildFallbackFollowupPrompts(),
            Context = BuildFallbackContext()
        };
    }

    private static Dictionary<string, string> BuildFallbackSystemPrompts()
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["vi"] = "You are a mystical, wise, and empathetic Tarot Reader.\nFormat your response clearly using Markdown.\nYou give highly accurate and deeply personalized readings.\nYou MUST reply purely in Vietnamese (Tiếng Việt).",
            ["en"] = "You are a mystical, wise, and empathetic Tarot Reader.\nFormat your response clearly using Markdown.\nYou give highly accurate and deeply personalized readings.\nYou MUST reply purely in English.",
            ["zh"] = "You are a mystical, wise, and empathetic Tarot Reader.\nFormat your response clearly using Markdown.\nYou give highly accurate and deeply personalized readings.\nYou MUST reply purely in Chinese (繁體中文)."
        };
    }

    private static Dictionary<string, JsonElement> BuildFallbackInitialPrompts()
    {
        return new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase)
        {
            ["default"] = CreateJsonString("""My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}"""),
            ["daily_1"] = CreateJsonString("""My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}"""),
            ["spread_3"] = CreateJsonString("""My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}"""),
            ["spread_5"] = CreateJsonString("""My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}"""),
            ["spread_10"] = CreateJsonString("""My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}""")
        };
    }

    private static Dictionary<string, JsonElement> BuildFallbackFollowupPrompts()
    {
        return new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase)
        {
            ["default"] = CreateJsonString("""Based on my previous reading (Question: "{{question}}", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}""")
        };
    }

    private static ReadingPromptContext BuildFallbackContext()
    {
        return new ReadingPromptContext
        {
            DefaultQuestion = CreateJsonString("A general reading about my current life."),
            Orientation = new ReadingPromptOrientation
            {
                Upright = CreateJsonString("Upright"),
                Reversed = CreateJsonString("Reversed")
            },
            UnknownCardLabel = CreateJsonString("Unknown Card")
        };
    }

    private static JsonElement CreateJsonString(string value)
    {
        return JsonSerializer.SerializeToElement(value);
    }
}
