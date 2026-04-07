

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IChatMessageRepository
{
    Task<ChatMessageDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId, int page, int pageSize, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ChatMessageDto> Items, string? NextCursor)> GetByConversationIdCursorAsync(
        string conversationId,
        string? cursor,
        int limit,
        CancellationToken cancellationToken = default);

    Task<bool> HasPaymentOfferResponseAsync(
        string conversationId,
        string offerMessageId,
        CancellationToken cancellationToken = default);

    Task<ChatMessageDto?> FindLatestPendingPaymentOfferAsync(
        string conversationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChatMessageDto>> GetExpiredPendingPaymentOffersAsync(
        DateTime nowUtc,
        int limit = 200,
        CancellationToken cancellationToken = default);

        Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default);

        Task<IEnumerable<ChatMessageDto>> GetLatestMessagesAsync(IEnumerable<string> conversationIds, CancellationToken cancellationToken = default);

        Task UpdateFlagAsync(string messageId, bool isFlagged, CancellationToken cancellationToken = default);
}
