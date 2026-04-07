using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Wallet.Queries;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

public class UserMetadataDto
{
        public WalletBalanceDto Wallet { get; set; } = null!;

        public StreakStatusResult Streak { get; set; } = null!;

        public int UnreadNotificationCount { get; set; }

        public NotificationListResponse RecentNotifications { get; set; } = null!;

        public int UnreadChatCount { get; set; }

        public ListConversationsResult ActiveConversations { get; set; } = null!;
}
