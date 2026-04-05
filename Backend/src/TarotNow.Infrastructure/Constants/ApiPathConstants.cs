namespace TarotNow.Infrastructure.Constants;

internal static class ApiPathConstants
{
    private const string Prefix = "/api/v1";
    public const string ChatHub = Prefix + "/chat";
    public const string PresenceHub = Prefix + "/presence";
    public const string Sessions = Prefix + "/sessions";
    /* FIX #23: Thêm path cho CallHub.
     * Trước đây thiếu constant này → ResolveBearerTokenAsync không đọc access_token
     * từ query string cho CallHub → WebSocket bị reject 401 → "connection not found". */
    public const string CallHub = Prefix + "/call";
}
