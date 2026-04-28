using System.Text.Json;

namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static (bool IsValid, string? Error) ValidateLocaleMap(JsonElement localeMap, string path)
    {
        if (localeMap.ValueKind != JsonValueKind.Object)
        {
            return (false, $"{path} must be a JSON object.");
        }

        foreach (var locale in PromptLocales)
        {
            if (!TryGetPropertyCaseInsensitive(localeMap, locale, out var localizedValue))
            {
                return (false, $"{path}.{locale} is required.");
            }

            var valueValidation = ValidateStringElement(localizedValue, $"{path}.{locale}");
            if (!valueValidation.IsValid)
            {
                return valueValidation;
            }
        }

        return (true, null);
    }

    private static (bool IsValid, string? Error) ValidateRequiredTextField(JsonElement parent, string propertyName)
    {
        if (!TryGetPropertyCaseInsensitive(parent, propertyName, out var value))
        {
            return (false, $"{propertyName} is required.");
        }

        return ValidateStringElement(value, propertyName);
    }

    private static (bool IsValid, string? Error) ValidatePromptValueElement(JsonElement value, string path)
    {
        if (value.ValueKind == JsonValueKind.String)
        {
            return ValidateStringElement(value, path);
        }

        if (value.ValueKind == JsonValueKind.Object)
        {
            return ValidateLocaleMap(value, path);
        }

        return (false, $"{path} must be either string or locale object.");
    }

    private static (bool IsValid, string? Error) ValidateStringElement(JsonElement value, string path)
    {
        if (value.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(value.GetString()))
        {
            return (false, $"{path} must be a non-empty string.");
        }

        return (true, null);
    }

    private static (bool IsValid, string? Error) TryGetRequiredObject(
        JsonElement parent,
        string propertyName,
        out JsonElement objectElement)
    {
        if (!TryGetPropertyCaseInsensitive(parent, propertyName, out objectElement))
        {
            return (false, $"{propertyName} is required.");
        }

        if (objectElement.ValueKind != JsonValueKind.Object)
        {
            return (false, $"{propertyName} must be a JSON object.");
        }

        return (true, null);
    }

    private static bool TryGetPropertyCaseInsensitive(
        JsonElement objectElement,
        string propertyName,
        out JsonElement value)
    {
        if (objectElement.ValueKind != JsonValueKind.Object)
        {
            value = default;
            return false;
        }

        if (objectElement.TryGetProperty(propertyName, out value))
        {
            return true;
        }

        foreach (var property in objectElement.EnumerateObject())
        {
            if (!string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            value = property.Value;
            return true;
        }

        value = default;
        return false;
    }
}
