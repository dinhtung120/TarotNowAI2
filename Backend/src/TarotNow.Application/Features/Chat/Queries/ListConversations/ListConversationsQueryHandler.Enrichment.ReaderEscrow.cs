using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    private async Task EnrichEscrowSummaryAsync(
        List<ConversationDto> conversations,
        CancellationToken cancellationToken)
    {
        if (conversations.Count == 0)
        {
            return;
        }

        var refs = conversations.Select(item => item.Id).ToList();
        var activeSessions = await _financeRepo.GetSessionsByConversationRefsAsync(refs, cancellationToken);
        
        /*
         * CHÚ Ý: ToLookup thay vì ToDictionary để tránh lỗi crash 500 nếu một cuộc hội thoại
         * lỡ có nhiều hơn 1 phiên tài chính (ví dụ do lỗi data hoặc các phiên cũ).
         * Chúng ta sẽ lấy phiên MỚI NHẤT (dựa vào UpdatedAt/CreatedAt) để hiển thị.
         */
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
            return;
        }

        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        var profiles = await _readerProfileRepo.GetByUserIdsAsync(readerIds, cancellationToken);
        
        foreach (var profile in profiles)
        {
            if (string.IsNullOrEmpty(profile.UserId)) continue;
            var status = profile.Status;
                
            if (_presenceTracker.IsOnline(profile.UserId))
            {
                if (string.Equals(status, "offline", StringComparison.OrdinalIgnoreCase))
                {
                    status = "online";
                }
            }

            map[profile.UserId] = status;
        }

        foreach (var conversation in conversations)
        {
            if (!string.IsNullOrEmpty(conversation.ReaderId) && map.TryGetValue(conversation.ReaderId, out var status))
            {
                conversation.ReaderStatus = status;
            }
        }
    }
}
