using System.Globalization;
using System.Text.Json;
using TarotNow.Application.Common.SystemConfigs;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    private string? ReadString(string key)
    {
        if (_snapshotStore.TryGetValue(SystemConfigRegistry.NormalizeKey(key), out var value))
        {
            return value;
        }

        return null;
    }

    private int ReadInt(IEnumerable<string> keys, int fallback)
    {
        foreach (var key in keys)
        {
            var raw = ReadString(key);
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed;
            }
        }

        return fallback;
    }

    private long ReadLong(IEnumerable<string> keys, long fallback)
    {
        foreach (var key in keys)
        {
            var raw = ReadString(key);
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            if (long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed;
            }
        }

        return fallback;
    }

    private decimal? ReadDecimal(string key)
    {
        var raw = ReadString(key);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        return decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;
    }

    /// <summary>
    /// Chuẩn hóa số nguyên dương cho các tham số giới hạn.
    /// </summary>
    private static int ResolvePositiveInt(int configuredValue, int fallback)
    {
        return configuredValue > 0 ? configuredValue : fallback;
    }

    /// <summary>
    /// Chuẩn hóa số không âm cho các tham số chi phí.
    /// </summary>
    private static long ResolveNonNegativeLong(long configuredValue, long fallback)
    {
        return configuredValue >= 0 ? configuredValue : fallback;
    }

    private static long ResolvePositiveLong(long configuredValue, long fallback)
    {
        return configuredValue > 0 ? configuredValue : fallback;
    }

    private static decimal ClampDecimal(decimal value, decimal min, decimal max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }

    private static IReadOnlyList<int> NormalizeDistinctPositiveOrderedList(
        IEnumerable<int> values,
        int minInclusive,
        int maxInclusive)
    {
        return values
            .Where(v => v >= minInclusive && v <= maxInclusive)
            .Distinct()
            .OrderBy(v => v)
            .ToArray();
    }

    private IReadOnlyList<int> ReadIntArray(string key)
    {
        var raw = ReadString(key);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<int[]>(raw, JsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }
}
