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
                DefaultQuestion = NormalizeNestedMap(catalog.Context.DefaultQuestion),
                Orientation = new ReadingPromptOrientation
                {
                    Upright = NormalizeNestedMap(catalog.Context.Orientation.Upright),
                    Reversed = NormalizeNestedMap(catalog.Context.Orientation.Reversed)
                },
                UnknownCardLabel = NormalizeNestedMap(catalog.Context.UnknownCardLabel)
            }
        };
    }

    private static Dictionary<string, Dictionary<string, string>> NormalizePromptTemplates(
        Dictionary<string, Dictionary<string, string>> templates)
    {
        var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in templates)
        {
            var key = pair.Key?.Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            result[key.ToLowerInvariant()] = NormalizeNestedMap(pair.Value);
        }

        return result;
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

    private static Dictionary<string, Dictionary<string, string>> BuildFallbackInitialPrompts()
    {
        return new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["default"] = BuildFallbackTemplateLocaleMap(
                """My question: "{{question}}". Interpret this reading for me. Spread Type: {{spread_type}}. Cards Chosen: {{cards_context}}""")
        };
    }

    private static Dictionary<string, Dictionary<string, string>> BuildFallbackFollowupPrompts()
    {
        return new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["default"] = BuildFallbackTemplateLocaleMap(
                """Based on my previous reading (Question: "{{question}}", Spread: {{spread_type}}, Cards: {{cards_context}}), answer my follow-up question: {{followup_question}}""")
        };
    }

    private static Dictionary<string, string> BuildFallbackTemplateLocaleMap(string template)
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["vi"] = template,
            ["en"] = template,
            ["zh"] = template
        };
    }

    private static ReadingPromptContext BuildFallbackContext()
    {
        return new ReadingPromptContext
        {
            DefaultQuestion = BuildFallbackTemplateLocaleMap("A general reading about my current life."),
            Orientation = new ReadingPromptOrientation
            {
                Upright = BuildFallbackTemplateLocaleMap("Upright"),
                Reversed = BuildFallbackTemplateLocaleMap("Reversed")
            },
            UnknownCardLabel = BuildFallbackTemplateLocaleMap("Unknown Card")
        };
    }
}
