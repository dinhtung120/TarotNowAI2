using System.Text.Json;
using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2Service
{
    private static readonly string[] RoomEndedEvents = ["participant_left", "room_finished", "room_deleted"];

    private static string NormalizeEndReason(string reason)
    {
        return string.IsNullOrWhiteSpace(reason) ? "normal" : reason.Trim().ToLowerInvariant();
    }

    private static bool IsIdentityForUser(string participantIdentity, string userId)
    {
        return string.Equals(participantIdentity, userId, StringComparison.OrdinalIgnoreCase)
            || string.Equals(participantIdentity, BuildParticipantIdentity(userId), StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsParticipantJoinedEvent(string eventName)
    {
        return string.Equals(eventName, ParticipantJoinedEvent, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsRoomEndedEvent(string eventName)
    {
        return RoomEndedEvents.Any(evt => string.Equals(evt, eventName, StringComparison.OrdinalIgnoreCase));
    }

    private static bool TryResolveParticipantRole(
        CallSessionV2Dto session,
        string participantIdentity,
        out bool isInitiator,
        out bool isCallee)
    {
        isInitiator = IsIdentityForUser(participantIdentity, session.InitiatorId);
        isCallee = IsIdentityForUser(participantIdentity, session.CalleeId);
        return isInitiator || isCallee;
    }

    private static CallSessionV2Patch BuildParticipantJoinedPatch(
        CallSessionV2Dto session,
        DateTime now,
        bool isInitiator,
        bool isCallee)
    {
        var initiatorJoinedAt = session.InitiatorJoinedAt ?? (isInitiator ? now : null);
        var calleeJoinedAt = session.CalleeJoinedAt ?? (isCallee ? now : null);
        var hasBothJoined = initiatorJoinedAt.HasValue && calleeJoinedAt.HasValue;

        return new CallSessionV2Patch
        {
            NewStatus = hasBothJoined ? CallSessionV2Statuses.Connected : CallSessionV2Statuses.Joining,
            ConnectedAt = hasBothJoined ? now : null,
            InitiatorJoinedAt = isInitiator ? now : null,
            CalleeJoinedAt = isCallee ? now : null,
            ExpectedPreviousStatuses =
            [
                CallSessionV2Statuses.Requested,
                CallSessionV2Statuses.Accepted,
                CallSessionV2Statuses.Joining,
                CallSessionV2Statuses.Connected,
            ],
        };
    }

    private static LiveKitWebhookPayload? ParseWebhook(string payload)
    {
        try
        {
            using var document = JsonDocument.Parse(payload);
            var root = document.RootElement;

            var eventName = root.TryGetProperty("event", out var eventProp)
                ? eventProp.GetString() ?? string.Empty
                : string.Empty;

            var roomName = root.TryGetProperty("room", out var roomProp)
                && roomProp.TryGetProperty("name", out var nameProp)
                ? nameProp.GetString() ?? string.Empty
                : string.Empty;

            var participantIdentity = root.TryGetProperty("participant", out var participantProp)
                && participantProp.TryGetProperty("identity", out var identityProp)
                ? identityProp.GetString() ?? string.Empty
                : string.Empty;

            return new LiveKitWebhookPayload(eventName, roomName, participantIdentity);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed record LiveKitWebhookPayload(string EventName, string RoomName, string ParticipantIdentity);
}
