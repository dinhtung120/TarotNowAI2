namespace TarotNow.Application.Common.Realtime;

/// <summary>
/// Tên các Redis Pub/Sub channel realtime chuẩn toàn hệ thống.
/// </summary>
public static class RealtimeChannelNames
{
    /// <summary>
    /// Kênh notification realtime.
    /// </summary>
    public const string Notifications = "realtime:notifications";

    /// <summary>
    /// Kênh wallet realtime.
    /// </summary>
    public const string Wallet = "realtime:wallet";

    /// <summary>
    /// Kênh chat realtime.
    /// </summary>
    public const string Chat = "realtime:chat";

    /// <summary>
    /// Kênh gacha realtime.
    /// </summary>
    public const string Gacha = "realtime:gacha";

    /// <summary>
    /// Kênh gamification realtime.
    /// </summary>
    public const string Gamification = "realtime:gamification";
}
