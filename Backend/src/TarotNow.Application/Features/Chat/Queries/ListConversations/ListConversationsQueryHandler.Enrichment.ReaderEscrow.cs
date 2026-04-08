using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    /// <summary>
    /// Enrich thông tin escrow summary cho từng conversation.
    /// Luồng xử lý: lấy session theo danh sách conversation id, chọn session mới nhất mỗi conversation rồi gán tổng frozen + trạng thái escrow.
    /// </summary>
    private async Task EnrichEscrowSummaryAsync(
        List<ConversationDto> conversations,
        CancellationToken cancellationToken)
    {
        if (conversations.Count == 0)
        {
            // Không có conversation thì không cần truy vấn finance session.
            return;
        }

        var refs = conversations.Select(item => item.Id).ToList();
        var activeSessions = await _financeRepo.GetSessionsByConversationRefsAsync(refs, cancellationToken);

        // Chọn session mới nhất theo UpdatedAt/CreatedAt để phản ánh trạng thái escrow hiện tại.
        var sessionMap = activeSessions
            .OrderByDescending(s => s.UpdatedAt ?? s.CreatedAt)
            .GroupBy(s => s.ConversationRef)
            .Where(g => !string.IsNullOrEmpty(g.Key))
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var conversation in conversations)
        {
            if (sessionMap.TryGetValue(conversation.Id, out var session))
            {
                conversation.EscrowTotalFrozen = session.TotalFrozen;
                conversation.EscrowStatus = session.Status;
            }
        }
    }

    /// <summary>
    /// Enrich trạng thái online/offline của reader cho danh sách conversation.
    /// Luồng xử lý: tải profile reader theo lô, nâng trạng thái lên Online khi presence tracker báo online, rồi gán lại vào conversation.
    /// </summary>
    private async Task EnrichReaderStatusAsync(
        IEnumerable<ConversationDto> conversations,
        CancellationToken cancellationToken)
    {
        var readerIds = conversations
            .Select(item => item.ReaderId)
            .Where(item => string.IsNullOrWhiteSpace(item) == false)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        if (readerIds.Count == 0)
        {
            // Không có reader id hợp lệ thì không enrich trạng thái.
            return;
        }

        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        var profiles = await _readerProfileRepo.GetByUserIdsAsync(readerIds, cancellationToken);

        foreach (var profile in profiles)
        {
            if (string.IsNullOrEmpty(profile.UserId))
            {
                // Edge case profile thiếu user id thì bỏ qua để tránh key rỗng.
                continue;
            }

            var status = profile.Status;

            if (_presenceTracker.IsOnline(profile.UserId))
            {
                if (string.Equals(status, ReaderOnlineStatus.Offline, StringComparison.OrdinalIgnoreCase))
                {
                    // Ưu tiên trạng thái realtime online từ presence tracker.
                    status = ReaderOnlineStatus.Online;
                }
            }

            map[profile.UserId] = status;
        }

        foreach (var conversation in conversations)
        {
            if (!string.IsNullOrEmpty(conversation.ReaderId) && map.TryGetValue(conversation.ReaderId, out var status))
            {
                // Gán trạng thái reader đã enrich cho conversation tương ứng.
                conversation.ReaderStatus = status;
            }
        }
    }
}
