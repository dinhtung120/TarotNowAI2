

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IConversationRepository
{
        Task AddAsync(ConversationDto conversation, CancellationToken cancellationToken = default);

        Task<ConversationDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId, string readerId, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

        Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByParticipantIdPaginatedAsync(
        string participantId, int page, int pageSize, IReadOnlyCollection<string>? statuses = null, CancellationToken cancellationToken = default);

        Task<long> CountActiveByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ConversationDto>> GetConversationsAwaitingCompletionResolutionAsync(
        DateTime dueAtUtc,
        int limit = 200,
        CancellationToken cancellationToken = default);

        Task<int> GetTotalUnreadCountAsync(string participantId, CancellationToken cancellationToken = default);

        Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default);
}
