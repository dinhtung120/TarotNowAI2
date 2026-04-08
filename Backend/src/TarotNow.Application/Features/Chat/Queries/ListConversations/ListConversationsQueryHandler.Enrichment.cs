using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    /// <summary>
    /// Tải danh sách conversation theo participant + tab inbox.
    /// Luồng xử lý: chuyển tab thành tập trạng thái, gọi repository phân trang, rồi trả danh sách và totalCount.
    /// </summary>
    private async Task<(List<ConversationDto> Items, long TotalCount)> LoadConversationsAsync(
        string userId,
        ListConversationsQuery request,
        CancellationToken cancellationToken)
    {
        var statuses = ResolveInboxStatuses(request.Tab);
        var page = await _conversationRepo.GetByParticipantIdPaginatedAsync(
            userId,
            request.Page,
            request.PageSize,
            statuses,
            cancellationToken);

        return (page.Items.ToList(), page.TotalCount);
    }

    /// <summary>
    /// Chuyển tab inbox sang tập trạng thái conversation tương ứng.
    /// Luồng xử lý: tab all trả null (không lọc), các tab còn lại map sang nhóm trạng thái theo nghiệp vụ.
    /// </summary>
    private static IReadOnlyCollection<string>? ResolveInboxStatuses(string? tab)
    {
        if (string.Equals(tab, "all", StringComparison.OrdinalIgnoreCase))
        {
            // Tab all hiển thị toàn bộ nên không áp filter trạng thái.
            return null;
        }

        if (string.Equals(tab, "completed", StringComparison.OrdinalIgnoreCase))
        {
            // Nhóm completed bao gồm cả cuộc trò chuyện đã hoàn tất/hủy/hết hạn.
            return
            [
                ConversationStatus.Completed,
                ConversationStatus.Cancelled,
                ConversationStatus.Expired
            ];
        }

        if (string.Equals(tab, "pending", StringComparison.OrdinalIgnoreCase))
        {
            // Tab pending chỉ hiển thị các cuộc trò chuyện chưa bắt đầu xử lý chính thức.
            return
            [
                ConversationStatus.Pending
            ];
        }

        // Mặc định tab active gồm các cuộc trò chuyện còn hoạt động.
        return
        [
            ConversationStatus.AwaitingAcceptance,
            ConversationStatus.Ongoing,
            ConversationStatus.Disputed
        ];
    }
}
