namespace TarotNow.Infrastructure.Constants;

// Hằng đường dẫn API dùng chung để tránh hard-code lặp và giữ nhất quán routing realtime.
internal static class ApiPathConstants
{
    // Prefix version API hiện hành.
    private const string Prefix = "/api/v1";

    // Endpoint hub chat realtime.
    public const string ChatHub = Prefix + "/chat";

    // Endpoint hub presence realtime.
    public const string PresenceHub = Prefix + "/presence";

    // Prefix endpoint phiên đọc bài.
    public const string Sessions = Prefix + "/sessions";

    // Endpoint hub cuộc gọi realtime.
    public const string CallHub = Prefix + "/call";
}
