namespace TarotNow.Api.Realtime;

/// <summary>
/// Tên group SignalR dùng cho presence domain.
/// </summary>
internal static class PresenceGroupNames
{
    /// <summary>
    /// Group cá nhân của user để nhận sự kiện realtime theo user id.
    /// </summary>
    public static string User(string userId) => $"user:{userId}";

    /// <summary>
    /// Group observer theo từng user để client khác theo dõi trạng thái online/offline.
    /// </summary>
    public static string UserStatusObservers(string userId) => $"presence:watch:user:{userId}";
}
