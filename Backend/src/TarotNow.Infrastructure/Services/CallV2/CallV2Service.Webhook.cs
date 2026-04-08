using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2Service
{
    private const string ParticipantJoinedEvent = "participant_joined";

    public async Task HandleWebhookAsync(string authorizationHeader, string payload, CancellationToken ct = default)
    {
        if (_tokenFactory.ValidateWebhookToken(authorizationHeader, payload) == false)
        {
            throw new UnauthorizedAccessException();
        }

        var webhook = ParseWebhook(payload);
        if (webhook == null || string.IsNullOrWhiteSpace(webhook.RoomName)) return;

        var session = await _sessions.GetByRoomNameAsync(webhook.RoomName, ct);
        if (session == null) return;

        if (IsParticipantJoinedEvent(webhook.EventName))
        {
            await HandleParticipantJoinedAsync(session, webhook.ParticipantIdentity, ct);
            return;
        }

        if (IsRoomEndedEvent(webhook.EventName))
        {
            await FinalizeSessionAsync(session, "disconnected", ct);
        }
    }

    private async Task HandleParticipantJoinedAsync(CallSessionV2Dto session, string participantIdentity, CancellationToken ct)
    {
        if (CallSessionV2Statuses.IsFinal(session.Status)) return;

        // Tải lại dữ liệu phiên gọi mới nhất từ Database để tránh Race Condition (lỗi chạy đua).
        // Đảm bảo rằng nếu người kia đã tham gia trước đó 1 mili giây, chúng ta sẽ thấy được mốc thời gian của họ.
        var freshSession = await _sessions.GetByIdAsync(session.Id, ct) ?? session;

        if (TryResolveParticipantRole(freshSession, participantIdentity, out var isInitiator, out var isCallee) == false) return;

        var patch = BuildParticipantJoinedPatch(freshSession, DateTime.UtcNow, isInitiator, isCallee);
        var updated = await _sessions.TryPatchAsync(freshSession.Id, patch, ct);

        if (freshSession.ConnectedAt.HasValue == false && updated?.ConnectedAt.HasValue == true)
        {
            _logger.LogInformation("[CallV2] Cuộc gọi {SessionId} chính thức chuyển sang trạng thái Connected.", freshSession.Id);
            CallV2Telemetry.RecordJoinSuccess(updated.Type);
        }
    }
}
