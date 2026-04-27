using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;

namespace TarotNow.Application.Features.UserContext.Queries.GetNavbarSnapshot;

/// <summary>
/// Snapshot gọn cho navbar: đếm unread, streak, preview dropdown thông báo (một round-trip).
/// </summary>
public class NavbarSnapshotDto
{
    public long UnreadNotificationCount { get; set; }

    public long UnreadChatCount { get; set; }

    public StreakStatusResult Streak { get; set; } = null!;

    public NotificationListResponse DropdownPreview { get; set; } = null!;
}
