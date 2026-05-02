using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

public partial class ListMessagesQueryHandler
{
    /// <summary>
    /// Enrich profile của user và reader cho conversation.
    /// Luồng xử lý: thu thập Guid participant hợp lệ, tải user map theo lô, rồi gán display name/avatar vào conversation.
    /// </summary>
    private async Task EnrichParticipantProfilesAsync(
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        var userIds = new HashSet<Guid>();
        if (Guid.TryParse(conversation.UserId, out var userId))
        {
            userIds.Add(userId);
        }

        if (Guid.TryParse(conversation.ReaderId, out var readerId))
        {
            userIds.Add(readerId);
        }

        if (userIds.Count == 0)
        {
            // Edge case: participant id không parse được Guid nên không thể enrich profile.
            return;
        }

        var userMap = await _userRepo.GetUserBasicInfoMapAsync(userIds, cancellationToken);
        if (userMap.TryGetValue(userId, out var userInfo))
        {
            conversation.UserName = userInfo.DisplayName;
            conversation.UserAvatar = userInfo.AvatarUrl;
        }

        if (userMap.TryGetValue(readerId, out var readerInfo))
        {
            conversation.ReaderName = readerInfo.DisplayName;
            conversation.ReaderAvatar = readerInfo.AvatarUrl;
        }
    }

    /// <summary>
    /// Enrich trạng thái reader và thông tin escrow của conversation.
    /// Luồng xử lý: tải reader profile để xác định status online/offline, sau đó tải finance session để gán frozen/status.
    /// </summary>
    private async Task EnrichReaderStatusAndEscrowAsync(
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(conversation.ReaderId, cancellationToken);
        if (readerProfile != null)
        {
            var status = readerProfile.Status;
            if (_presenceTracker.IsOnline(readerProfile.UserId))
            {
                if (string.Equals(status, ReaderOnlineStatus.Offline, StringComparison.OrdinalIgnoreCase))
                {
                    // Ưu tiên trạng thái realtime online khi presence tracker xác nhận đang kết nối.
                    status = ReaderOnlineStatus.Online;
                }
            }

            conversation.ReaderStatus = status;
        }

        var session = await _financeRepository.GetSessionByConversationRefAsync(conversation.Id, cancellationToken);
        if (session != null)
        {
            // Đồng bộ số dư frozen và trạng thái escrow để client hiển thị ngay trên màn hình chat.
            conversation.EscrowTotalFrozen = session.TotalFrozen;
            conversation.EscrowStatus = session.Status;
        }
    }

    /// <summary>
    /// Enrich trạng thái đánh giá reader của conversation cho requester hiện tại.
    /// Luồng xử lý: chỉ bật review khi requester là user của conversation và trạng thái đã completed.
    /// </summary>
    private async Task EnrichConversationReviewStateAsync(
        ConversationDto conversation,
        string requesterId,
        CancellationToken cancellationToken)
    {
        conversation.CanSubmitReview = false;
        conversation.HasSubmittedReview = false;
        conversation.ReviewedAt = null;

        if (conversation.UserId != requesterId
            || conversation.Status != ConversationStatus.Completed)
        {
            return;
        }

        conversation.CanSubmitReview = true;

        var review = await _conversationReviewRepository.GetByConversationAndUserAsync(
            conversation.Id,
            requesterId,
            cancellationToken);
        if (review == null)
        {
            return;
        }

        conversation.HasSubmittedReview = true;
        conversation.CanSubmitReview = false;
        conversation.ReviewedAt = review.CreatedAt;
    }
}
