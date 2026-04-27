using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using TarotNow.Application.Features.Wallet.Queries;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

// DTO tổng hợp metadata khởi tạo cho client sau đăng nhập.
public class UserMetadataDto
{
    // Thông tin số dư ví hiện tại của user.
    public WalletBalanceDto Wallet { get; set; } = null!;

    // Trạng thái streak check-in hiện tại.
    public StreakStatusResult Streak { get; set; } = null!;

    // Tổng số thông báo chưa đọc.
    public long UnreadNotificationCount { get; set; }

    // Danh sách thông báo gần đây theo phân trang mặc định.
    public NotificationListResponse RecentNotifications { get; set; } = null!;

    // Tổng số tin nhắn chat chưa đọc.
    public long UnreadChatCount { get; set; }

    // Danh sách hội thoại active để hiển thị nhanh ở trang chính.
    public ListConversationsResult ActiveConversations { get; set; } = null!;
}
