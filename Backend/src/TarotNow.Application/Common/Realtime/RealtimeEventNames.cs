namespace TarotNow.Application.Common.Realtime;

/// <summary>
/// Tên sự kiện realtime chuẩn dùng cho Redis bridge và SignalR contract.
/// </summary>
public static class RealtimeEventNames
{
    /// <summary>
    /// Sự kiện có notification mới.
    /// </summary>
    public const string NotificationNew = "notification.new";

    /// <summary>
    /// Sự kiện thay đổi số dư ví.
    /// </summary>
    public const string WalletBalanceChanged = "wallet.balance_changed";

    /// <summary>
    /// Sự kiện có tin nhắn chat mới.
    /// </summary>
    public const string ChatMessageCreated = "message.created";

    /// <summary>
    /// Sự kiện conversation được cập nhật.
    /// </summary>
    public const string ConversationUpdated = "conversation.updated";

    /// <summary>
    /// Sự kiện số lượng unread chat thay đổi.
    /// </summary>
    public const string ChatUnreadChanged = "chat.unread_changed";

    /// <summary>
    /// Sự kiện mark-read tin nhắn.
    /// </summary>
    public const string ChatMessageRead = "message.read";

    /// <summary>
    /// Sự kiện bắt đầu gõ tin nhắn.
    /// </summary>
    public const string TypingStarted = "typing.started";

    /// <summary>
    /// Sự kiện dừng gõ tin nhắn.
    /// </summary>
    public const string TypingStopped = "typing.stopped";

    /// <summary>
    /// Sự kiện kết quả quay gacha.
    /// </summary>
    public const string GachaResult = "gacha.result";

    /// <summary>
    /// Sự kiện hoàn thành quest.
    /// </summary>
    public const string GamificationQuestCompleted = "gamification.quest_completed";

    /// <summary>
    /// Sự kiện mở khóa achievement.
    /// </summary>
    public const string GamificationAchievementUnlocked = "gamification.achievement_unlocked";

    /// <summary>
    /// Sự kiện thăng cấp thẻ bài.
    /// </summary>
    public const string GamificationCardLevelUp = "gamification.card_level_up";

    /// <summary>
    /// Sự kiện inventory thay đổi.
    /// </summary>
    public const string InventoryChanged = "inventory.changed";

    /// <summary>
    /// Sự kiện free-draw quota thay đổi.
    /// </summary>
    public const string ReadingQuotaChanged = "reading.quota_changed";

    /// <summary>
    /// Sự kiện profile thay đổi.
    /// </summary>
    public const string ProfileChanged = "profile.changed";

    /// <summary>
    /// Sự kiện title thay đổi.
    /// </summary>
    public const string TitleChanged = "title.changed";

    /// <summary>
    /// Sự kiện thay đổi trạng thái hiện diện người dùng.
    /// </summary>
    public const string UserStatusChanged = "user.status_changed";
}
