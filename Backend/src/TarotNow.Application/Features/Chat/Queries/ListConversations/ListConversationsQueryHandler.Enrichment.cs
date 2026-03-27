using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
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

    private static IReadOnlyCollection<string>? ResolveInboxStatuses(string? tab)
    {
        if (string.Equals(tab, "all", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (string.Equals(tab, "completed", StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                ConversationStatus.Completed,
                ConversationStatus.Cancelled,
                ConversationStatus.Expired
            ];
        }

        if (string.Equals(tab, "pending", StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                ConversationStatus.Pending
            ];
        }

        return
        [
            ConversationStatus.AwaitingAcceptance,
            ConversationStatus.Ongoing,
            ConversationStatus.Disputed
        ];
    }

}
