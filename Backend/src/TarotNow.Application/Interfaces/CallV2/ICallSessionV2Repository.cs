using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICallSessionV2Repository
{
    Task AddAsync(CallSessionV2Dto session, CancellationToken ct = default);

    Task<CallSessionV2Dto?> GetByIdAsync(string id, CancellationToken ct = default);

    Task<CallSessionV2Dto?> GetByRoomNameAsync(string roomName, CancellationToken ct = default);

    Task<CallSessionV2Dto?> GetActiveByConversationAsync(string conversationId, CancellationToken ct = default);

    Task<CallSessionV2Dto?> TryPatchAsync(string id, CallSessionV2Patch patch, CancellationToken ct = default);

    Task<IReadOnlyList<CallSessionV2Dto>> ListStaleByStatusAsync(
        IReadOnlyCollection<string> statuses,
        DateTime updatedBeforeUtc,
        int limit,
        CancellationToken ct = default);
}
