using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2Service
{
    public async Task<CallJoinTicketDto> StartAsync(Guid requesterId, string conversationId, string callType, CancellationToken ct = default)
    {
        var normalizedType = NormalizeCallTypeOrThrow(callType);
        var requesterIdText = requesterId.ToString();
        var conversation = await LoadConversationOrThrowAsync(conversationId, ct);

        EnsureConversationParticipantOrThrow(conversation, requesterIdText);
        EnsureConversationOngoingOrThrow(conversation);
        await EnsureNoActiveCallOrThrowAsync(conversationId, ct);

        var calleeId = ResolvePeerId(conversation, requesterIdText);
        var roomName = BuildRoomName(conversationId);
        await EnsureRoomCreatedOrThrowAsync(roomName, ct);

        var now = DateTime.UtcNow;
        var session = new CallSessionV2Dto
        {
            ConversationId = conversationId,
            RoomName = roomName,
            InitiatorId = requesterIdText,
            CalleeId = calleeId,
            Type = normalizedType,
            Status = CallSessionV2Statuses.Requested,
            CreatedAt = now,
            UpdatedAt = now,
        };

        session = await PersistSessionOrRollbackRoomAsync(session, ct);
        await _realtimePush.BroadcastIncomingAsync(session, GetTimeouts(), ct);
        return BuildJoinTicket(session, requesterIdText);
    }

    public async Task<CallJoinTicketDto> AcceptAsync(Guid requesterId, string callSessionId, CancellationToken ct = default)
    {
        var requesterIdText = requesterId.ToString();
        var session = await GetSessionOrThrowAsync(callSessionId, ct);

        EnsureConversationParticipantOrThrow(session, requesterIdText);
        EnsureCalleeForAcceptOrThrow(session, requesterIdText);
        await EnsureNotExpiredRequestedCallAsync(session, ct);

        var broadcastAccepted = string.Equals(session.Status, CallSessionV2Statuses.Requested, StringComparison.OrdinalIgnoreCase);
        session = await EnsureAcceptedStatusAsync(session, ct);

        if (broadcastAccepted)
        {
            await _realtimePush.BroadcastAcceptedAsync(session, GetTimeouts(), ct);
        }

        return BuildJoinTicket(session, requesterIdText);
    }

    public async Task<CallJoinTicketDto> IssueTokenAsync(Guid requesterId, string callSessionId, CancellationToken ct = default)
    {
        var requesterIdText = requesterId.ToString();
        var session = await GetSessionOrThrowAsync(callSessionId, ct);

        EnsureConversationParticipantOrThrow(session, requesterIdText);
        if (CallSessionV2Statuses.IsActive(session.Status) == false)
        {
            throw new BusinessRuleException(CallV2ErrorCodes.TokenExpired, "Phiên gọi đã kết thúc hoặc không còn hiệu lực.");
        }

        CallV2Telemetry.RecordReconnectTokenIssued();
        return BuildJoinTicket(session, requesterIdText);
    }

    private async Task<CallSessionV2Dto> EnsureAcceptedStatusAsync(CallSessionV2Dto session, CancellationToken ct)
    {
        if (string.Equals(session.Status, CallSessionV2Statuses.Requested, StringComparison.OrdinalIgnoreCase) == false)
        {
            return session;
        }

        var updated = await _sessions.TryPatchAsync(session.Id, new CallSessionV2Patch
        {
            NewStatus = CallSessionV2Statuses.Accepted,
            AcceptedAt = DateTime.UtcNow,
            ExpectedPreviousStatuses = [CallSessionV2Statuses.Requested],
        }, ct);

        if (updated != null) return updated;
        return await GetSessionOrThrowAsync(session.Id, ct);
    }
}
