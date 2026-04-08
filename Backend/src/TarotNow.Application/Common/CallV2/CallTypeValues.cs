namespace TarotNow.Application.Common;

public static class CallTypeValues
{
    public const string Audio = "audio";

    public const string Video = "video";

    public static bool IsSupported(string value)
    {
        return string.Equals(value, Audio, StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, Video, StringComparison.OrdinalIgnoreCase);
    }

    public static string Normalize(string value)
    {
        return string.Equals(value, Video, StringComparison.OrdinalIgnoreCase)
            ? Video
            : Audio;
    }
}
