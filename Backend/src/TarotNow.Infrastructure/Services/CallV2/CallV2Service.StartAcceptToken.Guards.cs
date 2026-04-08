using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2Service
{
    private async Task EnsureNoActiveCallOrThrowAsync(string conversationId, CancellationToken ct)
    {
        var activeSession = await _sessions.GetActiveByConversationAsync(conversationId, ct);
        if (activeSession == null) return;

        if (TryGetStaleRecoveryReason(activeSession, out var recoveryReason))
        {
            await FinalizeSessionAsync(activeSession, recoveryReason, ct);
            return;
        }

        throw new BusinessRuleException(CallV2ErrorCodes.CallAlreadyActive, "Đang có cuộc gọi khác diễn ra trong hội thoại này.");
    }

    private async Task EnsureRoomCreatedOrThrowAsync(string roomName, CancellationToken ct)
    {
        if (await _rooms.CreateRoomAsync(roomName, ct)) return;
        throw new BusinessRuleException(CallV2ErrorCodes.RoomUnavailable, "Không thể khởi tạo phòng gọi tại thời điểm hiện tại.");
    }

    private async Task<CallSessionV2Dto> PersistSessionOrRollbackRoomAsync(CallSessionV2Dto session, CancellationToken ct)
    {
        try
        {
            await _sessions.AddAsync(session, ct);
            return session;
        }
        catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            await _rooms.DeleteRoomAsync(session.RoomName, ct);
            throw new BusinessRuleException(CallV2ErrorCodes.CallAlreadyActive, "Đang có cuộc gọi khác diễn ra trong hội thoại này.");
        }
    }

    private bool TryGetStaleRecoveryReason(CallSessionV2Dto session, out string reason)
    {
        var now = DateTime.UtcNow;
        var ringTimeout = TimeSpan.FromSeconds(Math.Max(5, _callOptions.RingTimeoutSeconds));
        var joinTimeout = TimeSpan.FromSeconds(Math.Max(5, _callOptions.JoinTimeoutSeconds));

        if (string.Equals(session.Status, CallSessionV2Statuses.Requested, StringComparison.OrdinalIgnoreCase)
            && session.CreatedAt.Add(ringTimeout) < now)
        {
            reason = "timeout_server";
            return true;
        }

        if ((string.Equals(session.Status, CallSessionV2Statuses.Accepted, StringComparison.OrdinalIgnoreCase)
            || string.Equals(session.Status, CallSessionV2Statuses.Joining, StringComparison.OrdinalIgnoreCase))
            && session.UpdatedAt.Add(joinTimeout) < now)
        {
            reason = "join_timeout";
            return true;
        }

        if (string.Equals(session.Status, CallSessionV2Statuses.Ending, StringComparison.OrdinalIgnoreCase)
            && session.UpdatedAt.Add(joinTimeout) < now)
        {
            reason = "stale_ending";
            return true;
        }

        reason = string.Empty;
        return false;
    }
}
