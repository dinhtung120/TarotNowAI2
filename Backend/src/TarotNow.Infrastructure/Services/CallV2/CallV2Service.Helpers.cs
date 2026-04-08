using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2Service
{
    private static string NormalizeCallTypeOrThrow(string callType)
    {
        if (CallTypeValues.IsSupported(callType) == false)
        {
            throw new BadRequestException("Loại cuộc gọi không hợp lệ.");
        }

        return CallTypeValues.Normalize(callType);
    }

    private async Task<CallSessionV2Dto> GetSessionOrThrowAsync(string callSessionId, CancellationToken ct)
    {
        return await _sessions.GetByIdAsync(callSessionId, ct)
            ?? throw new NotFoundException("Không tìm thấy phiên gọi.");
    }

    private async Task<ConversationDto> LoadConversationOrThrowAsync(string conversationId, CancellationToken ct)
    {
        return await _conversations.GetByIdAsync(conversationId, ct)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");
    }

    private static void EnsureConversationOngoingOrThrow(ConversationDto conversation)
    {
        if (string.Equals(conversation.Status, "ongoing", StringComparison.OrdinalIgnoreCase)) return;
        throw new BusinessRuleException(CallV2ErrorCodes.CallNotAllowed, "Chỉ có thể gọi khi hội thoại đang ở trạng thái ongoing.");
    }

    private static void EnsureConversationParticipantOrThrow(ConversationDto conversation, string requesterId)
    {
        if (conversation.UserId == requesterId || conversation.ReaderId == requesterId) return;
        throw new BusinessRuleException(CallV2ErrorCodes.CallNotAllowed, "Bạn không có quyền thao tác cuộc gọi này.");
    }

    private static void EnsureConversationParticipantOrThrow(CallSessionV2Dto session, string requesterId)
    {
        if (session.InitiatorId == requesterId || session.CalleeId == requesterId) return;
        throw new BusinessRuleException(CallV2ErrorCodes.CallNotAllowed, "Bạn không có quyền thao tác cuộc gọi này.");
    }

    private static void EnsureCalleeForAcceptOrThrow(CallSessionV2Dto session, string requesterId)
    {
        if (session.CalleeId == requesterId) return;
        throw new BusinessRuleException(CallV2ErrorCodes.CallNotAllowed, "Chỉ người nhận cuộc gọi mới được chấp nhận cuộc gọi.");
    }

    private async Task EnsureNotExpiredRequestedCallAsync(CallSessionV2Dto session, CancellationToken ct)
    {
        if (string.Equals(session.Status, CallSessionV2Statuses.Requested, StringComparison.OrdinalIgnoreCase) == false) return;
        var timeout = TimeSpan.FromSeconds(Math.Max(5, _callOptions.RingTimeoutSeconds));
        if (session.CreatedAt.Add(timeout) >= DateTime.UtcNow) return;

        await FinalizeSessionAsync(session, "timeout_server", ct);
        throw new BusinessRuleException(CallV2ErrorCodes.JoinTimeout, "Cuộc gọi đã hết thời gian chờ kết nối.");
    }

    private string ResolvePeerId(ConversationDto conversation, string requesterId)
    {
        return conversation.UserId == requesterId ? conversation.ReaderId : conversation.UserId;
    }

    private string BuildRoomName(string conversationId)
    {
        return $"tn-{conversationId}-{Guid.NewGuid():N}".ToLowerInvariant();
    }

    private CallJoinTicketDto BuildJoinTicket(CallSessionV2Dto session, string userId)
    {
        if (string.IsNullOrWhiteSpace(_liveKitOptions.Url))
        {
            throw new BusinessRuleException(CallV2ErrorCodes.RoomUnavailable, "Thiếu cấu hình LiveKit URL.");
        }

        var participantIdentity = BuildParticipantIdentity(userId);
        return new CallJoinTicketDto
        {
            Session = session,
            LiveKitUrl = _liveKitOptions.Url,
            AccessToken = _tokenFactory.CreateParticipantToken(participantIdentity, session.RoomName),
            ParticipantIdentity = participantIdentity,
            Timeouts = GetTimeouts(),
        };
    }

    private static string BuildParticipantIdentity(string userId)
    {
        return $"user:{userId}";
    }

}
