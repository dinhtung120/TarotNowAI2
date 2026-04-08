using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2MaintenanceService
{
    private async Task FinalizeAsync(CallSessionV2Dto session, string finalStatus, string reason, CancellationToken cancellationToken)
    {
        if (CallSessionV2Statuses.IsFinal(session.Status)) return;

        var patched = await _sessions.TryPatchAsync(session.Id, new CallSessionV2Patch
        {
            NewStatus = finalStatus,
            EndedAt = DateTime.UtcNow,
            EndReason = reason,
            ExpectedPreviousStatuses =
            [
                CallSessionV2Statuses.Requested,
                CallSessionV2Statuses.Accepted,
                CallSessionV2Statuses.Joining,
                CallSessionV2Statuses.Connected,
                CallSessionV2Statuses.Ending,
            ],
        }, cancellationToken);

        if (patched == null) return;

        try
        {
            await _realtimePush.BroadcastEndedAsync(patched, reason, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không thể phát sự kiện timeout cho call session {SessionId}", session.Id);
        }
    }
}
