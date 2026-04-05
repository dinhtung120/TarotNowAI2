namespace TarotNow.Api.Constants;

/// <summary>
/// Reader presence states used by API/presentation layer.
/// </summary>
public static class ApiReaderStatusConstants
{
    public const string Online = "online";
    public const string Offline = "offline";
    public const string Busy = "busy";

    private static readonly IReadOnlyDictionary<string, string> AliasToStatus =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [Online] = Online,
            ["active"] = Online,
            ["connected"] = Online,
            ["available"] = Online,
            [Offline] = Offline,
            ["disconnected"] = Offline,
            ["invisible"] = Offline,
            [Busy] = Busy,
            ["away"] = Busy,
            ["idle"] = Busy,
            ["accepting_questions"] = Busy,
            ["acceptingquestions"] = Busy,
            ["accepting-questions"] = Busy,
            ["accepting"] = Busy,
            ["ready"] = Busy
        };

    public static bool TryNormalize(string? status, out string normalized)
    {
        normalized = Offline;
        if (string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(status.Trim()))
        {
            return false;
        }

        if (!AliasToStatus.TryGetValue(status.Trim(), out var mapped))
        {
            return false;
        }

        normalized = mapped;
        return true;
    }

    public static string NormalizeOrDefault(string? status, string defaultStatus = Offline)
        => TryNormalize(status, out var normalized) ? normalized : defaultStatus;
}
