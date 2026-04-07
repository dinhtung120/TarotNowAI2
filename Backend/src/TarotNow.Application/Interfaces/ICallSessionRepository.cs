using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Interfaces;

public interface ICallSessionRepository
{
        Task AddAsync(CallSessionDto session, CancellationToken ct = default);

        Task<CallSessionDto?> GetByIdAsync(string id, CancellationToken ct = default);

        Task<CallSessionDto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default);

        Task<IEnumerable<CallSessionDto>> GetActiveByConversationIdsAsync(IEnumerable<string> conversationIds, CancellationToken ct = default);

        Task<bool> UpdateStatusAsync(
        string id,
        CallSessionStatus newStatus,
        DateTime? startedAt = null,
        DateTime? endedAt = null,
        string? endReason = null,
        CallSessionStatus? expectedPreviousStatus = null,
        CancellationToken ct = default);

        Task<(IEnumerable<CallSessionDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId,
        int page,
        int pageSize,
        CancellationToken ct = default);
}
