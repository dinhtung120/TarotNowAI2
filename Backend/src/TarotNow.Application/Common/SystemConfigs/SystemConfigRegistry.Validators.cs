using System.Globalization;
using System.Text.Json;

namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static Func<string, (bool IsValid, string? Error)> ValidateInt(int min, int max)
    {
        return rawValue =>
        {
            if (!int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return (false, "Value must be an integer.");
            }

            return parsed < min || parsed > max
                ? (false, $"Value must be between {min} and {max}.")
                : (true, null);
        };
    }

    private static Func<string, (bool IsValid, string? Error)> ValidateLong(long min, long max)
    {
        return rawValue =>
        {
            if (!long.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return (false, "Value must be a whole number.");
            }

            return parsed < min || parsed > max
                ? (false, $"Value must be between {min} and {max}.")
                : (true, null);
        };
    }

    private static Func<string, (bool IsValid, string? Error)> ValidateDecimal(decimal min, decimal max)
    {
        return rawValue =>
        {
            if (!decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
            {
                return (false, "Value must be a decimal number.");
            }

            return parsed < min || parsed > max
                ? (false, $"Value must be between {min.ToString(CultureInfo.InvariantCulture)} and {max.ToString(CultureInfo.InvariantCulture)}.")
                : (true, null);
        };
    }

    private static (bool IsValid, string? Error) ValidateJson(string rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return (false, "JSON value cannot be empty.");
        }

        try
        {
            using var _ = JsonDocument.Parse(rawValue);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"Invalid JSON: {ex.Message}");
        }
    }

    private static Func<string, (bool IsValid, string? Error)> ValidateIntArray(int minLength, int maxLength)
    {
        return rawValue =>
        {
            try
            {
                var values = JsonSerializer.Deserialize<int[]>(rawValue, JsonOptions);
                if (values is null || values.Length < minLength || values.Length > maxLength)
                {
                    return (false, $"Array length must be between {minLength} and {maxLength}.");
                }

                return values.Any(v => v < 0)
                    ? (false, "Array values must be non-negative.")
                    : (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Invalid integer array JSON: {ex.Message}");
            }
        };
    }

    private static Func<string, (bool IsValid, string? Error)> ValidateIntArrayRange(
        int minLength,
        int maxLength,
        int minValue,
        int maxValue)
    {
        return rawValue =>
        {
            try
            {
                var values = JsonSerializer.Deserialize<int[]>(rawValue, JsonOptions);
                if (values is null || values.Length < minLength || values.Length > maxLength)
                {
                    return (false, $"Array length must be between {minLength} and {maxLength}.");
                }

                return values.Any(v => v < minValue || v > maxValue)
                    ? (false, $"Array values must be between {minValue} and {maxValue}.")
                    : (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Invalid integer array JSON: {ex.Message}");
            }
        };
    }
}
