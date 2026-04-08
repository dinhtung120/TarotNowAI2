namespace TarotNow.Application.Common;

public static class CallSessionV2Statuses
{
    public const string Requested = "requested";
    public const string Accepted = "accepted";
    public const string Joining = "joining";
    public const string Connected = "connected";
    public const string Ending = "ending";
    public const string Ended = "ended";
    public const string Failed = "failed";

    public static readonly string[] ActiveStates =
    [
        Requested,
        Accepted,
        Joining,
        Connected,
        Ending
    ];

    public static bool IsActive(string status)
    {
        return ActiveStates.Contains(status, StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsFinal(string status)
    {
        return string.Equals(status, Ended, StringComparison.OrdinalIgnoreCase)
            || string.Equals(status, Failed, StringComparison.OrdinalIgnoreCase);
    }
}
