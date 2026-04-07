namespace TarotNow.Infrastructure.Constants;

internal static class ApiPathConstants
{
    private const string Prefix = "/api/v1";
    public const string ChatHub = Prefix + "/chat";
    public const string PresenceHub = Prefix + "/presence";
    public const string Sessions = Prefix + "/sessions";
    
    public const string CallHub = Prefix + "/call";
}
