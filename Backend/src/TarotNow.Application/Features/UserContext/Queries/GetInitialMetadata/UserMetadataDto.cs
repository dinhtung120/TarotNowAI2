using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

/// <summary>
/// DTO chứa toàn bộ thông tin cần thiết để khởi tạo Dashboard/Navbar (Gộp request).
/// TỐI ƯU HÓA: Bổ sung Notification List để triệt tiêu request lẻ.
/// </summary>
public class UserMetadataDto
{
    /// <summary>
    /// Số dư ví (Vàng, Kim cương, Kim cương đóng băng).
    /// </summary>
    public WalletBalanceDto Wallet { get; set; } = null!;

    /// <summary>
    /// Trạng thái Streak (Chuỗi ngày điểm danh).
    /// </summary>
    public StreakStatusResult Streak { get; set; } = null!;

    /// <summary>
    /// Số lượng thông báo hệ thống chưa đọc.
    /// </summary>
    public int UnreadNotificationCount { get; set; }

    /// <summary>
    /// 10 thông báo gần nhất để mồi (prime) cho Dropdown trên Navbar.
    /// </summary>
    public NotificationListResponse RecentNotifications { get; set; } = null!;

    /// <summary>
    /// Số lượng cuộc hội thoại chat có tin nhắn mới.
    /// </summary>
    public int UnreadChatCount { get; set; }

    /// <summary>
    /// Danh sách 100 cuộc hội thoại "Active" gần nhất để mồi (prime) Inbox.
    /// </summary>
    public ListConversationsResult ActiveConversations { get; set; } = null!;
}
