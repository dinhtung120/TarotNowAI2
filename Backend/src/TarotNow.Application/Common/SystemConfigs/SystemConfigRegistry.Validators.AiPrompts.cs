using System.Text.Json;

namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static readonly string[] PromptLocales = ["vi", "en", "zh"];

    private static (bool IsValid, string? Error) ValidateReadingPromptsJson(string rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return (false, "Prompt catalog JSON cannot be empty.");
        }

        try
        {
            using var document = JsonDocument.Parse(rawValue);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return (false, "Prompt catalog must be a JSON object.");
            }

            var rootValidation = ValidateRootPromptSections(root);
            if (!rootValidation.IsValid)
            {
                return rootValidation;
            }

            return ValidatePromptContext(root);
        }
        catch (Exception ex)
        {
            return (false, $"Invalid prompt catalog JSON: {ex.Message}");
        }
    }

    private static (bool IsValid, string? Error) ValidateRootPromptSections(JsonElement root)
    {
        var localeValidation = ValidateRequiredTextField(root, "defaultLocale");
        if (!localeValidation.IsValid)
        {
            return localeValidation;
        }

        var systemValidation = ValidateRequiredLocaleSection(root, "system");
        if (!systemValidation.IsValid)
        {
            return systemValidation;
        }

        var initialValidation = ValidateDefaultTemplateSection(root, "initial");
        if (!initialValidation.IsValid)
        {
            return initialValidation;
        }

        return ValidateDefaultTemplateSection(root, "followup");
    }

    private static (bool IsValid, string? Error) ValidateDefaultTemplateSection(
        JsonElement root,
        string sectionName)
    {
        var sectionValidation = TryGetRequiredObject(root, sectionName, out var section);
        if (!sectionValidation.IsValid)
        {
            return sectionValidation;
        }

        var defaultValidation = TryGetRequiredObject(section, "default", out var defaultTemplate);
        if (!defaultValidation.IsValid)
        {
            return defaultValidation;
        }

        return ValidateLocaleMap(defaultTemplate, $"{sectionName}.default");
    }

    private static (bool IsValid, string? Error) ValidatePromptContext(JsonElement root)
    {
        var contextValidation = TryGetRequiredObject(root, "context", out var context);
        if (!contextValidation.IsValid)
        {
            return contextValidation;
        }

        var defaultQuestionValidation = ValidateRequiredLocaleSection(context, "defaultQuestion", "context.defaultQuestion");
        if (!defaultQuestionValidation.IsValid)
        {
            return defaultQuestionValidation;
        }

        var orientationValidation = ValidateOrientationSection(context);
        if (!orientationValidation.IsValid)
        {
            return orientationValidation;
        }

        return ValidateOptionalUnknownCardSection(context);
    }

    private static (bool IsValid, string? Error) ValidateOrientationSection(JsonElement context)
    {
        var orientationValidation = TryGetRequiredObject(context, "orientation", out var orientation);
        if (!orientationValidation.IsValid)
        {
            return orientationValidation;
        }

        var uprightValidation = ValidateRequiredLocaleSection(orientation, "upright", "context.orientation.upright");
        if (!uprightValidation.IsValid)
        {
            return uprightValidation;
        }

        return ValidateRequiredLocaleSection(orientation, "reversed", "context.orientation.reversed");
    }

    private static (bool IsValid, string? Error) ValidateOptionalUnknownCardSection(JsonElement context)
    {
        if (!TryGetPropertyCaseInsensitive(context, "unknownCardLabel", out var unknownCardLabel))
        {
            return (true, null);
        }

        if (unknownCardLabel.ValueKind == JsonValueKind.Null)
        {
            return (true, null);
        }

        if (unknownCardLabel.ValueKind != JsonValueKind.Object)
        {
            return (false, "context.unknownCardLabel must be a JSON object.");
        }

        return ValidateLocaleMap(unknownCardLabel, "context.unknownCardLabel");
    }

    private static (bool IsValid, string? Error) ValidateRequiredLocaleSection(
        JsonElement parent,
        string propertyName,
        string? explicitPath = null)
    {
        var objectValidation = TryGetRequiredObject(parent, propertyName, out var localeMap);
        if (!objectValidation.IsValid)
        {
            return objectValidation;
        }

        return ValidateLocaleMap(localeMap, explicitPath ?? propertyName);
    }
}
